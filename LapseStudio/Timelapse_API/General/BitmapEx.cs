using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Timelapse_API
{
	[Serializable]
	public class BitmapEx : IDisposable
	{
        /* Example usage (for 8bit RGB)

            bmp.LockBits();
            uint x, y;
            long index;
            byte* row;
            for (y = 0; y < bmp.Height; y++)
            {
                row = (byte*)bmp.Scan0 + y * bmp.Stride;
                for (x = 0; x < bmp.Width; x++)
                {
                    index = x * 3;
                    row[index] = 125;       //R
                    row[index + 1] = 125;   //G
                    row[index + 2] = 125;   //B
                }
            }
            bmp.UnlockBits();
            
         *  OR:
         
            bmp.LockBits();
            uint x, y;
            long index;
            byte* pix = (byte*)bmp.Scan0;
            for (y = 0; y < bmp.Height; y++)
            {
                for (x = 0; x < bmp.Width; x++)
                {
                    index = y * bmp.Stride + (x * bmp.ChannelCount);        
                    pix[index] = 125;       //R
                    pix[index + 1] = 125;   //G
                    pix[index + 2] = 125;   //B
                }
            }
            bmp.UnlockBits();
         */

        public uint Width { get; protected set; }
		public uint Height { get; protected set; }
		public ImageType BitDepth { get; protected set; }
		public byte ChannelCount { get; private set; }
        public byte BytePerChannel { get; private set; }

		[NonSerialized]
		private GCHandle pinHandle;
		private bool IsDisposed;
		private byte[] ImageData;

		public bool IsPinned { get; private set; }
		public IntPtr Scan0 { get; private set; }
		public uint Stride { get; private set; }
		public uint Length { get { return Height * Stride; } }

		#region Constructor/Init

        /// <summary>
        /// Loads an image from a path. (Jpg, Png, Tiff, Bmp, Gif and Wdp are supported)
        /// </summary>
        /// <param name="path">Path to the image</param>
		public BitmapEx(string path)
		{
            using (Stream str = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BitmapDecoder dec = BitmapDecoder.Create(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                if (dec.Frames.Count > 0) SetFromBitmapSource(dec.Frames[0]);
                else throw new FileLoadException("Couldn't load file " + path);
            }
		}
        
        /// <summary>
        /// Loads an image from an encoded stream. (Jpg, Png, Tiff, Bmp, Gif and Wdp are supported)
        /// </summary>
		public BitmapEx(Stream encodedStream)
		{
            encodedStream.Position = 0;
            BitmapDecoder dec = BitmapDecoder.Create(encodedStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            if (dec.Frames.Count > 0) SetFromBitmapSource(dec.Frames[0]);
            else throw new FileLoadException("Couldn't load file");
		}

        /// <summary>
        /// Loads an image from an encoded byte array. (Jpg, Png, Tiff, Bmp, Gif and Wdp are supported)
        /// </summary>
        public BitmapEx(byte[] encodedData)
        {
            using (MemoryStream str = new MemoryStream(encodedData))
            {
                BitmapDecoder dec = BitmapDecoder.Create(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                if (dec.Frames.Count > 0) SetFromBitmapSource(dec.Frames[0]);
                else throw new FileLoadException("Couldn't load file");
            }
        }

        /// <summary>
        /// Creates a new empty BitmapEx.
        /// </summary>
        /// <param name="Width">Width in pixels</param>
        /// <param name="Height">Height in pixels</param>
        /// <param name="BitDepth">Bitdepth of pixel data</param>
        public BitmapEx(uint Width, uint Height, ImageType BitDepth)
        {
            this.Width = Width;
            this.Height = Height;
            this.BitDepth = BitDepth;
            SetInitValues();
            this.ImageData = new byte[Height * Stride];
        }

        /// <summary>
        /// Creates a new BitmapEx from raw pixel data.
        /// </summary>
        /// <param name="RawData">RGB(A) pixel values</param>
        /// <param name="Width">Width in pixels</param>
        /// <param name="Height">Height in pixels</param>
        /// <param name="BitDepth">Bitdepth of pixel data</param>
		public BitmapEx(byte[] RawData, uint Width, uint Height, ImageType BitDepth)
		{
			this.ImageData = RawData;
			this.Width = Width;
			this.Height = Height;
			this.BitDepth = BitDepth;
			SetInitValues();
		}
                
		#endregion

        #region Subroutines

        private PixelFormat GetPixelFormat(ImageType BitDepth, out bool SwitchChannels)
        {
            PixelFormat pixFormat;
            if (this.BitDepth == ImageType.RGB8)
            {
                pixFormat = PixelFormats.Bgr24;
                SwitchChannels = true;
            }
            else if (this.BitDepth == ImageType.RGBA8)
            {
                pixFormat = PixelFormats.Bgra32;
                SwitchChannels = true;
            }
            else if (this.BitDepth == ImageType.RGB16)
            {
                pixFormat = PixelFormats.Rgb48;
                SwitchChannels = false;
            }
            else if (this.BitDepth == ImageType.RGB32)
            {
                pixFormat = PixelFormats.Rgb128Float;
                SwitchChannels = false;
            }
            else if (this.BitDepth == ImageType.RGBA32)
            {
                pixFormat = PixelFormats.Rgba128Float;
                SwitchChannels = false;
            }
            else if (this.BitDepth == ImageType.RGBA16)
            {
                pixFormat = PixelFormats.Rgba64;
                SwitchChannels = false;
            }
            else throw new ArgumentException("Pixel format not supported");

            return pixFormat;
        }

        private ImageType GetBitDepth(PixelFormat pixFormat, out bool SwitchChannels)
        {
            ImageType bitDepth;
            if (pixFormat == PixelFormats.Bgr24
                || (pixFormat == PixelFormats.Default && pixFormat.BitsPerPixel == 24))
            {
                bitDepth = ImageType.RGB8;
                SwitchChannels = true;
            }
            else if (pixFormat == PixelFormats.Bgr32 || pixFormat == PixelFormats.Bgra32 || pixFormat == PixelFormats.Pbgra32
                || (pixFormat == PixelFormats.Default && pixFormat.BitsPerPixel == 32))
            {
                bitDepth = ImageType.RGBA8;
                SwitchChannels = true;
            }
            else if (pixFormat == PixelFormats.Rgb24)
            {
                bitDepth = ImageType.RGB8;
                SwitchChannels = false;
            }
            else if (pixFormat == PixelFormats.Rgb48
                || (pixFormat == PixelFormats.Default && pixFormat.BitsPerPixel == 48))
            {
                bitDepth = ImageType.RGB16;
                SwitchChannels = false;
            }
            else if (pixFormat == PixelFormats.Rgb128Float
                || (pixFormat == PixelFormats.Default && pixFormat.BitsPerPixel == 128))
            {
                bitDepth = ImageType.RGB32;
                SwitchChannels = false;
            }
            else if (pixFormat == PixelFormats.Rgba128Float || pixFormat == PixelFormats.Prgba128Float)
            {
                bitDepth = ImageType.RGBA32;
                SwitchChannels = false;
            }
            else if (pixFormat == PixelFormats.Rgba64 || pixFormat == PixelFormats.Prgba64
                || (pixFormat == PixelFormats.Default && pixFormat.BitsPerPixel == 64))
            {
                bitDepth = ImageType.RGBA16;
                SwitchChannels = false;
            }
            else throw new ArgumentException("Pixel format not supported");

            return bitDepth;
        }

        private void SetInitValues()
        {
            switch (BitDepth)
            {
                case ImageType.RGB8:
                    BytePerChannel = 1;
                    Stride = Width * 3;
                    ChannelCount = 3;
                    break;
                case ImageType.RGB16:
                    BytePerChannel = 2;
                    Stride = Width * 3;
                    ChannelCount = 3;
                    break;
                case ImageType.RGB32:
                    BytePerChannel = 4;
                    Stride = Width * 3;
                    ChannelCount = 3;
                    break;
                case ImageType.RGB64:
                    BytePerChannel = 8;
                    Stride = Width * 3;
                    ChannelCount = 3;
                    break;
                case ImageType.RGBA8:
                    BytePerChannel = 1;
                    Stride = Width * 4;
                    ChannelCount = 4;
                    break;
                case ImageType.RGBA16:
                    BytePerChannel = 2;
                    Stride = Width * 4;
                    ChannelCount = 4;
                    break;
                case ImageType.RGBA32:
                    BytePerChannel = 4;
                    Stride = Width * 4;
                    ChannelCount = 4;
                    break;
                case ImageType.RGBA64:
                    BytePerChannel = 8;
                    Stride = Width * 4;
                    ChannelCount = 4;
                    break;
            }
        }

        private void SetFromBitmapSource(BitmapSource bmpSrc)
        {
            bool SwitchChannels = false;
            this.BitDepth = GetBitDepth(bmpSrc.Format, out SwitchChannels);

            this.Width = (uint)bmpSrc.PixelWidth;
            this.Height = (uint)bmpSrc.PixelHeight;
            SetInitValues();
            int bpc = this.BytePerChannel;

            ImageData = new byte[this.Width * this.Height * bpc * this.ChannelCount];
            bmpSrc.CopyPixels(ImageData, (int)this.Stride * bpc, 0);

            if (SwitchChannels)
            {
                //Make RGB from BGR
                unsafe
                {
                    byte[] tmp = new byte[bpc];
                    fixed (byte* pix = ImageData)
                    fixed (byte* tmpP = tmp)
                    {
                        int j;
                        long bpc2 = 2 * bpc;
                        for (long i = 0; i < this.Height * this.Width * this.ChannelCount; i += bpc * this.ChannelCount)
                        {
                            for (j = 0; j < bpc; j++)
                            {
                                tmpP[j] = pix[i + j];           //Save B values
                                pix[i + j] = pix[i + j + bpc2]; //Set B values with R values
                            }
                            for (j = 0; j < bpc; j++) pix[i + j + bpc2] = tmpP[j]; //Set R values from save B values
                        }
                    }
                }
            }
        }

        #endregion

        #region Lock/Unlock/Dispose/Clone

        public void LockBits()
		{
			if(IsDisposed) throw new ObjectDisposedException("BitmapEx");
			if(!IsPinned)
			{
				pinHandle = GCHandle.Alloc(ImageData, GCHandleType.Pinned);
				Scan0 = pinHandle.AddrOfPinnedObject();
				IsPinned = true;
			}
		}

		public void UnlockBits()
		{
			if(IsDisposed) throw new ObjectDisposedException("BitmapEx");
			if(IsPinned)
			{
				pinHandle.Free();
				IsPinned = false;
			}
		}

		public void Dispose()
		{
			if(IsPinned) UnlockBits();
			ImageData = null;
			Width = 0;
			Height = 0;
			ChannelCount = 0;
			IsDisposed = true;
		}

		public BitmapEx Clone()
		{
			if (IsDisposed) throw new ObjectDisposedException("BitmapEx");
			return new BitmapEx((byte[])ImageData.Clone(), Width, Height, BitDepth);
		}

		#endregion

        #region Saving

        public void Save(string path)
        {
            FileFormat format;
            string ext = Path.GetExtension(path).ToLower();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg": format = FileFormat.jpg; break;
                case ".tif":
                case ".tiff": format = FileFormat.tiff; break;
                case ".png": format = FileFormat.png; break;
                case ".bmp": format = FileFormat.bmp; break;
                case ".wdp": format = FileFormat.wdp; break;

                default:
                    throw new ArgumentException("File format not supported *" + ext);
            }

            using (FileStream str = new FileStream(path, FileMode.Create)) { this.Save(str, format); }
        }

        public void Save(Stream str, FileFormat format)
        {
            BitmapEncoder enc;
            switch(format)
            {
                case FileFormat.jpg: enc = new JpegBitmapEncoder(); ((JpegBitmapEncoder)enc).QualityLevel = 100; break;
                case FileFormat.tiff: enc = new TiffBitmapEncoder(); ((TiffBitmapEncoder)enc).Compression = TiffCompressOption.Lzw; break;
                case FileFormat.png: enc = new PngBitmapEncoder(); break;
                case FileFormat.bmp: enc = new BmpBitmapEncoder(); break;
                case FileFormat.wdp: enc = new WmpBitmapEncoder(); break;

                default:
                    throw new ArgumentException("File format not supported");
            }

            bool SwitchChannels;
            PixelFormat pixFormat = GetPixelFormat(this.BitDepth, out SwitchChannels);

            byte[] imgData;
            int bpc = this.BytePerChannel;

            if (SwitchChannels)
            {
                imgData = new byte[this.ImageData.Length];
                //Make BGR from RGB
                unsafe
                {
                    fixed (byte* pixIn = this.ImageData)
                    fixed (byte* pixOut = imgData)
                    {
                        int j;
                        int bpc2 = 2 * bpc;
                        for (long i = 0; i < this.Height * this.Width * this.ChannelCount; i += bpc * this.ChannelCount)
                        {
                            for (j = 0; j < bpc; j++)
                            {
                                pixOut[i + j] = pixIn[i + j + bpc2];    //set R to B
                                pixOut[i + j + 1] = pixIn[i + j + 1];   //set G to G
                                pixOut[i + j + bpc2] = pixIn[i + j];    //set B to R
                            }
                        }
                    }
                }
            }
            else imgData = ImageData;

            BitmapSource src = BitmapSource.Create((int)this.Width, (int)this.Height, 96, 96, pixFormat, null, imgData, (int)(this.Width * bpc * this.ChannelCount));
            enc.Frames.Add(BitmapFrame.Create(src));
            enc.Save(str);
        }

        #endregion

        #region Scaling
        
        /// <summary>
        /// Scales image to desired width. Preserves aspect ratio.
        /// </summary>
        /// <param name="nWidth">New width of image</param>
        /// <returns>The resized image</returns>
        public BitmapEx ScaleW(uint nWidth)
        {
            double factor = nWidth / (double)Width;
            uint nHeight = (uint)(Height * factor);
            if (nWidth <= 2 || nHeight <= 2) throw new ArgumentException("Output image would be too small with this factor");

            return Scale(nWidth, nHeight);
        }

        /// <summary>
        /// Scales image to desired height. Preserves aspect ratio.
        /// </summary>
        /// <param name="nHeight">New height of image</param>
        /// <returns>The resized image</returns>
        public BitmapEx ScaleH(uint nHeight)
        {
            double factor = nHeight / (double)Height;
            uint nWidth = (uint)(Width * factor);
            if (nWidth <= 2 || nHeight <= 2) throw new ArgumentException("Output image would be too small with this factor");

            return Scale(nWidth, nHeight);
        }

        /// <summary>
        /// Scales image width and height with given factor. 1 = no resize.
        /// </summary>
        /// <param name="factor">The factor to which the image will be scaled</param>
        /// <returns>The resized image</returns>
        public BitmapEx Scale(double factor)
        {
            uint nWidth = (uint)(Width * factor);
            uint nHeight = (uint)(Height * factor);
            if (nWidth <= 2 || nHeight <= 2) throw new ArgumentException("Output image would be too small with this factor");

            return Scale(nWidth, nHeight);
        }

        /// <summary>
        /// Resizes this image to a new size with bi-cubic interpolation
        /// </summary>
        /// <param name="nWidth">New Width</param>
        /// <param name="nHeight">New Height</param>
        /// <returns>The resized image</returns>
        public BitmapEx Scale(uint nWidth, uint nHeight)
        {
            if (nWidth == Width && nHeight == Height) return this.Clone();

            BitmapEx bmpN = new BitmapEx(nWidth, nHeight, this.BitDepth);

            try
            {
                this.LockBits();
                bmpN.LockBits();

                switch (this.BitDepth)
                {
                    case ImageType.RGB8:
                    case ImageType.RGBA8:
                        DoScale8(bmpN, nWidth, nHeight);
                        break;

                    case ImageType.RGB16:
                    case ImageType.RGBA16:
                        DoScale16(bmpN, nWidth, nHeight);
                        break;

                    case ImageType.RGB32:
                    case ImageType.RGBA32:
                        DoScale32(bmpN, nWidth, nHeight);
                        break;

                    case ImageType.RGB64:
                    case ImageType.RGBA64:
                        DoScale64(bmpN, nWidth, nHeight);
                        break;

                    default:
                        throw new ArgumentException("Bitdepth not supported");
                }
            }
            finally
            {
                this.UnlockBits();
                bmpN.UnlockBits();
            }

            return bmpN;
        }

        private unsafe void DoScale8(BitmapEx bmpN, uint nWidth, uint nHeight)
        {
            long dstOffset = 0;
            double xFactor = Width / (double)nWidth;
            double yFactor = Height / (double)nHeight;

            byte* src = (byte*)this.Scan0;
            byte* dst = (byte*)bmpN.Scan0;
            byte* p;

            double ox, oy, dx, dy, k1, k2, r, g, b;
            uint ox1, oy1, ox2, oy2, y, x;
            uint ymax = Height - 1;
            uint xmax = Width - 1;
            int n, m;
            byte cc = this.ChannelCount;

            for (y = 0; y < nHeight; y++)
            {
                oy = y * yFactor - 0.5d;
                oy1 = (uint)oy;
                dy = oy - oy1;

                for (x = 0; x < nWidth; x++, dst += cc)
                {
                    ox = x * xFactor - 0.5d;
                    ox1 = (uint)ox;
                    dx = ox - ox1;
                    r = g = b = 0;

                    for (n = -1; n < 3; n++)
                    {
                        k1 = BiCubicKernel(dy - n);

                        oy2 = (uint)(oy1 + n);
                        if (oy2 < 0) oy2 = 0;
                        if (oy2 > ymax) oy2 = ymax;

                        for (m = -1; m < 3; m++)
                        {
                            k2 = k1 * BiCubicKernel(m - dx);

                            ox2 = (uint)(ox1 + m);
                            if (ox2 < 0) ox2 = 0;
                            if (ox2 > xmax) ox2 = xmax;

                            p = src + oy2 * Stride + ox2 * cc;

                            r += k2 * p[0];
                            g += k2 * p[1];
                            b += k2 * p[2];
                        }
                    }

                    dst[0] = (byte)Math.Max(0, Math.Min(255, r));
                    dst[1] = (byte)Math.Max(0, Math.Min(255, g));
                    dst[2] = (byte)Math.Max(0, Math.Min(255, b));
                }
                dst += dstOffset;
            }
        }

        private unsafe void DoScale16(BitmapEx bmpN, uint nWidth, uint nHeight)
        {
            long dstOffset = bmpN.Stride - 3 * nWidth;
            double xFactor = Width / (double)nWidth;
            double yFactor = Height / (double)nHeight;

            ushort* src = (ushort*)this.Scan0;
            ushort* dst = (ushort*)bmpN.Scan0;
            ushort* p;

            double ox, oy, dx, dy, k1, k2, r, g, b;
            uint ox1, oy1, ox2, oy2, y, x;
            uint ymax = Height - 1;
            uint xmax = Width - 1;
            int n, m;
            byte cc = this.ChannelCount;

            for (y = 0; y < nHeight; y++)
            {
                oy = y * yFactor - 0.5d;
                oy1 = (uint)oy;
                dy = oy - oy1;

                for (x = 0; x < nWidth; x++, dst += cc)
                {
                    ox = x * xFactor - 0.5d;
                    ox1 = (uint)ox;
                    dx = ox - ox1;
                    r = g = b = 0;

                    for (n = -1; n < 3; n++)
                    {
                        k1 = BiCubicKernel(dy - n);

                        oy2 = (uint)(oy1 + n);
                        if (oy2 < 0) oy2 = 0;
                        if (oy2 > ymax) oy2 = ymax;

                        for (m = -1; m < 3; m++)
                        {
                            k2 = k1 * BiCubicKernel(m - dx);

                            ox2 = (uint)(ox1 + m);
                            if (ox2 < 0) ox2 = 0;
                            if (ox2 > xmax) ox2 = xmax;

                            p = src + oy2 * Stride + ox2 * cc;

                            r += k2 * p[0];
                            g += k2 * p[1];
                            b += k2 * p[2];
                        }
                    }

                    dst[0] = (ushort)Math.Max(0, Math.Min(65535, r));
                    dst[1] = (ushort)Math.Max(0, Math.Min(65535, g));
                    dst[2] = (ushort)Math.Max(0, Math.Min(65535, b));
                }
                dst += dstOffset;
            }
        }

        private unsafe void DoScale32(BitmapEx bmpN, uint nWidth, uint nHeight)
        {
            long dstOffset = bmpN.Stride - 3 * nWidth;
            double xFactor = Width / (double)nWidth;
            double yFactor = Height / (double)nHeight;

            uint* src = (uint*)this.Scan0;
            uint* dst = (uint*)bmpN.Scan0;
            uint* p;

            double ox, oy, dx, dy, k1, k2, r, g, b;
            uint ox1, oy1, ox2, oy2, y, x;
            uint ymax = Height - 1;
            uint xmax = Width - 1;
            int n, m;
            byte cc = this.ChannelCount;

            for (y = 0; y < nHeight; y++)
            {
                oy = y * yFactor - 0.5d;
                oy1 = (uint)oy;
                dy = oy - oy1;

                for (x = 0; x < nWidth; x++, dst += cc)
                {
                    ox = x * xFactor - 0.5d;
                    ox1 = (uint)ox;
                    dx = ox - ox1;
                    r = g = b = 0;

                    for (n = -1; n < 3; n++)
                    {
                        k1 = BiCubicKernel(dy - n);

                        oy2 = (uint)(oy1 + n);
                        if (oy2 < 0) oy2 = 0;
                        if (oy2 > ymax) oy2 = ymax;

                        for (m = -1; m < 3; m++)
                        {
                            k2 = k1 * BiCubicKernel(m - dx);

                            ox2 = (uint)(ox1 + m);
                            if (ox2 < 0) ox2 = 0;
                            if (ox2 > xmax) ox2 = xmax;

                            p = src + oy2 * Stride + ox2 * cc;

                            r += k2 * p[0];
                            g += k2 * p[1];
                            b += k2 * p[2];
                        }
                    }

                    dst[0] = (uint)Math.Max(0, Math.Min(4294967295, r));
                    dst[1] = (uint)Math.Max(0, Math.Min(4294967295, g));
                    dst[2] = (uint)Math.Max(0, Math.Min(4294967295, b));
                }
                dst += dstOffset;
            }
        }

        private unsafe void DoScale64(BitmapEx bmpN, uint nWidth, uint nHeight)
        {
            long dstOffset = bmpN.Stride - 3 * nWidth;
            decimal xFactor = Width / (decimal)nWidth;
            decimal yFactor = Height / (decimal)nHeight;

            ulong* src = (ulong*)this.Scan0;
            ulong* dst = (ulong*)bmpN.Scan0;
            ulong* p;

            decimal ox, oy, dx, dy, k1, k2, r, g, b;
            uint ox1, oy1, ox2, oy2, y, x;
            uint ymax = Height - 1;
            uint xmax = Width - 1;
            int n, m;
            byte cc = this.ChannelCount;

            for (y = 0; y < nHeight; y++)
            {
                oy = y * yFactor - 0.5m;
                oy1 = (uint)oy;
                dy = oy - oy1;

                for (x = 0; x < nWidth; x++, dst += cc)
                {
                    ox = x * xFactor - 0.5m;
                    ox1 = (uint)ox;
                    dx = ox - ox1;
                    r = g = b = 0;

                    for (n = -1; n < 3; n++)
                    {
                        k1 = BiCubicKernelDecimal(dy - n);

                        oy2 = (uint)(oy1 + n);
                        if (oy2 < 0) oy2 = 0;
                        if (oy2 > ymax) oy2 = ymax;

                        for (m = -1; m < 3; m++)
                        {
                            k2 = k1 * BiCubicKernelDecimal(m - dx);

                            ox2 = (uint)(ox1 + m);
                            if (ox2 < 0) ox2 = 0;
                            if (ox2 > xmax) ox2 = xmax;

                            p = src + oy2 * Stride + ox2 * cc;

                            r += k2 * p[0];
                            g += k2 * p[1];
                            b += k2 * p[2];
                        }
                    }

                    dst[0] = (ulong)Math.Max(0, Math.Min(18446744073709551615, r));
                    dst[1] = (ulong)Math.Max(0, Math.Min(18446744073709551615, g));
                    dst[2] = (ulong)Math.Max(0, Math.Min(18446744073709551615, b));
                }
                dst += dstOffset;
            }
        }

        private static decimal BiCubicKernelDecimal(decimal x)
        {
            if (x < 0) x = -x;
            if (x <= 1) return (1.5m * x - 2.5m) * x * x + 1;
            else if (x < 2) return ((-0.5m * x + 2.5m) * x - 4) * x + 2;
            else return 0m;
        }

        private static double BiCubicKernel(double x)
        {
            if (x < 0) x = -x;
            if (x <= 1) return (1.5 * x - 2.5) * x * x + 1;
            else if (x < 2) return ((-0.5 * x + 2.5) * x - 4) * x + 2;
            else return 0d;
        }

        #endregion
    }
}

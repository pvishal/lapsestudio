using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Timelapse_API
{
    [Serializable]
	public class BitmapEx : IDisposable
	{
        /* Example usage (for 8bit RGB)

            bmp.LockBits();
            unsafe
            {
                int x, y, index;
                byte* row;
                for (y = 0; y < bmp.Height; y++)
                {
                    row = (byte*)bmp.Scan0 + y * bmp.Stride;
                    for (x = 0; x < bmp.Width; x++)
                    {
                        index = x * 3;
                        row[index] = 125;
                        row[index + 1] = 125;
                        row[index + 2] = 125;
                    }
                }
            }
            bmp.UnlockBits();
         */
        
        public uint Width { get; protected set; }
		public uint Height { get; protected set; }
		public ImageType BitDepth { get; protected set; }
		public byte ChannelCount { get; private set; }

        [NonSerialized]
        private GCHandle pinHandle;
        private bool IsDisposed;
        private byte[] ImageData;

        public bool IsPinned { get; private set; }
        public IntPtr Scan0 { get; private set; }
        public uint Stride { get; private set; }
		public uint Length { get { return Height * Stride; } }

		#region Constructor/Init

        public BitmapEx(string path)
        {
            BitmapToBitmapByte(new Bitmap(path));
        }

        public BitmapEx(Bitmap bmp)
        {
            BitmapToBitmapByte(bmp);
        }

		public BitmapEx(byte[] RawData, uint Width, uint Height, ImageType BitDepth)
		{
			this.ImageData = RawData;
			this.Width = Width;
			this.Height = Height;
			this.BitDepth = BitDepth;
			SetStride();
		}

		private void SetStride()
		{
			switch (BitDepth)
			{
				case ImageType.RGB8:
				case ImageType.RGB16:
				case ImageType.RGB32:
				case ImageType.RGB64:
					Stride = Width * 3;
					ChannelCount = 3;
					break;
				case ImageType.RGBA8:
				case ImageType.RGBA16:
				case ImageType.RGBA32:
				case ImageType.RGBA64:
					Stride = Width * 4;
					ChannelCount = 4;
					break;
			}
		}

        private void BitmapToBitmapByte(Bitmap bmp)
        {
            BitmapData bmd = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);

            uint index, depth, index2 = 0;
            int y, x;
            bool HasAlpha;
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb) { depth = 3; HasAlpha = false; }
            else if (bmp.PixelFormat == PixelFormat.Format32bppArgb || bmp.PixelFormat == PixelFormat.Format32bppRgb || bmp.PixelFormat == PixelFormat.Format32bppPArgb) { depth = 4; HasAlpha = true; }
            else if (bmp.PixelFormat == PixelFormat.Format48bppRgb) { depth = 6; HasAlpha = false; }
            else if (bmp.PixelFormat == PixelFormat.Format64bppArgb || bmp.PixelFormat == PixelFormat.Format64bppPArgb) { depth = 8; HasAlpha = true; }
            else throw new ArgumentException("PixelFormat of image is not supported");

            if (depth == 3 || depth == 6) this.ImageData = new byte[bmp.Height * bmp.Width * 3];
            else this.ImageData = new byte[bmp.Height * bmp.Width * 4];

            unsafe
            {
                byte* row;
                for (y = 0; y < bmp.Height; y++)
                {
                    row = (byte*)bmd.Scan0 + y * bmd.Stride;
                    for (x = 0; x < bmp.Width; x++)
                    {
                        index = (uint)(x * depth);
                        this.ImageData[index2++] = row[index + 2];                  //R
                        this.ImageData[index2++] = row[index + 1];                  //G
                        this.ImageData[index2++] = row[index];                      //B
                        if (HasAlpha) this.ImageData[index2++] = row[index + 3];    //Alpha
                    }
                }
            }
            bmp.UnlockBits(bmd);

            this.Width = (uint)bmp.Width;
            this.Height = (uint)bmp.Height;
            if (HasAlpha) this.BitDepth = ImageType.RGBA8;
            else this.BitDepth = ImageType.RGB8;
            SetStride();
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
    }
}


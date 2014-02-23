using System;
using System.Drawing;
using System.Drawing.Imaging;
using ColorManagment;
using Gdk;
using System.IO;

namespace Timelapse_API
{
    public class UniversalImage
    {
        private Bitmap bmp;
        private Pixbuf pxf;
        private MemoryStream str;

        public int Width
        {
            get
            {
                if (pxf != null) return pxf.Width;
                else if (bmp != null) return bmp.Width;
                else return 0;
            }
        }
        public int Height
        {
            get
            {
                if (pxf != null) return pxf.Height;
                else if (bmp != null) return bmp.Height;
                else return 0;
            }
        }
        
        public UniversalImage(Bitmap bmp)
        {
            this.bmp = bmp;
        }

        public UniversalImage(Pixbuf pxf)
        {
            this.pxf = pxf;
        }

        public Pixbuf Pixbuf
        {
            get
            {
                if (pxf != null) return pxf;
                else if (bmp != null)
                {
                    bmp.Save(str, ImageFormat.Png);
                    return new Pixbuf(str);
                }
                else return null;
            }
            set
            {
                if (bmp != null) bmp = null;
                this.pxf = value;
            }
        }

        public Bitmap Bitmap
        {
            get
            {
                if (bmp != null) return bmp;
                else if (pxf != null)
                {
                    str = new MemoryStream(pxf.SaveToBuffer("png"));
                    return new Bitmap(str);
                }
                else return null;
            }
            set
            {
                if (pxf != null) pxf = null;
                this.bmp = value;
            }
        }

        /*protected byte[] Data;

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public ImageType Type { get; protected set; }
        public RGBSpaceName ColorSpace { get; protected set; } 

        public UniversalImage(int Width, int Height, ImageType Type)
        {
            Data = new byte[Width * Height];
            this.Width = Width;
            this.Height = Height;
            this.Type = Type;
            ColorSpace = RGBSpaceName.sRGB;
        }

        public static UniversalImage FromPath(string FilePath)
        {
            return FromBitmap(new Bitmap(FilePath));
        }

        public static UniversalImage FromBitmap(Bitmap bmp)
        {
            UniversalImage img = new UniversalImage(bmp.Width, bmp.Height, GetImageType(bmp.PixelFormat));

            BitmapData bmd = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
            int pixelsize, x, y, index = 0;
            bool HasAlpha = false;
            byte[] tmp;

            if (bmp.PixelFormat == PixelFormat.Format24bppRgb || bmp.PixelFormat == PixelFormat.Format48bppRgb) { pixelsize = 3; HasAlpha = false; }
            else if (bmp.PixelFormat == PixelFormat.Format32bppArgb || bmp.PixelFormat == PixelFormat.Format64bppArgb) { pixelsize = 4; HasAlpha = true; }
            else { throw new Exception("Pixelformat not supported"); }

            if (bmp.PixelFormat == PixelFormat.Format24bppRgb || bmp.PixelFormat == PixelFormat.Format32bppArgb)
            {
                unsafe
                {
                    byte* row;
                    for (y = 0; y < bmp.Height; y += 2)
                    {
                        row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        for (x = 0; x < bmp.Width; x += 2)
                        {
                            index = x * pixelsize;
                            img.Data[index] = row[index + 2];
                            img.Data[index + 1] = row[index + 1];
                            img.Data[index + 2] = row[index];
                            if (HasAlpha) img.Data[index + 3] = row[index + 3];
                        }
                    }
                }
            }
            else if (bmp.PixelFormat == PixelFormat.Format48bppRgb || bmp.PixelFormat == PixelFormat.Format64bppArgb)
            {
                unsafe
                {
                    ushort* row;
                    for (y = 0; y < bmp.Height; y += 2)
                    {
                        row = (ushort*)bmd.Scan0 + (y * (bmd.Stride / 2));
                        for (x = 0; x < bmp.Width; x += 2)
                        {
                            index = x * pixelsize;
                            tmp = BitConverter.GetBytes((ushort)(65535 * row[index + 2] / 16383d));
                            img.Data[index] = tmp[0]; img.Data[index + 1] = tmp[1];
                            tmp = BitConverter.GetBytes((ushort)(65535 * row[index + 1] / 16383d));
                            img.Data[index + 2] = tmp[2]; img.Data[index + 3] = tmp[3];
                            tmp = BitConverter.GetBytes((ushort)(65535 * row[index] / 16383d));
                            img.Data[index + 4] = tmp[4]; img.Data[index + 5] = tmp[5];

                            if (HasAlpha)
                            {
                                tmp = BitConverter.GetBytes((ushort)(65535 * row[index + 3] / 16383d));
                                img.Data[index + 6] = tmp[6]; img.Data[index + 7] = tmp[7];
                            }
                        }
                    }
                }
            }
            bmp.UnlockBits(bmd);

            return img;
        }

        public Bitmap GetBitmap()
        {
            Bitmap output = new Bitmap(Width, Height, GetPixelFormat(Type));
            BitmapData bmd = output.LockBits(new System.Drawing.Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, output.PixelFormat);
            int pixelsize, x, y, index = 0;
            bool HasAlpha = false;

            if (output.PixelFormat == PixelFormat.Format24bppRgb || output.PixelFormat == PixelFormat.Format48bppRgb) { pixelsize = 3; HasAlpha = false; }
            else if (output.PixelFormat == PixelFormat.Format32bppArgb || output.PixelFormat == PixelFormat.Format64bppArgb) { pixelsize = 4; HasAlpha = true;}
            else { throw new Exception("Pixelformat not supported"); }

            if (output.PixelFormat == PixelFormat.Format24bppRgb || output.PixelFormat == PixelFormat.Format32bppArgb)
            {
                unsafe
                {
                    byte* row;
                    for (y = 0; y < output.Height; y += 2)
                    {
                        row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        for (x = 0; x < output.Width; x += 2)
                        {
                            index = x * pixelsize;
                            row[index + 2] = Data[index];
                            row[index + 1] = Data[index + 1];
                            row[index] = Data[index + 2];
                            if (HasAlpha) row[index + 3] = Data[index + 3];
                        }
                    }
                }
            }
            else if (output.PixelFormat == PixelFormat.Format48bppRgb || output.PixelFormat == PixelFormat.Format64bppArgb)
            {
                unsafe
                {
                    ushort* row;
                    for (y = 0; y < output.Height; y += 2)
                    {
                        row = (ushort*)bmd.Scan0 + (y * (bmd.Stride / 2));
                        for (x = 0; x < output.Width; x += 2)
                        {
                            index = x * pixelsize;
                            if (Type == ImageType.RGB16 || Type == ImageType.RGBA16)
                            {
                                row[index + 2] = BitConverter.ToUInt16(Data, index);
                                row[index + 1] = BitConverter.ToUInt16(Data, index + 2);
                                row[index] = BitConverter.ToUInt16(Data, index + 4);
                                if (HasAlpha) row[index + 3] = BitConverter.ToUInt16(Data, index + 6);
                            }
                            else if (Type == ImageType.RGB32 || Type == ImageType.RGBA32)
                            {
                                row[index + 2] = (ushort)(16383 * BitConverter.ToUInt32(Data, index) / 4294967295d);
                                row[index + 1] = (ushort)(16383 * BitConverter.ToUInt32(Data, index + 4) / 4294967295d);
                                row[index] = (ushort)(16383 * BitConverter.ToUInt32(Data, index + 8) / 4294967295d);
                                if (HasAlpha) row[index + 3] = (ushort)(16383 * BitConverter.ToUInt32(Data, index + 12) / 4294967295d);
                            }
                        }
                    }
                }
            }
            output.UnlockBits(bmd);

            return output;
        }

        private static PixelFormat GetPixelFormat(ImageType Type)
        {
            switch (Type)
            {
                case ImageType.RGB8:
                    return PixelFormat.Format24bppRgb;
                case ImageType.RGBA8:
                    return PixelFormat.Format32bppArgb;
                case ImageType.RGB16:
                case ImageType.RGB32:
                    return PixelFormat.Format64bppArgb;
                case ImageType.RGBA16:
                case ImageType.RGBA32:
                    return PixelFormat.Format48bppRgb;

                default:
                    return PixelFormat.Format24bppRgb;
            }
        }

        private static ImageType GetImageType(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Format24bppRgb:
                    return ImageType.RGB8;
                case PixelFormat.Format32bppArgb:
                    return ImageType.RGBA8;
                case PixelFormat.Format48bppRgb:
                    return ImageType.RGB16;
                case PixelFormat.Format64bppArgb:
                    return ImageType.RGBA16;

                default:
                    throw new NotSupportedException();
            }
        }*/
    }
}

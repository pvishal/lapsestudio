using System;
using System.Drawing;
using System.Drawing.Imaging;
using ColorManagment;
using Gdk;
using System.IO;

namespace Timelapse_API
{
    //TODO: replace with MagickImage

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
    }
}

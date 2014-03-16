using System;
using Timelapse_UI;
using Timelapse_API;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace LapseStudioWinFormsUI
{
	public static class WinFormHelper
	{
        public static MessageBoxIcon GetWinType(MessageWindowType typ)
        {
            switch (typ)
            {
                case MessageWindowType.Error:
                    return MessageBoxIcon.Error;

                case MessageWindowType.Question:
                    return MessageBoxIcon.Question;

                case MessageWindowType.Warning:
                    return MessageBoxIcon.Warning;

                case MessageWindowType.Info:
                    return MessageBoxIcon.Information;
                    
                case MessageWindowType.Other:
                default:
                    return MessageBoxIcon.None;
            }
        }

		public static WindowResponse GetResponse(DialogResult resp)
		{
			switch(resp)
			{
				case DialogResult.Abort:
					return WindowResponse.Cancel;

				case DialogResult.Ignore:
					return WindowResponse.Ignore;

                case DialogResult.Cancel:
                    return WindowResponse.Cancel;

                case DialogResult.No:
                    return WindowResponse.No;

                case DialogResult.OK:
                    return WindowResponse.Ok;

                case DialogResult.Retry:
                    return WindowResponse.Retry;

                case DialogResult.Yes:
                    return WindowResponse.Yes;

                case DialogResult.None:
				default:
					return WindowResponse.None;
			}
		}

        public static MessageBoxButtons GetButtons(MessageWindowButtons buttons)
		{
			switch(buttons)
			{
				case MessageWindowButtons.Cancel:
                    return MessageBoxButtons.OKCancel;

				case MessageWindowButtons.OkCancel:
                    return MessageBoxButtons.OKCancel;

				case MessageWindowButtons.YesNo:
                    return MessageBoxButtons.YesNo;

                case MessageWindowButtons.YesNoCancel:
                    return MessageBoxButtons.YesNoCancel;

                case MessageWindowButtons.RetryCancel:
                    return MessageBoxButtons.RetryCancel;

                case MessageWindowButtons.AbortRetryIgnore:
                    return MessageBoxButtons.AbortRetryIgnore;

                case MessageWindowButtons.Close:
                case MessageWindowButtons.None:
				case MessageWindowButtons.Ok:
				default:
                    return MessageBoxButtons.OK;
			}
		}

        public static Bitmap ConvertToBitmap(BitmapEx bmpEx)
        {
            uint index, depth = 0;
            int y, x;
            bool HasAlpha;
            PixelFormat format;
            if (bmpEx.BitDepth == ImageType.RGB8) { format = PixelFormat.Format24bppRgb; depth = 3; HasAlpha = false; }
            else if (bmpEx.BitDepth == ImageType.RGBA8) { format = PixelFormat.Format32bppArgb; depth = 4; HasAlpha = true; }
            else if (bmpEx.BitDepth == ImageType.RGB16) { format = PixelFormat.Format48bppRgb; depth = 6; HasAlpha = false; }
            else if (bmpEx.BitDepth == ImageType.RGBA16) { format = PixelFormat.Format64bppArgb; depth = 8; HasAlpha = true; }
            else throw new ArgumentException("Bitdepth not supported");

            Bitmap outBmp = new Bitmap((int)bmpEx.Width, (int)bmpEx.Height, format);
            BitmapData bmd = outBmp.LockBits(new System.Drawing.Rectangle(0, 0, outBmp.Width, outBmp.Height), ImageLockMode.WriteOnly, outBmp.PixelFormat);
            bmpEx.LockBits();          
  
            unsafe
            {
                byte* rowIn;
                byte* rowOut;
                for (y = 0; y < outBmp.Height; y++)
                {
                    rowOut = (byte*)bmd.Scan0 + y * bmd.Stride;
                    rowIn = (byte*)bmpEx.Scan0 + y * bmpEx.Stride;
                    for (x = 0; x < outBmp.Width; x++)
                    {
                        index = (uint)(x * depth);
                        rowOut[index + 2] = rowIn[index];
                        rowOut[index + 1] = rowIn[index + 1];
                        rowOut[index] = rowIn[index + 2];
                        if (HasAlpha) rowOut[index + 3] = rowIn[index + 3];
                    }
                }
            }
            outBmp.UnlockBits(bmd);
            bmpEx.UnlockBits();

            return outBmp;
        }
	}
}


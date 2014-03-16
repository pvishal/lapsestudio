using System;
using Timelapse_UI;
using Timelapse_API;
using Gtk;
using Gdk;
using ImageType = Timelapse_API.ImageType;

namespace LapseStudioGtkUI
{
	public static class GtkHelper
	{
		public static MessageType GetWinType(MessageWindowType typ)
		{
			switch(typ)
			{
				case MessageWindowType.Error:
					return MessageType.Error;

				case MessageWindowType.Other:
					return MessageType.Other;

				case MessageWindowType.Question:
					return MessageType.Question;

				case MessageWindowType.Warning:
					return MessageType.Warning;

				case MessageWindowType.Info:
				default:
					return MessageType.Info;
			}
		}

		public static WindowResponse GetResponse(ResponseType resp)
		{
			switch(resp)
			{
				case ResponseType.Accept:
					return WindowResponse.Accept;

				case ResponseType.Apply:
					return WindowResponse.Apply;

				case ResponseType.Cancel:
					return WindowResponse.Cancel;

				case ResponseType.Close:
					return WindowResponse.Close;

				case ResponseType.DeleteEvent:
					return WindowResponse.DeleteEvent;

				case ResponseType.Help:
					return WindowResponse.Help;

				case ResponseType.No:
					return WindowResponse.No;

				case ResponseType.Ok:
					return WindowResponse.Ok;

				case ResponseType.Reject:
					return WindowResponse.Ignore;

				case ResponseType.Yes:
					return WindowResponse.Yes;

				case ResponseType.None:
				default:
					return WindowResponse.None;
			}
		}

        public static Pixbuf ConvertToPixbuf(BitmapEx bmpEx)
        {
            int bitspersample;
            bool HasAlpha;

            if (bmpEx.BitDepth == ImageType.RGB8) { bitspersample = 8; HasAlpha = false; }
            else if (bmpEx.BitDepth == ImageType.RGBA8) { bitspersample = 8; HasAlpha = true; }
            else if (bmpEx.BitDepth == ImageType.RGB16) { bitspersample = 16; HasAlpha = false; }
            else if (bmpEx.BitDepth == ImageType.RGBA16) { bitspersample = 16; HasAlpha = true; }
            else throw new ArgumentException("Bitdepth not supported");

            byte[] data = new byte[bmpEx.Height * bmpEx.Stride];
            bmpEx.LockBits();
            unsafe { System.Runtime.InteropServices.Marshal.Copy(bmpEx.Scan0, data, 0, data.Length); }
            bmpEx.UnlockBits();

            return new Pixbuf(data, HasAlpha, bitspersample, (int)bmpEx.Width, (int)bmpEx.Height, (int)bmpEx.Stride);
        }
	}
}


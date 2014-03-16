using System;
using System.Drawing;
using Timelapse_UI;
using Timelapse_API;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;

namespace LapseStudioMacUI
{
	public static class CocoaHelper
	{
		public static NSAlertStyle GetWinType(MessageWindowType typ)
		{
			switch(typ)
			{
				case MessageWindowType.Error:
					return NSAlertStyle.Critical;

				case MessageWindowType.Warning:
					return NSAlertStyle.Warning;

				case MessageWindowType.Info:
				default:
					return NSAlertStyle.Informational;
			}
		}

		public static WindowResponse GetResponse(int resp, MessageWindowButtons bType)
		{
			switch (bType)
			{
				case MessageWindowButtons.AbortRetryIgnore:
					if(resp == 1000) return WindowResponse.Cancel;
					else if(resp == 1001) return WindowResponse.Retry;
					else if(resp == 1002) return WindowResponse.Ignore;
					break;
				case MessageWindowButtons.Cancel:
					if(resp == 1000) return WindowResponse.Cancel;
					break;
				case MessageWindowButtons.Close:
					if(resp == 1000) return WindowResponse.Close;
					break;
				case  MessageWindowButtons.Ok:
					if(resp == 1000) return WindowResponse.Ok;
					break;
				case MessageWindowButtons.OkCancel:
					if(resp == 1000) return WindowResponse.Ok;
					else if(resp == 1001) return WindowResponse.Cancel;
					break;
				case MessageWindowButtons.RetryCancel:
					if(resp == 1000) return WindowResponse.Retry;
					else if(resp == 1001) return WindowResponse.Cancel;
					break;
				case MessageWindowButtons.YesNo:
					if(resp == 1000) return WindowResponse.Yes;
					else if(resp == 1001) return WindowResponse.No;
					break;
				case MessageWindowButtons.YesNoCancel:
					if(resp == 1000) return WindowResponse.Yes;
					else if(resp == 1001) return WindowResponse.No;
					else if(resp == 1002) return WindowResponse.Cancel;
					break;

				case MessageWindowButtons.None:
				default:
					return WindowResponse.None;
			}
			return WindowResponse.None;
		}

		public static WindowResponse GetResponse(int resp)
		{
			switch (resp)
			{
				case 0:	return WindowResponse.Cancel;
				case 1: return WindowResponse.Ok;
				default: return WindowResponse.None;
			}
		}

		public static NSImage ToNSImage(BitmapEx bmpEx)
		{
			bmpEx.LockBits();
			long bufferLength = bmpEx.Width * bmpEx.Height;
			CGDataProvider provider = new CGDataProvider(bmpEx.Scan0, (int)bufferLength);
			int bitsPerComponent, bitsPerPixel, bytesPerRow = (int)bmpEx.Stride;
			CGColorSpace colorSpaceRef = CGColorSpace.CreateDeviceRGB();

			switch (bmpEx.BitDepth)
			{
				case ImageType.RGB16:
					bitsPerComponent = 16;
					bitsPerPixel = 48;
					bufferLength *= 3;
					break;
				case ImageType.RGBA16:
					bitsPerComponent = 16;
					bitsPerPixel = 64;
					bufferLength *= 4;
					break;
				case ImageType.RGB8:
					bitsPerComponent = 8;
					bitsPerPixel = 24;
					bufferLength *= 3;
					break;
				case ImageType.RGBA8:
					bitsPerComponent = 8;
					bitsPerPixel = 32;
					bufferLength *= 4;
					break;
				case ImageType.RGB32:
					bitsPerComponent = 32;
					bitsPerPixel = 96;
					bufferLength *= 3;
					break;
				case ImageType.RGBA32:
					bitsPerComponent = 32;
					bitsPerPixel = 128;
					bufferLength *= 4;
					break;
				case ImageType.RGB64:
					bitsPerComponent = 64;
					bitsPerPixel = 192;
					bufferLength *= 3;
					break;
				case ImageType.RGBA64:
					bitsPerComponent = 64;
					bitsPerPixel = 256;
					bufferLength *= 4;
					break;

				default:
					throw new ArgumentException("Bitdepth not supported");
			}

			CGImage img = new CGImage((int)bmpEx.Width, (int)bmpEx.Height, bitsPerComponent, bitsPerPixel, bytesPerRow, colorSpaceRef, CGBitmapFlags.ByteOrderDefault, provider, null, true, CGColorRenderingIntent.Default);

			bmpEx.UnlockBits();

			return new NSImage(img, new SizeF(img.Width, img.Height));
		}
	}
}


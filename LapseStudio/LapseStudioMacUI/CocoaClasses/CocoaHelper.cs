using Timelapse_UI;
using MonoMac.AppKit;

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
	}
}


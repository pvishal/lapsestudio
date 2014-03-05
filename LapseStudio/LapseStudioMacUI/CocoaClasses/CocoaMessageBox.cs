using Timelapse_UI;
using MonoMac.AppKit;
using MessageTranslation;

namespace LapseStudioMacUI
{
	public class CocoaMessageBox : MessageBox
	{
		public override WindowResponse Show(object parent, string message, string title, MessageWindowType type, MessageWindowButtons bType)
		{
			NSAlert al = new NSAlert();
			al.AlertStyle = CocoaHelper.GetWinType(type);
			al.MessageText = title;
			al.InformativeText = message;

			switch (bType)
			{
				case MessageWindowButtons.AbortRetryIgnore:
					al.AddButton(Message.GetString("Abort"));
					al.AddButton(Message.GetString("Retry"));
					al.AddButton(Message.GetString("Ignore"));
					break;
				case MessageWindowButtons.Cancel:
					al.AddButton(Message.GetString("Cancel"));
					break;
				case MessageWindowButtons.Close:
					al.AddButton(Message.GetString("Close"));
					break;
				case  MessageWindowButtons.Ok:
					al.AddButton(Message.GetString("Ok"));
					break;
				case MessageWindowButtons.OkCancel:
					al.AddButton(Message.GetString("Ok"));
					al.AddButton(Message.GetString("Cancel"));
					break;
				case MessageWindowButtons.RetryCancel:
					al.AddButton(Message.GetString("Retry"));
					al.AddButton(Message.GetString("Cancel"));
					break;
				case MessageWindowButtons.YesNo:
					al.AddButton(Message.GetString("Yes"));
					al.AddButton(Message.GetString("No"));
					break;
				case MessageWindowButtons.YesNoCancel:
					al.AddButton(Message.GetString("Yes"));
					al.AddButton(Message.GetString("No"));
					al.AddButton(Message.GetString("Cancel"));
					break;
			}

			WindowResponse resp = CocoaHelper.GetResponse(al.RunModal(), bType);
			al.Dispose();
			return resp;
		}
	}
}


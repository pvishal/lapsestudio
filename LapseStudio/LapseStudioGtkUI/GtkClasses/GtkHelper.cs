using Timelapse_UI;
using Gtk;

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
	}
}


using Gtk;
using MessageTranslation;
using Timelapse_UI;

namespace LapseStudioGtkUI
{
    public class GtkMessageBox : MessageBox
    {
		public override WindowResponse Show(object parent, string message, string title, MessageWindowType type, MessageWindowButtons bType)
		{
			Window p = (Window)parent;
			MessageType t = GtkHelper.GetWinType(type);

            MessageDialog md = new MessageDialog(p, DialogFlags.Modal, t, ButtonsType.None, message);
			md.Title = title;
			if (p != null && p.Icon != null) { md.Icon = p.Icon; }
			md.WindowPosition = WindowPosition.CenterOnParent;

			if (bType == MessageWindowButtons.Ok || bType == MessageWindowButtons.OkCancel) md.AddButton(Message.GetString("Ok"), ResponseType.Ok);
			if (bType == MessageWindowButtons.Close) md.AddButton(Message.GetString("Close"), ResponseType.Close);
			if (bType == MessageWindowButtons.YesNo || bType == MessageWindowButtons.YesNoCancel) { md.AddButton(Message.GetString("Yes"), ResponseType.Yes); md.AddButton(Message.GetString("No"), ResponseType.No); }
			if (bType == MessageWindowButtons.AbortRetryIgnore || bType == MessageWindowButtons.RetryCancel) md.AddButton(Message.GetString("Retry"), ResponseType.Accept);
			if (bType == MessageWindowButtons.AbortRetryIgnore) md.AddButton(Message.GetString("Ignore"), ResponseType.Reject);
			if (bType == MessageWindowButtons.Cancel || bType == MessageWindowButtons.OkCancel || bType == MessageWindowButtons.RetryCancel || bType == MessageWindowButtons.YesNoCancel || bType == MessageWindowButtons.AbortRetryIgnore) md.AddButton(Message.GetString("Cancel"), ResponseType.Cancel);

            ResponseType result = (ResponseType)md.Run();
            md.Destroy();
			return GtkHelper.GetResponse(result);
		}
    }
}
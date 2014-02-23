using System.Windows.Forms;
using Timelapse_UI;
using MessageBox = System.Windows.Forms.MessageBox;

namespace LapseStudioWinFormsUI
{
    public class WinFormMessageBox : Timelapse_UI.MessageBox
    {
		public override WindowResponse Show(object parent, string message, string title, MessageWindowType type, MessageWindowButtons bType)
        {
            if (parent != null) return WinFormHelper.GetResponse(MessageBox.Show(message, title, WinFormHelper.GetButtons(bType), WinFormHelper.GetWinType(type)));
            else return WinFormHelper.GetResponse(MessageBox.Show((IWin32Window)parent, message, title, WinFormHelper.GetButtons(bType), WinFormHelper.GetWinType(type)));
		}
    }
}
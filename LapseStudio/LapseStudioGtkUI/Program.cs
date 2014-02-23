using System;
using Gtk;

namespace LapseStudioGUI
{
	class MainClass
	{
		public static void Main(string[] args)
		{
            try
            {
                Application.Init();
                MainWindow win = new MainWindow();
                win.Show();
                Application.Run();
            }
            catch (Exception e)
            {
                string message = "An error happened:" + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace;
                MessageDialog dlg = new MessageDialog(null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, message);
                dlg.Title = "Error";
                dlg.WindowPosition = WindowPosition.CenterOnParent;
                dlg.Run();
                dlg.Destroy();
            }
		}
	}
}

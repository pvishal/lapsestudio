using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using Timelapse_UI;

namespace LapseStudioMacUI
{
	public partial class AppDelegate : NSApplicationDelegate
	{
		MainWindowController mainWindowController;
		SettingsWindowController SettingsDialog;
		AboutWindowController AboutDialog;
		HelpWindowController HelpDialog;

		public AppDelegate()
		{
		}

		public override void FinishedLaunching(NSObject notification)
		{
			mainWindowController = new MainWindowController();
			mainWindowController.Window.MakeKeyAndOrderFront(this);
		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
		{
			return false;
		}

		partial void MenuAboutItem_Click(NSObject sender)
		{
			try
			{
				AboutDialog = new AboutWindowController();
				AboutDialog.LoadWindow();
				AboutDialog.Window.WillClose += AboutDialog_WillClose;
				NSApplication.SharedApplication.RunModalForWindow(AboutDialog.Window);
			}
			catch(Exception ex) { Error.Report("MenuAboutItem_Click", ex); }
		}

		private void AboutDialog_WillClose (object sender, EventArgs e)
		{
			try
			{
				NSApplication.SharedApplication.StopModal();
				AboutDialog.Dispose();
			}
			catch(Exception ex) { Error.Report("AboutDialog_WillClose", ex); }
		}

		partial void MenuQuitItem_Click(NSObject sender)
		{
			try { mainWindowController.MainUI.Quit(ClosingReason.User); }
			catch(Exception ex) { Error.Report("MenuQuitItem_Click", ex); }
		}

		partial void MenuPreferencesItem_Click(NSObject sender)
		{
			try
			{
				SettingsDialog = new SettingsWindowController();
				SettingsDialog.LoadWindow();
				SettingsDialog.Window.WillClose += SettingsDialog_WillClose;
				NSApplication.SharedApplication.RunModalForWindow(SettingsDialog.Window);
			}
			catch(Exception ex) { Error.Report("MenuPreferencesItem_Click", ex); }
		}

		private void SettingsDialog_WillClose(object sender, EventArgs e)
		{
			try
			{
				NSApplication.SharedApplication.StopModal();
				if(SettingsDialog.Result == WindowResponse.Ok) mainWindowController.MainUI.SettingsChanged();
				SettingsDialog.Dispose();
			}
			catch(Exception ex) { Error.Report("SettingsDialog_WillClose", ex); }
		}

		partial void MenuCloseItem_Click(NSObject sender)
		{
			try { mainWindowController.MainUI.Quit(ClosingReason.User); }
			catch(Exception ex) { Error.Report("MenuCloseItem_Click", ex); }
		}

		partial void MenuHelpItem_Click(NSObject sender)
		{
			try
			{
				HelpDialog = new HelpWindowController();
				HelpDialog.ShowWindow(this);
			}
			catch(Exception ex) { Error.Report("MenuHelpItem_Click", ex); }
		}

		partial void MenuNewItem_Click(NSObject sender)
		{
			try { mainWindowController.MainUI.Click_NewProject(); }
			catch(Exception ex) { Error.Report("MenuNewItem_Click", ex); }
		}

		partial void MenuOpenItem_Click(NSObject sender)
		{
			try { mainWindowController.MainUI.Click_OpenProject(); }
			catch(Exception ex) { Error.Report("MenuOpenItem_Click", ex); }
		}

		partial void MenuSaveAsItem_Click(NSObject sender)
		{
			try { mainWindowController.MainUI.Click_SaveProject(true); }
			catch(Exception ex) { Error.Report("MenuSaveAsItem_Click", ex); }
		}

		partial void MenuSaveItem_Click(NSObject sender)
		{
			try { mainWindowController.MainUI.Click_SaveProject(false); }
			catch(Exception ex) { Error.Report("MenuSaveItem_Click", ex); }
		}
	}
}

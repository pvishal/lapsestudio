using System;
using System.Collections.Generic;
using MonoMac.AppKit;
using MonoMac.Foundation;
using Timelapse_UI;

namespace LapseStudioMacUI
{
	public class CocoaFileDialog : FileDialog
	{
		NSOpenPanel ofdlg;
		NSSavePanel sfdlg;

		protected override FileDialog GetDialog()
		{
			return this;
		}

		public override WindowResponse Show()
		{
			WindowResponse resp;
			List<string> filetypes = new List<string>();

			switch(DialogType)
			{
				case FileDialogType.OpenFile:
					ofdlg = new NSOpenPanel();
					ofdlg.DirectoryUrl = new NSUrl(InitialDirectory);
					ofdlg.Title = Title;
					ofdlg.CanChooseFiles = true;
					ofdlg.CanChooseDirectories = false;
					ofdlg.AllowsMultipleSelection = false;
					foreach(FileTypeFilter arr in FileTypeFilters) filetypes.AddRange(arr.Filter);
					ofdlg.AllowedFileTypes = filetypes.ToArray();

					resp = CocoaHelper.GetResponse(ofdlg.RunModal());
					SelectedPath = ofdlg.Url.Path;
					return resp;

				case FileDialogType.SelectFolder:
					ofdlg = new NSOpenPanel();
					ofdlg.DirectoryUrl = new NSUrl(InitialDirectory);
					ofdlg.Title = Title;
					ofdlg.CanChooseFiles = false;
					ofdlg.CanChooseDirectories = true;
					ofdlg.AllowsMultipleSelection = false;

					resp = CocoaHelper.GetResponse(ofdlg.RunModal());
					SelectedPath = ofdlg.Url.Path;
					return resp;

				case FileDialogType.SaveFile:
					sfdlg = new NSSavePanel();
					sfdlg.DirectoryUrl = new NSUrl(InitialDirectory);
					sfdlg.Title = Title;
					sfdlg.CanCreateDirectories = true;

					resp = CocoaHelper.GetResponse(sfdlg.RunModal());
					SelectedPath = sfdlg.Url.Path;
					return resp;
			}

			return WindowResponse.None;
		}

		public override void Dispose()
		{
			if (ofdlg != null) ofdlg.Dispose();
			if (sfdlg != null) sfdlg.Dispose();
		}

	}
}


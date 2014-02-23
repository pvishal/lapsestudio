using System.Windows.Forms;
using Timelapse_UI;
using FileDialog = System.Windows.Forms.FileDialog;

namespace LapseStudioWinFormsUI
{
	public class WinFormFileDialog : Timelapse_UI.FileDialog
	{
        private FileDialog fdlg;
        private FolderBrowserDialog fbdlg;

        protected override Timelapse_UI.FileDialog GetDialog()
		{
			return this;
		}

		public override WindowResponse Show()
		{
            if (DialogType == FileDialogType.SelectFolder)
            {
                fbdlg = new FolderBrowserDialog();
                fbdlg.SelectedPath = InitialDirectory;
                fbdlg.ShowNewFolderButton = true;
                fbdlg.Description = Title;

                WindowResponse resp = WinFormHelper.GetResponse(fbdlg.ShowDialog());
                SelectedPath = fbdlg.SelectedPath;
                return resp;
            }
            else
            {
                switch (DialogType)
                {
                    case FileDialogType.OpenFile:
                        fdlg = new OpenFileDialog();
                        break;
                    case FileDialogType.SaveFile:
                        fdlg = new SaveFileDialog();
                        break;
                }

                fdlg.InitialDirectory = InitialDirectory;
                fdlg.Title = Title;
                fdlg.Filter = string.Empty;

                foreach (FileTypeFilter filter in FileTypeFilters)
                {
                    fdlg.Filter += filter.FilterName + "|";
                    foreach (string ft in filter.Filter) fdlg.Filter += "*." + ft;
                }
                WindowResponse resp = WinFormHelper.GetResponse(fdlg.ShowDialog());
                SelectedPath = fdlg.FileName;
                return resp;
            }
		}

		public override void Dispose()
		{
            if (fdlg != null) fdlg.Dispose();
            if (fbdlg != null) fbdlg.Dispose();
		}
	}
}


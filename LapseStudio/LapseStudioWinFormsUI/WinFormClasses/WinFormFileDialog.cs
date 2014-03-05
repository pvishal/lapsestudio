using System.Windows.Forms;
using Timelapse_UI;
using FileDialog = System.Windows.Forms.FileDialog;

namespace LapseStudioWinFormsUI
{
	public class WinFormFileDialog : Timelapse_UI.FileDialog
	{
        private FileDialog fdlg;
        private OpenFolderDialog fbdlg;
        private static IWin32Window owner;

        protected override Timelapse_UI.FileDialog GetDialog()
		{
			return this;
		}

        public static void InitOpenFolderDialog(IWin32Window win)
        {
            owner = win;
        }

		public override WindowResponse Show()
		{
            if (DialogType == FileDialogType.SelectFolder)
            {
                fbdlg = new OpenFolderDialog();
                fbdlg.InitialFolder = InitialDirectory;
                fbdlg.Title = Title;

                WindowResponse resp = WinFormHelper.GetResponse(fbdlg.ShowDialog(owner));
                SelectedPath = fbdlg.Folder;
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
                string tmpFilter = string.Empty;

                foreach (FileTypeFilter filter in FileTypeFilters)
                {
                    tmpFilter += filter.FilterName + "|";
                    for (int i = 0; i < filter.Filter.Length; i++) tmpFilter += (i == 0 ? "" : ";") + "*." + filter.Filter[i];
                }
                fdlg.Filter = tmpFilter;
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

using Gtk;
using Timelapse_UI;
using MessageTranslation;

namespace LapseStudioGtkUI
{
	public class GtkFileDialog : FileDialog
	{
		private FileChooserDialog fc;

		protected override FileDialog GetDialog()
		{
			return this;
		}

		public override WindowResponse Show()
		{
			FileChooserAction fca = FileChooserAction.Open;
			switch(DialogType)
			{
				case FileDialogType.OpenFile:
					fca = FileChooserAction.Open;
					break;
				case FileDialogType.SelectFolder:
					fca = FileChooserAction.SelectFolder;
					break;
				case FileDialogType.SaveFile:
					fca = FileChooserAction.Save;
					break;
			}

			fc = new FileChooserDialog(Title, null, fca, Message.GetString("Cancel"), ResponseType.Cancel, Message.GetString("Select"), ResponseType.Ok);
            fc.SetCurrentFolder(InitialDirectory);
			foreach(FileTypeFilter filter in FileTypeFilters)
			{
				FileFilter ft = new FileFilter();
				ft.Name = filter.FilterName;
				foreach(string pat in filter.Filter) ft.AddPattern("*." + pat);
				fc.AddFilter(ft);
			}
            WindowResponse resp = GtkHelper.GetResponse((ResponseType)fc.Run());
            SelectedPath = fc.Filename;
			return resp;
		}

		public override void Dispose()
		{
			if(fc != null) fc.Destroy();
		}
	}
}


using System.IO;
using System.Linq;
using Timelapse_API;
using MessageTranslation;

namespace Timelapse_UI
{
	public class FileTreeHelper
	{
		MessageBox MsgBox;
		FileDialog FDialog;

		public FileTreeHelper(MessageBox MsgBox, FileDialog FDialog)
		{
			this.MsgBox = MsgBox;
			this.FDialog = FDialog;
		}

		public void OpenMetaData(int index)
		{
			if (LSSettings.UsedProgram == ProjectType.CameraRaw)
			{
				if (((FrameACR)ProjectManager.CurrentProject.Frames[index]).XMPFile == null)
				{
					if (MsgBox.ShowMessage(MessageContent.RemoveMetadataLink) == WindowResponse.Yes) { ProjectManager.ReadXMP(); return; }

					using(FileDialog fdlg = FDialog.CreateDialog(FileDialogType.OpenFile, Message.GetString("Open XMP")))
					{
						fdlg.AddFileTypeFilter(new FileTypeFilter(Message.GetString("XMP"), "xmp", "XMP"));
						if(Directory.Exists(LSSettings.LastMetaDir)) fdlg.InitialDirectory = LSSettings.LastMetaDir;

						if(fdlg.Show() == WindowResponse.Ok)
						{
							LSSettings.LastMetaDir = Path.GetDirectoryName(fdlg.SelectedPath);
							ProjectManager.AddKeyframe(index, fdlg.SelectedPath);
						}
					}
				}
				else { ProjectManager.AddKeyframe(index); }
			}
			else if (LSSettings.UsedProgram == ProjectType.RawTherapee)
			{
				if (((FrameRT)ProjectManager.CurrentProject.Frames[index]).PP3File == null)
				{
					using(FileDialog fdlg = FDialog.CreateDialog(FileDialogType.OpenFile, Message.GetString("Open PP3")))
					{
						fdlg.AddFileTypeFilter(new FileTypeFilter(Message.GetString("Postprocessing Profile"), "PP3", "pp3"));
						if(Directory.Exists(LSSettings.LastMetaDir)) fdlg.InitialDirectory = LSSettings.LastMetaDir;

						if(fdlg.Show() == WindowResponse.Ok)
						{
							LSSettings.LastMetaDir = Path.GetDirectoryName(fdlg.SelectedPath);
							ProjectManager.AddKeyframe(index, fdlg.SelectedPath);
						}
					}
				}
				else { ProjectManager.AddKeyframe(index); }
			}
			else { ProjectManager.AddKeyframe(index); }
		}

		public void UpdateBrightness(int index, double CurrentValue)
		{
			double change = CurrentValue - ProjectManager.CurrentProject.Frames[index].AlternativeBrightness;
			ProjectManager.CurrentProject.Frames[index].AlternativeBrightness = CurrentValue;

			for (int i = index + 1; i < ProjectManager.CurrentProject.Frames.Count; i++)
			{
				ProjectManager.CurrentProject.Frames[i].AlternativeBrightness += change;
			}

			double min = ProjectManager.CurrentProject.Frames.Min(p => p.AlternativeBrightness);
			if (min < 0)
			{
				for (int i = 0; i < ProjectManager.CurrentProject.Frames.Count; i++)
				{
					ProjectManager.CurrentProject.Frames[i].AlternativeBrightness += min + 5;
				}
				change += min + 5;
			}
		}
	}
}
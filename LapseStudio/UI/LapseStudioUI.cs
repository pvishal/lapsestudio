using System;
using System.IO;
using MessageTranslation;
using Timelapse_API;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace Timelapse_UI
{
	public abstract class LapseStudioUI : IDisposable
    {
		#region Events

		public delegate void StringUpdate(string Value);

		public event StringUpdate InfoTextChanged;
		public event StringUpdate TitleChanged;

		#endregion

		#region Variables

		public string InfoText
		{
			get { return _InfoText; }
			set
			{
				if(_InfoText != value && InfoTextChanged != null) InfoTextChanged(value);
				_InfoText = value;
			}
		}
		private string _InfoText;

		public string Title
		{
			get { return _Title; }
			set
			{
				if(_Title != value && TitleChanged != null) TitleChanged(value);
				_Title = value;
			}
		}
		private string _Title;

		public MessageBox MsgBox;
		public FileDialog FDialog;
		private bool ProjectSaved = true;
		private string ProjectSavePath;

		public BrightnessGraph MainGraph;

		#endregion

		public LapseStudioUI(MessageBox MsgBox, FileDialog FDialog)
		{
			this.MsgBox = MsgBox;
			this.FDialog = FDialog;
            Error.Init(MsgBox);
		}

		#region Abstract Methods

        public abstract void Dispose();

        public abstract void ResetMovement();

		public abstract void InitMovement();

		public abstract void InvokeUI(Action action);

		public abstract void QuitApplication();

		public abstract void InitOpenedProject();

		public abstract void ResetProgress();

		public abstract void ResetPictureBoxes();

		public abstract void InitUI();

		#endregion

		#region Shared Methods

		public bool Quit(ClosingReason reason)
		{
			//the return value defines if the quiting should get cancelled or not
			if (reason != ClosingReason.Error)
			{
				WindowResponse res;

				if (ProjectManager.CurrentProject.IsWorking)
				{
					res = MsgBox.ShowMessage(MessageContent.BusyClose);
					if (res == WindowResponse.No) { return true; }
					else if (res == WindowResponse.Yes) { ProjectManager.Cancel(); }
				}

				res = AskForSaving();
				if (res == WindowResponse.Cancel) { return true; }
			}
			else { ProjectManager.Cancel(); }

			LSSettings.Save();

			this.Dispose();

			//TODO: add a waitone thing variable to project
			while (ProjectManager.CurrentProject.IsWorking) { System.Threading.Thread.Sleep(50); }

			QuitApplication();
			return false;
		}

		public WindowResponse AskForSaving()
		{
			if (!ProjectSaved)
			{
				WindowResponse res = MsgBox.ShowMessage(MessageContent.SaveQuestion);
				if (res == WindowResponse.Yes) { Click_SaveProject(false); return WindowResponse.Ok; }
				else if (res == WindowResponse.No) { return WindowResponse.Ok; }
				else { return WindowResponse.Cancel; }
			}
			return WindowResponse.Ok;
		}

		public void SetSaveStatus(bool isSaved)
		{
			ProjectSaved = isSaved;
			string t = "LapseStudio";
			if (isSaved && !String.IsNullOrEmpty(ProjectSavePath)) { t += " - " + Path.GetFileNameWithoutExtension(ProjectSavePath); }
			else if (String.IsNullOrEmpty(ProjectSavePath) && isSaved) { t += " - " + Message.GetString("NewProject"); }
			else if (String.IsNullOrEmpty(ProjectSavePath) && !isSaved) { t +=" - " + Message.GetString("NewProject") + "*"; }
			else { t += " - " + Path.GetFileNameWithoutExtension(ProjectSavePath) + "*"; }
			Title = t;
		}

		public void SavingProject()
		{
			SavingStorage Storage = new SavingStorage();
			using (GZipStream str = new GZipStream(File.Create(ProjectSavePath), CompressionMode.Compress))
			{
				BinaryFormatter ft = new BinaryFormatter();
				ft.Serialize(str, Storage);
			}
			MsgBox.ShowMessage(MessageContent.ProjectSaved);
		}

		public void OpeningProject()
		{
			using (GZipStream str = new GZipStream(File.Open(ProjectSavePath, FileMode.Open), CompressionMode.Decompress))
			{
				BinaryFormatter ft = new BinaryFormatter();
				SavingStorage Storage = (SavingStorage)ft.Deserialize(str);
				ProjectManager.OpenProject(Storage);
			}
		}

		public void ProcessFiles()
		{
			bool ask = true;
			if (LSSettings.UsedProgram == ProjectType.CameraRaw) { ask = false; }
			else if (LSSettings.UsedProgram == ProjectType.LapseStudio)
            {
                if (!File.Exists(LSSettings.RTPath)) { MsgBox.Show(Message.GetString("RawTherapee can't be found. Abort!")); return; }
                ask = true;
                ((ProjectLS)ProjectManager.CurrentProject).SaveFormat = LSSettings.SaveFormat;
            }
			else if (LSSettings.UsedProgram == ProjectType.RawTherapee)
			{
				ask = ((ProjectRT)ProjectManager.CurrentProject).RunRT;
				((ProjectRT)ProjectManager.CurrentProject).SaveFormat = LSSettings.SaveFormat;
			}

			if (ask)
			{
				using(FileDialog fdlg = FDialog.CreateDialog(FileDialogType.SelectFolder, Message.GetString("Select Folder")))
				{
					if(Directory.Exists(LSSettings.LastProcDir)) fdlg.InitialDirectory = LSSettings.LastProcDir;
					if(fdlg.Show() == WindowResponse.Ok)
					{
						LSSettings.LastProcDir = fdlg.SelectedPath;
						if (ProjectManager.CurrentProject.IsBrightnessCalculated) { ProjectManager.SetAltBrightness(MainGraph.Points); }
						ProjectManager.ProcessFiles(fdlg.SelectedPath);
					}
				}
			}
			else
			{
				if (ProjectManager.CurrentProject.IsBrightnessCalculated) { ProjectManager.SetAltBrightness(MainGraph.Points); }
				ProjectManager.ProcessFiles();
			}
			SetSaveStatus(false);
		}

		public void Init(Platform RunningPlatform)
		{
            ProjectManager.Init(RunningPlatform);
			LSSettings.Init();
			string lang;
			switch (LSSettings.UsedLanguage)
			{
				case Language.English: lang = "en"; break;
				case Language.German: lang = "de"; break;

				default: lang = "en"; break;
			}
			Message.Init(lang);
		}

		public void InitBaseUI()
		{
			ProjectManager.NewProject(LSSettings.UsedProgram);
			if (LSSettings.UsedProgram == ProjectType.RawTherapee)
			{
				((ProjectRT)ProjectManager.CurrentProject).RunRT = LSSettings.RunRT;
				((ProjectRT)ProjectManager.CurrentProject).RTPath = LSSettings.RTPath;
				((ProjectRT)ProjectManager.CurrentProject).KeepPP3 = LSSettings.KeepPP3;
				((ProjectRT)ProjectManager.CurrentProject).JpgQuality = LSSettings.JpgQuality;
				((ProjectRT)ProjectManager.CurrentProject).TiffCompression = LSSettings.TiffCompression != TiffCompressionFormat.None;
			}
			ResetMovement();
			MainGraph.Init();
			ProjectManager.Threadcount = LSSettings.Threadcount;
			InitUI();
		}

		public bool CheckBusy()
		{
			if (ProjectManager.CurrentProject.IsWorking) { MsgBox.ShowMessage(MessageContent.IsBusy); return true; }
			else return false;
		}

        public void SetBrightnessScale(double Val)
        {
            if (ProjectManager.CurrentProject.IsBrightnessCalculated)
            {
                for (int i = 1; i < ProjectManager.CurrentProject.Frames.Count; i++)
                {
                    //TODO_L: make brightness scale working
                    double orig1 = ProjectManager.CurrentProject.Frames[i - 1].OriginalBrightness;
                    double orig2 = ProjectManager.CurrentProject.Frames[i].OriginalBrightness;
                    ProjectManager.CurrentProject.Frames[i].AlternativeBrightness = orig2 + ((orig2 - orig1) * Val / 100);
                }
            }
        }

        public void SettingsChanged()
        {
            if (LSSettings.UsedProgram != ProjectManager.CurrentProject.Type) { if (Click_NewProject() == WindowResponse.Cancel) { LSSettings.UsedProgram = ProjectManager.CurrentProject.Type; } }
            else if (LSSettings.UsedProgram == ProjectType.RawTherapee) { ((ProjectRT)ProjectManager.CurrentProject).RTPath = LSSettings.RTPath; }
            ProjectManager.Threadcount = LSSettings.Threadcount;
        }

		#endregion

		#region User Input Methods

		public void Click_SaveProject(bool alwaysAsk)
		{
			if (CheckBusy()) return;
			if (File.Exists(ProjectSavePath) && !alwaysAsk) { SavingProject(); }
			else
			{
				using(FileDialog fdlg = FDialog.CreateDialog(FileDialogType.SaveFile, Message.GetString("Save Project")))
				{
					fdlg.AddFileTypeFilter(new FileTypeFilter(Message.GetString("LapseStudio Project"), "lasp"));
					if(Directory.Exists(LSSettings.LastProjDir)) fdlg.InitialDirectory = LSSettings.LastProjDir;
					if(fdlg.Show() == WindowResponse.Ok)
					{
						if (Path.GetExtension(fdlg.SelectedPath) != ".lasp") { Path.ChangeExtension(fdlg.SelectedPath, ".lasp"); }
						LSSettings.LastProjDir = Path.GetDirectoryName(fdlg.SelectedPath);
						ProjectSavePath = fdlg.SelectedPath;
						SavingProject();
					}
				}
			}
			SetSaveStatus(true);
		}

		public void Click_OpenProject()
		{
			if (CheckBusy()) return;
			using(FileDialog fdlg = FDialog.CreateDialog(FileDialogType.OpenFile, Message.GetString("Open Project")))
			{
				fdlg.AddFileTypeFilter(new FileTypeFilter(Message.GetString("LapseStudio Project"), "lasp"));
				if(Directory.Exists(LSSettings.LastProjDir)) fdlg.InitialDirectory = LSSettings.LastProjDir;
				if(fdlg.Show() == WindowResponse.Ok)
				{
					LSSettings.LastProjDir = System.IO.Path.GetDirectoryName(fdlg.SelectedPath);
					ProjectSavePath = fdlg.SelectedPath;
					OpeningProject();
				}
			}
			SetSaveStatus(true);
		}

		public void Click_AddFrames()
		{
			if (CheckBusy()) return;
			if (ProjectManager.CurrentProject.Frames.Count == 0)
			{
				using(FileDialog fdlg = FDialog.CreateDialog(FileDialogType.SelectFolder, Message.GetString("Select Folder")))
				{
					if(Directory.Exists(LSSettings.LastImgDir)) fdlg.InitialDirectory = LSSettings.LastImgDir;
					if(fdlg.Show() == WindowResponse.Ok)
					{
						LSSettings.LastImgDir = fdlg.SelectedPath;
						ProjectManager.AddFrame(fdlg.SelectedPath);
						SetSaveStatus(false);
					}
				}
			}
			else { MsgBox.ShowMessage(MessageContent.FramesAlreadyAdded); }
		}

		public WindowResponse Click_NewProject()
		{
			if (CheckBusy()) return WindowResponse.Cancel;
			WindowResponse res = AskForSaving();
			if (res == WindowResponse.Ok)
			{
				InitBaseUI();
				SetSaveStatus(true);
				ResetMovement();
				ResetPictureBoxes();
			}
			return res;
		}

		public void Click_Process()
		{
			if (CheckBusy()) return;
			if (ProjectManager.CurrentProject.KeyframeCount == 0) { MsgBox.ShowMessage(MessageContent.KeyframecountLow); }
			else if (ProjectManager.CurrentProject.IsBrightnessCalculated) { ProcessFiles(); }
			else if (LSSettings.UsedProgram == ProjectType.LapseStudio) { MsgBox.ShowMessage(MessageContent.BrightnessNotCalculatedError); }
			else if (MsgBox.ShowMessage(MessageContent.BrightnessNotCalculatedWarning) == WindowResponse.Yes) { ProcessFiles(); }
		}

		public void Click_RefreshMetadata()
		{
			if (CheckBusy()) return;
			if (ProjectManager.CurrentProject.Type == ProjectType.CameraRaw) { ProjectManager.ReadXMP(); }
		}

		public void Click_Calculate()
		{
			if (CheckBusy()) return;
			if (ProjectManager.CurrentProject.Frames.Count > 1) { ProjectManager.CalculateBrightness(LSSettings.BrCalcType); }
			else { MsgBox.ShowMessage(MessageContent.NotEnoughFrames); }
		}

		public void Click_ThumbEdit()
		{
			if (ProjectManager.CurrentProject.IsBrightnessCalculated)
			{
				ProjectManager.SetAltBrightness(MainGraph.Points);
				ProjectManager.ProcessThumbs();
			}
		}

		#endregion
    }
}

using System;
using System.IO;
using Timelapse_API;
using Timelapse_UI;
using MessageTranslation;

namespace Timelapse_UI
{
	public abstract class SettingsUI
	{
		MessageBox MsgBox;
        FileDialog FileDlg;

		public SettingsUI(MessageBox MsgBox, FileDialog FileDlg)
		{
			this.MsgBox = MsgBox;
            this.FileDlg = FileDlg;
		}

        public void InitUI()
        {
            InitLanguages(Enum.GetNames(typeof(Language)));
            InitPrograms(Enum.GetNames(typeof(ProjectType)));
            InitSaveFormats(Enum.GetNames(typeof(FileFormat)));
            InitBitDepths(Enum.GetNames(typeof(ImageBitDepth)));
            InitTiffCompressions(Enum.GetNames(typeof(TiffCompressionFormat)));
        }

		public void Load()
		{
			LSSettings.Read();
			ThreadCount = LSSettings.Threadcount;
			AutoThread = LSSettings.Autothread;
			LanguageSelection = (int)LSSettings.UsedLanguage;
			ProgramSelection = (int)LSSettings.UsedProgram;
			RunRT = LSSettings.RunRT;
			SaveFormatSelection = (int)LSSettings.SaveFormat;
			BitDepthSelection = (int)LSSettings.BitDepth;
			JpgQuality = LSSettings.JpgQuality;
			TiffCompressionSelection = (int)LSSettings.TiffCompression;
			RTPath = CheckRT();
			KeepPP3 = LSSettings.KeepPP3;
		}

		private string CheckRT()
		{
			if(!string.IsNullOrEmpty(LSSettings.RTPath) && AppExists(LSSettings.RTPath))return LSSettings.RTPath;
			else
			{
				string rt = ProjectRT.SearchForRT();
				if(rt != null) return rt;
				else return "";
			}
		}

		private bool AppExists(string path)
		{
			if(ProjectManager.RunningPlatform == Platform.MacOSX) return Directory.Exists(path);
			else return File.Exists(path);
		}

		public void Save()
		{
			LSSettings.Threadcount = ThreadCount;
			LSSettings.Autothread = AutoThread;
			LSSettings.UsedLanguage = (Language)LanguageSelection;
			LSSettings.UsedProgram = (ProjectType)ProgramSelection;
			LSSettings.RunRT = RunRT;
			LSSettings.SaveFormat = (FileFormat)SaveFormatSelection;
			LSSettings.BitDepth = (ImageBitDepth)BitDepthSelection;
			LSSettings.JpgQuality = JpgQuality;
			LSSettings.TiffCompression = (TiffCompressionFormat)TiffCompressionSelection;
			LSSettings.RTPath = RTPath;
			LSSettings.KeepPP3 = KeepPP3;
			LSSettings.Save();
		}

        public void BrowseRT()
        {
            using (FileDialog dlg = FileDlg.CreateDialog(FileDialogType.OpenFile, "Title"))
            {
                if (!string.IsNullOrWhiteSpace(RTPath) && Directory.Exists(Path.GetDirectoryName(RTPath)))
                {
                    dlg.InitialDirectory = Path.GetDirectoryName(RTPath);
                }
                else
                {
                    switch (ProjectManager.RunningPlatform)
                    {
                        case Platform.MacOSX:
                            dlg.InitialDirectory = "/Applications/";
                            break;
                        case Platform.Unix:
                            dlg.InitialDirectory = "/usr/bin/";
                            break;
                        case Platform.Windows:
                            dlg.InitialDirectory = @"C:\Program Files\";
                            break;
                    }
                }

				switch (ProjectManager.RunningPlatform)
				{
					case Platform.MacOSX:
						dlg.AddFileTypeFilter(new FileTypeFilter(Message.GetString("Application"), "app", "App"));
						break;
					case Platform.Windows:
						dlg.AddFileTypeFilter(new FileTypeFilter(Message.GetString("Executable"), "exe"));
						break;
				}

                if (dlg.Show() == WindowResponse.Ok) RTPath = dlg.SelectedPath;
            }
        }

        public void LanguageChanged()
		{
			if (LanguageSelection != (int)LSSettings.UsedLanguage)
			{
				MsgBox.Show(Message.GetString("Changing the language requires a restart."), MessageWindowType.Info, MessageWindowButtons.Ok);
			}
		}

        public void ProgramChanged()
		{
			switch ((ProjectType)ProgramSelection)
			{
				case ProjectType.LapseStudio:
					RunRTEnabled = false;
					SaveFormatEnabled = true;
					JpgQualityEnabled = true;
					TiffCompressionEnabled = true;
					BitDepthEnabled = true;
					KeepPP3Enabled = false;
					SaveFormatChanged();
					break;
				case ProjectType.RawTherapee:
					RunRTEnabled = true;
					SaveFormatChanged();
					RunRTChanged();
					break;
				case ProjectType.CameraRaw:
					RunRTEnabled = false;
					SaveFormatEnabled = false;
					JpgQualityEnabled = false;
					TiffCompressionEnabled = false;
					BitDepthEnabled = false;
					KeepPP3Enabled = false;
					break;
			}
		}

        public void SaveFormatChanged()
		{
			switch ((FileFormat)SaveFormatSelection)
			{
				case FileFormat.jpg:
					JpgQualityEnabled = true;
					TiffCompressionEnabled = false;
					BitDepthEnabled = false;
					break;
				case FileFormat.png:
					JpgQualityEnabled = false;
					TiffCompressionEnabled = false;
					BitDepthEnabled = false;
					break;
				case FileFormat.tiff:
					JpgQualityEnabled = false;
					TiffCompressionEnabled = true;
					BitDepthEnabled = ((ProjectType)ProgramSelection == ProjectType.RawTherapee) ? false : true;
					break;
			}
		}

        public void AutoThreadChanged()
		{
			if (AutoThread)
			{
				ThreadCountEnabled = false;
				ThreadCount = Environment.ProcessorCount;
			}
			else { ThreadCountEnabled = true; }
		}

        public void RunRTChanged()
		{
			SaveFormatEnabled = RunRT;
			JpgQualityEnabled = RunRT;
			TiffCompressionEnabled = RunRT;
			BitDepthEnabled = RunRT;
			KeepPP3Enabled = RunRT;
			KeepPP3 = !RunRT;
			if (RunRT) { SaveFormatChanged(); }
		}
        
        protected abstract bool SaveFormatEnabled { get; set; }
        protected abstract bool JpgQualityEnabled { get; set; }
        protected abstract bool TiffCompressionEnabled { get; set; }
        protected abstract bool BitDepthEnabled { get; set; }
        protected abstract bool KeepPP3Enabled { get; set; }
        protected abstract bool RunRTEnabled { get; set; }
        protected abstract bool ThreadCountEnabled { get; set; }
        
        protected abstract void InitLanguages(string[] Entries);
        protected abstract void InitPrograms(string[] Entries);
        protected abstract void InitSaveFormats(string[] Entries);
        protected abstract void InitBitDepths(string[] Entries);
        protected abstract void InitTiffCompressions(string[] Entries);
        
        public abstract string RTPath { get; set; }
        public abstract int ThreadCount { get; set; }
        public abstract int JpgQuality { get; set; }
        public abstract bool AutoThread { get; set; }
        public abstract bool KeepPP3 { get; set; }
        public abstract bool RunRT { get; set; }
        public abstract int LanguageSelection { get; set; }
        public abstract int ProgramSelection { get; set; }
        public abstract int SaveFormatSelection { get; set; }
        public abstract int BitDepthSelection { get; set; }
        public abstract int TiffCompressionSelection { get; set; }
	}
}
using System;
using Timelapse_API;
using Timelapse_UI;
using MessageTranslation;

namespace Timelapse_UI
{
	public abstract class SettingsUI
	{
		MessageBox MsgBox;

		public SettingsUI(MessageBox MsgBox)
		{
			this.MsgBox = MsgBox;
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
			RTPath = LSSettings.RTPath;
			KeepPP3 = LSSettings.KeepPP3;
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


		public abstract bool SaveFormatEnabled { get; set; }
		public abstract bool JpgQualityEnabled { get; set; }
		public abstract bool TiffCompressionEnabled { get; set; }
		public abstract bool BitDepthEnabled { get; set; }
		public abstract bool KeepPP3Enabled { get; set; }
		public abstract bool RunRTEnabled { get; set; }
		public abstract bool ThreadCountEnabled { get; set; }


		public abstract void InitLanguages(string[] Entries);
		public abstract void InitPrograms(string[] Entries);
		public abstract void InitSaveFormats(string[] Entries);
		public abstract void InitBitDepths(string[] Entries);
		public abstract void InitTiffCompressions(string[] Entries);


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


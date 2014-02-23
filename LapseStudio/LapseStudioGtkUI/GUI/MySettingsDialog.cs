using System;
using System.IO;
using Timelapse_API;
using Timelapse_UI;
using MessageTranslation;

namespace LapseStudioGtkUI
{
	public partial class MySettingsDialog : Gtk.Dialog
	{
		MessageBox MsgBox = new GtkMessageBox();

        public MySettingsDialog()
        {
            this.Build();
            RTchooseButton.Sensitive = false;
            RTchooseButton.SetFilename("/usr/bin/rawtherapee");
            Load();
        }

		private void Load()
		{
			LSSettings.Read();
			ThreadSpin.Value = LSSettings.Threadcount;
			AutothreadsChBox.Active = LSSettings.Autothread;
			string[] languages = Enum.GetNames(typeof(Language));
			foreach(string lang in languages) { LanguageCoBox.AppendText(lang); }
			LanguageCoBox.Active = (int)LSSettings.UsedLanguage;
			ProgramCoBox.Active = (int)LSSettings.UsedProgram;
			RunRTChBox.Active = LSSettings.RunRT;
            FormatCoBox.Active = (int)LSSettings.SaveFormat;
            BitdepthCoBox.Active = (int)LSSettings.BitDepth;
            JpgQualityScale.Value = LSSettings.JpgQuality;
            CompressionCoBox.Active = (int)LSSettings.TiffCompression;
            if (File.Exists(LSSettings.RTPath)) { RTchooseButton.SetFilename(LSSettings.RTPath); }
            KeepPP3ChBox.Active = LSSettings.KeepPP3;
            OnProgramCoBoxChanged(null, null);
		}

		private void Save()
		{
            LSSettings.Threadcount = (int)ThreadSpin.Value;
            LSSettings.Autothread = AutothreadsChBox.Active;
            LSSettings.UsedLanguage = (Language)LanguageCoBox.Active;
            LSSettings.UsedProgram = (Timelapse_API.ProjectType)ProgramCoBox.Active;
            LSSettings.RunRT = RunRTChBox.Active;
            LSSettings.SaveFormat = (FileFormat)FormatCoBox.Active;
            LSSettings.BitDepth = (ImageBitDepth)BitdepthCoBox.Active;
            LSSettings.JpgQuality = (int)JpgQualityScale.Value;
            LSSettings.TiffCompression = (TiffCompressionFormat)CompressionCoBox.Active;
            if (System.IO.Path.GetFileName(RTchooseButton.Filename) == "rawtherapee") { LSSettings.RTPath = RTchooseButton.Filename; }
            LSSettings.KeepPP3 = KeepPP3ChBox.Active;
			LSSettings.Save();
		}


		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			this.HideAll();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			Save();
			this.HideAll();
		}

        protected void OnProgramCoBoxChanged(object sender, EventArgs e)
        {
            switch (ProgramCoBox.ActiveText)
            {
                case "LapseStudio":
                    RunRTChBox.Sensitive = false;
                    FormatCoBox.Sensitive = true;
                    JpgQualityScale.Sensitive = true;
                    CompressionCoBox.Sensitive = true;
                    BitdepthCoBox.Sensitive = true;
                    KeepPP3ChBox.Sensitive = false;
                    //RTchooseButton.Sensitive = false;
                    OnFormatCoBoxChanged(null, e);
                    break;
                case "RawTherapee":
                    RunRTChBox.Sensitive = true;
                    OnFormatCoBoxChanged(null, e);
                    OnRunRTChBoxToggled(null, e);
                    break;
                case "Adobe CameraRaw":
                    RunRTChBox.Sensitive = false;
                    FormatCoBox.Sensitive = false;
                    JpgQualityScale.Sensitive = false;
                    CompressionCoBox.Sensitive = false;
                    BitdepthCoBox.Sensitive = false;
                    KeepPP3ChBox.Sensitive = false;
                    //RTchooseButton.Sensitive = false;
                    break;
            }
        }

		protected void OnFormatCoBoxChanged(object sender, EventArgs e)
		{
			switch (FormatCoBox.ActiveText)
			{
				case "jpg":
					JpgQualityScale.Sensitive = true;
					CompressionCoBox.Sensitive = false;
					BitdepthCoBox.Sensitive = false;
					break;
				case "png":
					JpgQualityScale.Sensitive = false;
					CompressionCoBox.Sensitive = false;
					BitdepthCoBox.Sensitive = false;
					break;
				case "tiff":
					JpgQualityScale.Sensitive = false;
                    CompressionCoBox.Sensitive = true;
					BitdepthCoBox.Sensitive = (ProgramCoBox.ActiveText == "RawTherapee") ? false : true;
					break;
			}
		}

		protected void OnAutothreadsChBoxToggled(object sender, EventArgs e)
		{
			if (AutothreadsChBox.Active)
			{
				ThreadSpin.Sensitive = false;
				ThreadSpin.Value = Environment.ProcessorCount;
			}
			else { ThreadSpin.Sensitive = true; }
		}

		protected void OnLanguageCoBoxChanged(object sender, EventArgs e)
		{
			if (LanguageCoBox.ActiveText != LSSettings.UsedLanguage.ToString())
			{
				MsgBox.Show(Message.GetString("Changing the language requires a restart."), MessageWindowType.Info, MessageWindowButtons.Ok);
			}
		}

		protected void OnRunRTChBoxToggled(object sender, EventArgs e)
        {
            FormatCoBox.Sensitive = RunRTChBox.Active;
            JpgQualityScale.Sensitive = RunRTChBox.Active;
            CompressionCoBox.Sensitive = RunRTChBox.Active;
            BitdepthCoBox.Sensitive = RunRTChBox.Active;
            KeepPP3ChBox.Sensitive = RunRTChBox.Active;
            KeepPP3ChBox.Active = !RunRTChBox.Active;
            //RTchooseButton.Sensitive = RunRTChBox.Active;
            if (RunRTChBox.Active) { OnFormatCoBoxChanged(null, e); }
		}

		protected void OnRTchooseButtonSelectionChanged(object sender, EventArgs e)
		{
            if (RTchooseButton.Filename != null)
            {
                if (System.IO.Path.GetFileNameWithoutExtension(RTchooseButton.Filename) != "rawtherapee") 
                {
					MsgBox.Show("This is not the correct file. \"rawtherapee\" is the right one.");
                }
            }
		}
	}
}


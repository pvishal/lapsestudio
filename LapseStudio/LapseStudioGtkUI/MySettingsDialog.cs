using System;
using Gtk;

namespace LapseStudioGtkUI
{
	public partial class MySettingsDialog : Dialog
	{
		GtkSettingsUI MySettings;

		public MySettingsDialog()
		{
			this.Build();
			MySettings = new GtkSettingsUI(this);
			MySettings.InitUI();
			MySettings.Load();
		}

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			this.HideAll();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			MySettings.Save();
			this.HideAll();
		}

		protected void OnProgramCoBoxChanged(object sender, EventArgs e)
		{
			MySettings.ProgramChanged();
		}

		protected void OnFormatCoBoxChanged(object sender, EventArgs e)
		{
			MySettings.SaveFormatChanged();
		}

		protected void OnLanguageCoBoxChanged(object sender, EventArgs e)
		{
			MySettings.LanguageChanged();
		}

		protected void OnRunRTChBoxToggled(object sender, EventArgs e)
		{
			MySettings.RunRTChanged();
		}

		protected void OnRTBrowseButtonClicked(object sender, System.EventArgs e)
		{
			MySettings.BrowseRT();
		}

		protected void OnAutothreadChBoxToggled(object sender, EventArgs e)
		{
			MySettings.AutoThreadChanged();
		}

		#region Widgets as Public

		public Button PublicRTBrowseButton { get { return RTBrowseButton; } }
		public ComboBox PublicLanguageCoBox { get { return LanguageCoBox; } }
		public ComboBox PublicProgramCoBox { get { return ProgramCoBox; } }
		public ComboBox PublicBitdepthCoBox { get { return BitdepthCoBox; } }
		public ComboBox PublicCompressionCoBox { get { return CompressionCoBox; } }
		public ComboBox PublicFormatCoBox { get { return FormatCoBox; } }
		public SpinButton PublicThreadSpin { get { return ThreadSpin; } }
		public HScale PublicJpgQualityScale { get { return JpgQualityScale; } }
		public CheckButton PublicKeepPP3ChBox { get { return KeepPP3ChBox; } }
		public CheckButton PublicRunRTChBox { get { return RunRTChBox; } }
		public CheckButton PublicAutothreadChBox { get { return AutothreadChBox; } }
		public Entry PublicRTPathBox { get { return RTPathBox; } }

		#endregion
	}
}


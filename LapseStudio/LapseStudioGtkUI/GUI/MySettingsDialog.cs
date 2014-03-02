using System;

namespace LapseStudioGtkUI
{
	public partial class MySettingsDialog : Gtk.Dialog
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

		protected void OnAutothreadsChBoxToggled(object sender, EventArgs e)
        {
            MySettings.AutoThreadChanged();
		}

		protected void OnLanguageCoBoxChanged(object sender, EventArgs e)
        {
            MySettings.LanguageChanged();
		}

		protected void OnRunRTChBoxToggled(object sender, EventArgs e)
        {
            MySettings.RunRTChanged();
		}
        
        protected void RTBrowseButton_Clicked(object sender, System.EventArgs e)
        {
            MySettings.BrowseRT();
        }
	}
}
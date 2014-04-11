using System;
using System.Windows.Forms;

namespace LapseStudioWinFormsUI
{
    public partial class MySettingsDialog : Form
    {
        WinFormSettingsUI Settings;

        public MySettingsDialog()
        {
            InitializeComponent();
            Settings = new WinFormSettingsUI(this);
            Settings.InitUI();
            Settings.Load();
        }
        
        private void OkButton_Click(object sender, EventArgs e)
        {
            Settings.Save();
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AutoThreadChBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.AutoThreadChanged();
        }

        private void LanguageCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.LanguageChanged();
        }

        private void ProgramCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.ProgramChanged();
        }

        private void OutputFormatCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.SaveFormatChanged();
        }

        private void RunRTChBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.RunRTChanged();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            Settings.BrowseRT();
        }

        private void JpgQualityTrackBar_Scroll(object sender, EventArgs e)
        {
            JpgQualityLabel.Text = JpgQualityTrackBar.Value.ToString();
        }
    }
}

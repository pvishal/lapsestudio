using System;
using System.Windows.Forms;
using System.IO;
using Timelapse_API;
using Timelapse_UI;
using MessageTranslation;
using Message = MessageTranslation.Message;
using FileDialog = System.Windows.Forms.FileDialog;

namespace LapseStudioWinFormsUI
{
    public partial class MySettingsDialog : Form
    {
        WinFormMessageBox MsgBox = new WinFormMessageBox();

        public MySettingsDialog()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            LSSettings.Read();
            ThreadUpDo.Value = LSSettings.Threadcount;
            AutoThreadChBox.Checked = LSSettings.Autothread;
            LanguageCoBox.Items.AddRange(Enum.GetNames(typeof(Language)));
            LanguageCoBox.SelectedIndex = (int)LSSettings.UsedLanguage;
            ProgramCoBox.SelectedIndex = (int)LSSettings.UsedProgram;
            RunRTChBox.Checked = LSSettings.RunRT;
            OutputFormatCoBox.SelectedIndex = (int)LSSettings.SaveFormat;
            BitDepthCoBox.SelectedIndex = (int)LSSettings.BitDepth;
            JpgQualityTrackBar.Value = LSSettings.JpgQuality;
            TiffCompCoBox.SelectedIndex = (int)LSSettings.TiffCompression;
            RTPathTextBox.Text = LSSettings.RTPath;
            KeepPP3ChBox.Checked = LSSettings.KeepPP3;
        }

        private void SaveSettings()
        {
            LSSettings.Threadcount = (int)ThreadUpDo.Value;
            LSSettings.Autothread = AutoThreadChBox.Enabled;
            LSSettings.UsedLanguage = (Language)LanguageCoBox.SelectedIndex;
            LSSettings.UsedProgram = (Timelapse_API.ProjectType)ProgramCoBox.SelectedIndex;
            LSSettings.RunRT = RunRTChBox.Checked;
            LSSettings.SaveFormat = (FileFormat)OutputFormatCoBox.SelectedIndex;
            LSSettings.BitDepth = (ImageBitDepth)BitDepthCoBox.SelectedIndex;
            LSSettings.JpgQuality = (int)JpgQualityTrackBar.Value;
            LSSettings.TiffCompression = (TiffCompressionFormat)TiffCompCoBox.SelectedIndex;
            LSSettings.RTPath = RTPathTextBox.Text;
            LSSettings.KeepPP3 = KeepPP3ChBox.Checked;
            LSSettings.Save();
        }


        private void OkButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AutoThreadChBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoThreadChBox.Checked)
            {
                ThreadUpDo.Enabled = false;
                ThreadUpDo.Value = Environment.ProcessorCount;
            }
            else { ThreadUpDo.Enabled = true; }
        }

        private void LanguageCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)LanguageCoBox.SelectedItem != LSSettings.UsedLanguage.ToString())
            {
                MsgBox.Show(Message.GetString("Changing the language requires a restart."), MessageWindowType.Info, MessageWindowButtons.Ok);
            }
        }

        private void ProgramCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((string)ProgramCoBox.SelectedItem)
            {
                case "LapseStudio":
                    RunRTChBox.Enabled = false;
                    OutputFormatCoBox.Enabled = true;
                    JpgQualityTrackBar.Enabled = true;
                    TiffCompCoBox.Enabled = true;
                    BitDepthCoBox.Enabled = true;
                    KeepPP3ChBox.Enabled = false;
                    BrowseButton.Enabled = false;
                    OutputFormatCoBox_SelectedIndexChanged(null, e);
                    break;
                case "RawTherapee":
                    RunRTChBox.Enabled = true;
                    OutputFormatCoBox_SelectedIndexChanged(null, e);
                    RunRTChBox_CheckedChanged(null, e);
                    break;
                case "Adobe CameraRaw":
                    RunRTChBox.Enabled = false;
                    OutputFormatCoBox.Enabled = false;
                    JpgQualityTrackBar.Enabled = false;
                    TiffCompCoBox.Enabled = false;
                    BitDepthCoBox.Enabled = false;
                    KeepPP3ChBox.Enabled = false;
                    BrowseButton.Enabled = false;
                    break;
            }
        }

        private void OutputFormatCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((string)OutputFormatCoBox.SelectedItem)
            {
                case "jpg":
                    JpgQualityTrackBar.Enabled = true;
                    TiffCompCoBox.Enabled = false;
                    BitDepthCoBox.Enabled = false;
                    break;
                case "png":
                    JpgQualityTrackBar.Enabled = false;
                    TiffCompCoBox.Enabled = false;
                    BitDepthCoBox.Enabled = false;
                    break;
                case "tiff":
                    JpgQualityTrackBar.Enabled = false;
                    TiffCompCoBox.Enabled = true;
                    BitDepthCoBox.Enabled = ((string)ProgramCoBox.SelectedItem == "RawTherapee") ? false : true;
                    break;
            }
        }

        private void RunRTChBox_CheckedChanged(object sender, EventArgs e)
        {
            OutputFormatCoBox.Enabled = RunRTChBox.Checked;
            JpgQualityTrackBar.Enabled = RunRTChBox.Checked;
            TiffCompCoBox.Enabled = RunRTChBox.Checked;
            BitDepthCoBox.Enabled = RunRTChBox.Checked;
            KeepPP3ChBox.Enabled = RunRTChBox.Checked;
            KeepPP3ChBox.Checked = !RunRTChBox.Checked;
            BrowseButton.Enabled = RunRTChBox.Checked;
            if (RunRTChBox.Checked) { OutputFormatCoBox_SelectedIndexChanged(null, e); }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(RTPathTextBox.Text) && Directory.Exists(Path.GetDirectoryName(RTPathTextBox.Text)))
            {
                MainOpenFileDialog.InitialDirectory = Path.GetDirectoryName(RTPathTextBox.Text);
            }

            if (MainOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetFileName(MainOpenFileDialog.FileName) != "rawtherapee.exe") MsgBox.Show("This is not the correct file. \"rawtherapee.exe\" is the right one.");
                else RTPathTextBox.Text = MainOpenFileDialog.FileName;
            }
        }

        private void JpgQualityTrackBar_Scroll(object sender, EventArgs e)
        {
            JpgQualityLabel.Text = JpgQualityTrackBar.Value.ToString();
        }
    }
}

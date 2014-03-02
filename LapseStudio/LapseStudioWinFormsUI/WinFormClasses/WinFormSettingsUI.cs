using Timelapse_UI;

namespace LapseStudioWinFormsUI
{
    public class WinFormSettingsUI : SettingsUI
    {
       MySettingsDialog Dlg;

       public WinFormSettingsUI(MySettingsDialog Dlg)
           : base(new WinFormMessageBox(), new WinFormFileDialog())
       {
           this.Dlg = Dlg;
       }
        
        protected override bool SaveFormatEnabled
        {
            get { return Dlg.OutputFormatCoBox.Enabled; }
            set { Dlg.OutputFormatCoBox.Enabled = value; }
        }

        protected override bool JpgQualityEnabled
        {
            get { return Dlg.JpgQualityTrackBar.Enabled; }
            set { Dlg.JpgQualityTrackBar.Enabled = value; }
        }

        protected override bool TiffCompressionEnabled
        {
            get { return Dlg.TiffCompCoBox.Enabled; }
            set { Dlg.TiffCompCoBox.Enabled = value; }
        }

        protected override bool BitDepthEnabled
        {
            get { return Dlg.BitDepthCoBox.Enabled; }
            set { Dlg.BitDepthCoBox.Enabled = value; }
        }

        protected override bool KeepPP3Enabled
        {
            get { return Dlg.KeepPP3ChBox.Enabled; }
            set { Dlg.KeepPP3ChBox.Enabled = value; }
        }

        protected override bool RunRTEnabled
        {
            get { return Dlg.RunRTChBox.Enabled; }
            set { Dlg.RunRTChBox.Enabled = value; }
        }

        protected override bool ThreadCountEnabled
        {
            get { return Dlg.ThreadUpDo.Enabled; }
            set { Dlg.ThreadUpDo.Enabled = value; }
        }
        

        protected override void InitLanguages(string[] Entries)
        {
            Dlg.LanguageCoBox.Items.Clear();
            Dlg.LanguageCoBox.Items.AddRange(Entries);
        }

        protected override void InitPrograms(string[] Entries)
        {
            Dlg.ProgramCoBox.Items.Clear();
            Dlg.ProgramCoBox.Items.AddRange(Entries);
        }

        protected override void InitSaveFormats(string[] Entries)
        {
            Dlg.OutputFormatCoBox.Items.Clear();
            Dlg.OutputFormatCoBox.Items.AddRange(Entries);
        }

        protected override void InitBitDepths(string[] Entries)
        {
            Dlg.BitDepthCoBox.Items.Clear();
            Dlg.BitDepthCoBox.Items.AddRange(Entries);
        }

        protected override void InitTiffCompressions(string[] Entries)
        {
            Dlg.TiffCompCoBox.Items.Clear();
            Dlg.TiffCompCoBox.Items.AddRange(Entries);
        }


        public override string RTPath
        {
            get { return Dlg.RTPathTextBox.Text; }
            set { Dlg.RTPathTextBox.Text = value; }
        }

        public override int ThreadCount
        {
            get { return (int)Dlg.ThreadUpDo.Value; }
            set { Dlg.ThreadUpDo.Value = value; }
        }

        public override int JpgQuality
        {
            get { return Dlg.JpgQualityTrackBar.Value; }
            set { Dlg.JpgQualityTrackBar.Value = value; }
        }

        public override bool AutoThread
        {
            get { return Dlg.AutoThreadChBox.Checked; }
            set { Dlg.AutoThreadChBox.Checked = value; }
        }

        public override bool KeepPP3
        {
            get { return Dlg.KeepPP3ChBox.Checked; }
            set { Dlg.KeepPP3ChBox.Checked = value; }
        }

        public override bool RunRT
        {
            get { return Dlg.RunRTChBox.Checked; }
            set { Dlg.RunRTChBox.Checked = value; }
        }

        public override int LanguageSelection
        {
            get { return Dlg.LanguageCoBox.SelectedIndex; }
            set { Dlg.LanguageCoBox.SelectedIndex = value; }
        }

        public override int ProgramSelection
        {
            get { return Dlg.ProgramCoBox.SelectedIndex; }
            set { Dlg.ProgramCoBox.SelectedIndex = value; }
        }

        public override int SaveFormatSelection
        {
            get { return Dlg.OutputFormatCoBox.SelectedIndex; }
            set { Dlg.OutputFormatCoBox.SelectedIndex = value; }
        }

        public override int BitDepthSelection
        {
            get { return Dlg.BitDepthCoBox.SelectedIndex; }
            set { Dlg.BitDepthCoBox.SelectedIndex = value; }
        }

        public override int TiffCompressionSelection
        {
            get { return Dlg.TiffCompCoBox.SelectedIndex; }
            set { Dlg.TiffCompCoBox.SelectedIndex = value; }
        }
    }
}

using Timelapse_UI;

namespace LapseStudioGtkUI
{
    public class GtkSettingsUI : SettingsUI
    {
        MySettingsDialog Dlg;

        public GtkSettingsUI(MySettingsDialog Dlg)
            : base(new GtkMessageBox(), new GtkFileDialog())
        {
            this.Dlg = Dlg;
        }
        
        protected override bool SaveFormatEnabled
        {
            get { return Dlg.FormatCoBox.Sensitive; }
            set { Dlg.FormatCoBox.Sensitive = value; }
        }

        protected override bool JpgQualityEnabled
        {
            get { return Dlg.JpgQualityScale.Sensitive; }
            set { Dlg.JpgQualityScale.Sensitive = value; }
        }

        protected override bool TiffCompressionEnabled
        {
            get { return Dlg.CompressionCoBox.Sensitive; }
            set { Dlg.CompressionCoBox.Sensitive = value; }
        }

        protected override bool BitDepthEnabled
        {
            get { return Dlg.BitdepthCoBox.Sensitive; }
            set { Dlg.BitdepthCoBox.Sensitive = value; }
        }

        protected override bool KeepPP3Enabled
        {
            get { return Dlg.KeepPP3ChBox.Sensitive; }
            set { Dlg.KeepPP3ChBox.Sensitive = value; }
        }

        protected override bool RunRTEnabled
        {
            get { return Dlg.RunRTChBox.Sensitive; }
            set { Dlg.RunRTChBox.Sensitive = value; }
        }

        protected override bool ThreadCountEnabled
        {
            get { return Dlg.ThreadSpin.Sensitive; }
            set { Dlg.ThreadSpin.Sensitive = value; }
        }
        

        protected override void InitLanguages(string[] Entries)
        {
            foreach (string entry in Entries) { Dlg.LanguageCoBox.AppendText(entry); }
        }

        protected override void InitPrograms(string[] Entries)
        {
            foreach (string entry in Entries) { Dlg.ProgramCoBox.AppendText(entry); }
        }

        protected override void InitSaveFormats(string[] Entries)
        {
            foreach (string entry in Entries) { Dlg.FormatCoBox.AppendText(entry); }
        }

        protected override void InitBitDepths(string[] Entries)
        {
            foreach (string entry in Entries) { Dlg.BitdepthCoBox.AppendText(entry); }
        }

        protected override void InitTiffCompressions(string[] Entries)
        {
            foreach (string entry in Entries) { Dlg.CompressionCoBox.AppendText(entry); }
        }
        

        public override string RTPath
        {
            get { return Dlg.RTPathBox.Text; }
            set { Dlg.RTPathBox.Text = value; }
        }

        public override int ThreadCount
        {
            get { return Dlg.ThreadSpin.ValueAsInt; }
            set { Dlg.ThreadSpin.Value = value; }
        }

        public override int JpgQuality
        {
            get { return (int)Dlg.JpgQualityScale.Value; }
            set { Dlg.JpgQualityScale.Value = value; }
        }

        public override bool AutoThread
        {
            get { return Dlg.AutothreadsChBox.Active; }
            set { Dlg.AutothreadsChBox.Active = value; }
        }

        public override bool KeepPP3
        {
            get { return Dlg.KeepPP3ChBox.Active; }
            set { Dlg.KeepPP3ChBox.Active = value; }
        }

        public override bool RunRT
        {
            get { return Dlg.RunRTChBox.Active; }
            set { Dlg.RunRTChBox.Active = value; }
        }

        public override int LanguageSelection
        {
            get { return Dlg.LanguageCoBox.Active; }
            set { Dlg.LanguageCoBox.Active = value; }
        }

        public override int ProgramSelection
        {
            get { return Dlg.ProgramCoBox.Active; }
            set { Dlg.ProgramCoBox.Active = value; }
        }

        public override int SaveFormatSelection
        {
            get { return Dlg.FormatCoBox.Active; }
            set { Dlg.FormatCoBox.Active = value; }
        }

        public override int BitDepthSelection
        {
            get { return Dlg.BitdepthCoBox.Active; }
            set { Dlg.BitdepthCoBox.Active = value; }
        }

        public override int TiffCompressionSelection
        {
            get { return Dlg.CompressionCoBox.Active; }
            set { Dlg.CompressionCoBox.Active = value; }
        }
    }
}

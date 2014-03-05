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
			get { return Dlg.PublicFormatCoBox.Sensitive; }
			set { Dlg.PublicFormatCoBox.Sensitive = value; }
        }

        protected override bool JpgQualityEnabled
        {
			get { return Dlg.PublicJpgQualityScale.Sensitive; }
			set { Dlg.PublicJpgQualityScale.Sensitive = value; }
        }

        protected override bool TiffCompressionEnabled
        {
			get { return Dlg.PublicCompressionCoBox.Sensitive; }
			set { Dlg.PublicCompressionCoBox.Sensitive = value; }
        }

        protected override bool BitDepthEnabled
        {
			get { return Dlg.PublicBitdepthCoBox.Sensitive; }
			set { Dlg.PublicBitdepthCoBox.Sensitive = value; }
        }

        protected override bool KeepPP3Enabled
        {
			get { return Dlg.PublicKeepPP3ChBox.Sensitive; }
			set { Dlg.PublicKeepPP3ChBox.Sensitive = value; }
        }

        protected override bool RunRTEnabled
        {
			get { return Dlg.PublicRunRTChBox.Sensitive; }
			set { Dlg.PublicRunRTChBox.Sensitive = value; }
        }

        protected override bool ThreadCountEnabled
        {
			get { return Dlg.PublicThreadSpin.Sensitive; }
			set { Dlg.PublicThreadSpin.Sensitive = value; }
        }
        

        protected override void InitLanguages(string[] Entries)
        {
			foreach (string entry in Entries) { Dlg.PublicLanguageCoBox.AppendText(entry); }
        }

        protected override void InitPrograms(string[] Entries)
        {
			foreach (string entry in Entries) { Dlg.PublicProgramCoBox.AppendText(entry); }
        }

        protected override void InitSaveFormats(string[] Entries)
        {
			foreach (string entry in Entries) { Dlg.PublicFormatCoBox.AppendText(entry); }
        }

        protected override void InitBitDepths(string[] Entries)
        {
			foreach (string entry in Entries) { Dlg.PublicBitdepthCoBox.AppendText(entry); }
        }

        protected override void InitTiffCompressions(string[] Entries)
        {
			foreach (string entry in Entries) { Dlg.PublicCompressionCoBox.AppendText(entry); }
        }
        

        public override string RTPath
        {
			get { return Dlg.PublicRTPathBox.Text; }
			set { Dlg.PublicRTPathBox.Text = value; }
        }

        public override int ThreadCount
        {
			get { return Dlg.PublicThreadSpin.ValueAsInt; }
			set { Dlg.PublicThreadSpin.Value = value; }
        }

        public override int JpgQuality
        {
			get { return (int)Dlg.PublicJpgQualityScale.Value; }
			set { Dlg.PublicJpgQualityScale.Value = value; }
        }

        public override bool AutoThread
        {
			get { return Dlg.PublicAutothreadChBox.Active; }
			set { Dlg.PublicAutothreadChBox.Active = value; }
        }

        public override bool KeepPP3
        {
			get { return Dlg.PublicKeepPP3ChBox.Active; }
			set { Dlg.PublicKeepPP3ChBox.Active = value; }
        }

        public override bool RunRT
        {
			get { return Dlg.PublicRunRTChBox.Active; }
			set { Dlg.PublicRunRTChBox.Active = value; }
        }

        public override int LanguageSelection
        {
			get { return Dlg.PublicLanguageCoBox.Active; }
			set { Dlg.PublicLanguageCoBox.Active = value; }
        }

        public override int ProgramSelection
        {
			get { return Dlg.PublicProgramCoBox.Active; }
			set { Dlg.PublicProgramCoBox.Active = value; }
        }

        public override int SaveFormatSelection
        {
			get { return Dlg.PublicFormatCoBox.Active; }
			set { Dlg.PublicFormatCoBox.Active = value; }
        }

        public override int BitDepthSelection
        {
			get { return Dlg.PublicBitdepthCoBox.Active; }
			set { Dlg.PublicBitdepthCoBox.Active = value; }
        }

        public override int TiffCompressionSelection
        {
			get { return Dlg.PublicCompressionCoBox.Active; }
			set { Dlg.PublicCompressionCoBox.Active = value; }
        }
    }
}

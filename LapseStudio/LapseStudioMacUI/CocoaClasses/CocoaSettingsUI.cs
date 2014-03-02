using System;
using Timelapse_UI;

namespace LapseStudioMacUI
{
	public class CocoaSettingsUI : SettingsUI
	{
		SettingsDialog StgDlg;

		public CocoaSettingsUI(SettingsDialog StgDlg) : base(new CocoaMessageBox())
		{
			this.StgDlg = StgDlg;
			Load();
		}

		public override void InitLanguages(string[] Entries)
		{
			throw new NotImplementedException();
		}

		public override void InitPrograms(string[] Entries)
		{
			throw new NotImplementedException();
		}

		public override void InitSaveFormats(string[] Entries)
		{
			throw new NotImplementedException();
		}

		public override void InitBitDepths(string[] Entries)
		{
			throw new NotImplementedException();
		}

		public override void InitTiffCompressions(string[] Entries)
		{
			throw new NotImplementedException();
		}

		public override bool SaveFormatEnabled {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool JpgQualityEnabled {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool TiffCompressionEnabled {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool BitDepthEnabled {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool KeepPP3Enabled {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool RunRTEnabled {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool ThreadCountEnabled {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override string RTPath {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override int ThreadCount {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override int JpgQuality {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool AutoThread {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool KeepPP3 {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool RunRT {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override int LanguageSelection {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override int ProgramSelection {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override int SaveFormatSelection {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override int BitDepthSelection {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override int TiffCompressionSelection {
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}


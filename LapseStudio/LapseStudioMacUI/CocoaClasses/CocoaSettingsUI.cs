using Timelapse_UI;
using MonoMac.AppKit;

namespace LapseStudioMacUI
{
	public class CocoaSettingsUI : SettingsUI
	{
		SettingsWindowController Dlg;

		public CocoaSettingsUI(SettingsWindowController Dlg)
			: base(new CocoaMessageBox(), new CocoaFileDialog())
		{
			this.Dlg = Dlg;
		}
			

		protected override void InitLanguages(string[] Entries)
		{
			Dlg.PublicLanguageCoBox.RemoveAllItems();
			Dlg.PublicLanguageCoBox.AddItems(Entries);
		}

		protected override void InitPrograms(string[] Entries)
		{
			Dlg.PublicProgramCoBox.RemoveAllItems();
			Dlg.PublicProgramCoBox.AddItems(Entries);
		}

		protected override void InitSaveFormats(string[] Entries)
		{
			Dlg.PublicSaveFormatCoBox.RemoveAllItems();
			Dlg.PublicSaveFormatCoBox.AddItems(Entries);
		}

		protected override void InitBitDepths(string[] Entries)
		{
			Dlg.PublicBitDepthCoBox.RemoveAllItems();
			Dlg.PublicBitDepthCoBox.AddItems(Entries);
		}

		protected override void InitTiffCompressions(string[] Entries)
		{
			Dlg.PublicTiffCompressionChBox.RemoveAllItems();
			Dlg.PublicTiffCompressionChBox.AddItems(Entries);
		}


		protected override bool SaveFormatEnabled
		{
			get { return Dlg.PublicSaveFormatCoBox.Enabled; }
			set { Dlg.PublicSaveFormatCoBox.Enabled = value; }
		}

		protected override bool JpgQualityEnabled
		{
			get { return Dlg.PublicJpgQualitySlider.Enabled; }
			set { Dlg.PublicJpgQualitySlider.Enabled = value; }
		}

		protected override bool TiffCompressionEnabled
		{
			get { return Dlg.PublicTiffCompressionChBox.Enabled; }
			set { Dlg.PublicTiffCompressionChBox.Enabled = value; }
		}

		protected override bool BitDepthEnabled
		{
			get { return Dlg.PublicBitDepthCoBox.Enabled; }
			set { Dlg.PublicBitDepthCoBox.Enabled = value; }
		}

		protected override bool KeepPP3Enabled
		{
			get { return Dlg.PublicKeepPP3ChBox.Enabled; }
			set { Dlg.PublicKeepPP3ChBox.Enabled = value; }
		}

		protected override bool RunRTEnabled
		{
			get { return Dlg.PublicRunRTChBox.Enabled; }
			set { Dlg.PublicRunRTChBox.Enabled = value; }
		}

		protected override bool ThreadCountEnabled
		{
			get { return Dlg.PublicThreadUpDo.Enabled; }
			set
			{
				Dlg.PublicThreadUpDo.Enabled = value;
				Dlg.PublicThreadBox.Enabled = value;
			}
		}


		public override string RTPath
		{
			get { return Dlg.PublicRTPathTextBox.StringValue; }
			set { Dlg.PublicRTPathTextBox.StringValue = value; }
		}

		public override int ThreadCount
		{
			get { return Dlg.PublicThreadUpDo.IntValue; }
			set
			{
				Dlg.PublicThreadUpDo.IntValue = value;
				Dlg.PublicThreadBox.IntValue = value;
			}
		}

		public override int JpgQuality
		{
			get { return Dlg.PublicJpgQualitySlider.IntValue; }
			set
			{
				Dlg.PublicJpgQualitySlider.IntValue = value;
				Dlg.PublicJpgQualityLabel.StringValue = value.ToString();
			}
		}

		public override bool AutoThread
		{
			get { return Dlg.PublicAutoThreadsChBox.State == NSCellStateValue.On; }
			set
			{
				Dlg.PublicAutoThreadsChBox.State = (value) ? NSCellStateValue.On : NSCellStateValue.Off;
				AutoThreadChanged();
			}
		}

		public override bool KeepPP3
		{
			get { return Dlg.PublicKeepPP3ChBox.State == NSCellStateValue.On; }
			set { Dlg.PublicKeepPP3ChBox.State = (value) ? NSCellStateValue.On : NSCellStateValue.Off; }
		}

		public override bool RunRT
		{
			get { return Dlg.PublicRunRTChBox.State == NSCellStateValue.On; }
			set { Dlg.PublicRunRTChBox.State = (value) ? NSCellStateValue.On : NSCellStateValue.Off; }
		}

		public override int LanguageSelection
		{
			get { return Dlg.PublicLanguageCoBox.IndexOfSelectedItem; }
			set
			{
				Dlg.PublicLanguageCoBox.SelectItem(value); 
				LanguageChanged();
			}
		}

		public override int ProgramSelection
		{
			get { return Dlg.PublicProgramCoBox.IndexOfSelectedItem; }
			set
			{
				Dlg.PublicProgramCoBox.SelectItem(value); 
				ProgramChanged();
			}
		}

		public override int SaveFormatSelection
		{
			get { return Dlg.PublicSaveFormatCoBox.IndexOfSelectedItem; }
			set
			{
				Dlg.PublicSaveFormatCoBox.SelectItem(value); 
				SaveFormatChanged();
			}
		}

		public override int BitDepthSelection
		{
			get { return Dlg.PublicBitDepthCoBox.IndexOfSelectedItem; }
			set { Dlg.PublicBitDepthCoBox.SelectItem(value); }
		}

		public override int TiffCompressionSelection
		{
			get { return Dlg.PublicTiffCompressionChBox.IndexOfSelectedItem; }
			set { Dlg.PublicTiffCompressionChBox.SelectItem(value); }
		}
	}
}
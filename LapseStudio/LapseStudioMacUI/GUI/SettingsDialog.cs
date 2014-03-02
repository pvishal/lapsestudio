using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Timelapse_API;
using Timelapse_UI;
using MessageTranslation;

namespace LapseStudioMacUI
{
	public partial class SettingsDialog : MonoMac.AppKit.NSWindow
	{
		#region Constructors

		// Called when created from unmanaged code
		public SettingsDialog(IntPtr handle) : base(handle)
		{
			Initialize();
		}
		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public SettingsDialog(NSCoder coder) : base(coder)
		{
			Initialize();
		}
		// Shared initialization code
		void Initialize()
		{
		}

		#endregion

		private CocoaSettingsUI Settings;
		public WindowResponse Result = WindowResponse.Cancel;

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
			Settings = new CocoaSettingsUI(this);
		}

		partial void CancelButton_Clicked(NSObject sender)
		{
			this.Close();
		}

		partial void OkButton_Clicked(NSObject sender)
		{
			Settings.Save();
			Result = WindowResponse.Ok;
			this.Close();
		}


		partial void AutoThreadsChBox_Toggled(NSObject sender)
		{
			Settings.AutoThreadChanged();
		}

		partial void JpgQualitySlider_Changed(NSObject sender)
		{
			JpgQualityLabel.StringValue = JpgQualitySlider.IntValue.ToString();
		}

		partial void LanguageCoBox_Changed(NSObject sender)
		{
			Settings.LanguageChanged();
		}

		partial void ProgramCoBox_Changed(NSObject sender)
		{
			Settings.ProgramChanged();
		}

		partial void RunRTChBox_Toggled(NSObject sender)
		{
			Settings.RunRTChanged();
		}

		partial void SaveFormatCoBox_Changed(NSObject sender)
		{
			Settings.SaveFormatChanged();
		}

		partial void ThreadUpDo_Changed(NSObject sender)
		{
			ThreadBox.IntValue = ThreadUpDo.IntValue;
		}

		partial void ThreadBox_Changed(NSObject sender)
		{
			ThreadUpDo.IntValue = ThreadBox.IntValue;
		}

		#region Controls as Public

		NSButton PublicAutoThreadsChBox { get { return AutoThreadsChBox; } set { AutoThreadsChBox = value; } }

		NSPopUpButton PublicBitDepthCoBox { get { return BitDepthCoBox; } set { BitDepthCoBox = value; } }

		NSButton PublicCancelButton { get { return CancelButton; } set { CancelButton = value; } }

		NSTextField PublicJpgQualityLabel { get { return JpgQualityLabel; } set { JpgQualityLabel = value; } }

		NSSlider PublicJpgQualitySlider { get { return JpgQualitySlider; } set { JpgQualitySlider = value; } }

		NSButton PublicKeepPP3ChBox { get { return KeepPP3ChBox; } set { KeepPP3ChBox = value; } }

		NSPopUpButton PublicLanguageCoBox { get { return LanguageCoBox; } set { LanguageCoBox = value; } }

		NSButton PublicOkButton { get { return OkButton; } set { OkButton = value; } }

		NSPopUpButton PublicProgramCoBox { get { return ProgramCoBox; } set { ProgramCoBox = value; } }

		NSButton PublicRunRTChBox { get { return RunRTChBox; } set { RunRTChBox = value; } }

		NSPopUpButton PublicSaveFormatCoBox { get { return SaveFormatCoBox; } set { SaveFormatCoBox = value; } }

		NSTextField PublicThreadBox { get { return ThreadBox; } set { ThreadBox = value; } }

		NSStepper PublicThreadUpDo { get { return ThreadUpDo; } set { ThreadUpDo = value; } }

		NSPopUpButton PublicTiffCompressionChBox { get { return TiffCompressionChBox; } set { TiffCompressionChBox = value; } }

		#endregion
	}
}


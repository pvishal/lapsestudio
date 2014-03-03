using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Timelapse_UI;

namespace LapseStudioMacUI
{
	public partial class SettingsWindowController : MonoMac.AppKit.NSWindowController
	{
		#region Constructors

		// Called when created from unmanaged code
		public SettingsWindowController(IntPtr handle) : base(handle)
		{
			Initialize();
		}
		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public SettingsWindowController(NSCoder coder) : base(coder)
		{
			Initialize();
		}
		// Call to load from the XIB/NIB file
		public SettingsWindowController() : base("SettingsWindow")
		{
			Initialize();
		}
		// Shared initialization code
		void Initialize()
		{
		}

		#endregion

		//strongly typed window accessor
		public new SettingsWindow Window { get { return (SettingsWindow)base.Window; } }

		private CocoaSettingsUI Settings;
		public WindowResponse Result = WindowResponse.Cancel;

		public override void AwakeFromNib()
		{
			try
			{
				base.AwakeFromNib();
				Settings = new CocoaSettingsUI(this);
				Settings.InitUI();
				Settings.Load();
			}
			catch (Exception ex) { Error.Report("Settings AwakeFromNib", ex); }
		}

		partial void CancelButton_Clicked(NSObject sender)
		{
			this.Close();
		}

		partial void OkButton_Clicked(NSObject sender)
		{
			try
			{
				Settings.Save();
				Result = WindowResponse.Ok;
				this.Close();
			}
			catch (Exception ex) { Error.Report("OkButton_Clicked", ex); }
		}


		partial void AutoThreadsChBox_Toggled(NSObject sender)
		{
			try { Settings.AutoThreadChanged(); }
			catch (Exception ex) { Error.Report("AutoThreadsChBox_Toggled", ex); }
		}

		partial void JpgQualitySlider_Changed(NSObject sender)
		{
			JpgQualityLabel.StringValue = JpgQualitySlider.IntValue.ToString();
		}

		partial void LanguageCoBox_Changed(NSObject sender)
		{
			try { Settings.LanguageChanged(); }
			catch (Exception ex) { Error.Report("LanguageCoBox_Changed", ex); }
		}

		partial void ProgramCoBox_Changed(NSObject sender)
		{
			try { Settings.ProgramChanged(); }
			catch (Exception ex) { Error.Report("ProgramCoBox_Changed", ex); }
		}

		partial void RunRTChBox_Toggled(NSObject sender)
		{
			try { Settings.RunRTChanged(); }
			catch (Exception ex) { Error.Report("RunRTChBox_Toggled", ex); }
		}

		partial void SaveFormatCoBox_Changed(NSObject sender)
		{
			try { Settings.SaveFormatChanged(); }
			catch (Exception ex) { Error.Report("SaveFormatCoBox_Changed", ex); }
		}

		partial void ThreadUpDo_Changed(NSObject sender)
		{
			ThreadBox.IntValue = ThreadUpDo.IntValue;
		}

		partial void ThreadBox_Changed(NSObject sender)
		{
			ThreadUpDo.IntValue = ThreadBox.IntValue;
		}

		partial void RTBrowseButton_Click(NSObject sender)
		{
			try { Settings.BrowseRT(); }
			catch (Exception ex) { Error.Report("RTBrowseButton_Click", ex); }
		}

		#region Controls as Public

		public NSButton PublicAutoThreadsChBox { get { return AutoThreadsChBox; } set { AutoThreadsChBox = value; } }
		public NSPopUpButton PublicBitDepthCoBox { get { return BitDepthCoBox; } set { BitDepthCoBox = value; } }
		public NSButton PublicCancelButton { get { return CancelButton; } set { CancelButton = value; } }
		public NSTextField PublicJpgQualityLabel { get { return JpgQualityLabel; } set { JpgQualityLabel = value; } }
		public NSSlider PublicJpgQualitySlider { get { return JpgQualitySlider; } set { JpgQualitySlider = value; } }
		public NSButton PublicKeepPP3ChBox { get { return KeepPP3ChBox; } set { KeepPP3ChBox = value; } }
		public NSPopUpButton PublicLanguageCoBox { get { return LanguageCoBox; } set { LanguageCoBox = value; } }
		public NSButton PublicOkButton { get { return OkButton; } set { OkButton = value; } }
		public NSPopUpButton PublicProgramCoBox { get { return ProgramCoBox; } set { ProgramCoBox = value; } }
		public NSButton PublicRunRTChBox { get { return RunRTChBox; } set { RunRTChBox = value; } }
		public NSPopUpButton PublicSaveFormatCoBox { get { return SaveFormatCoBox; } set { SaveFormatCoBox = value; } }
		public NSTextField PublicThreadBox { get { return ThreadBox; } set { ThreadBox = value; } }
		public NSStepper PublicThreadUpDo { get { return ThreadUpDo; } set { ThreadUpDo = value; } }
		public NSPopUpButton PublicTiffCompressionChBox { get { return TiffCompressionChBox; } set { TiffCompressionChBox = value; } }
		public NSTextField PublicRTPathTextBox { get { return RTPathTextBox; } set { RTPathTextBox = value; } }

		#endregion
	}
}


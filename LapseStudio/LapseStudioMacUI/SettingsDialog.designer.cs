// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace LapseStudioMacUI
{
	[Register ("SettingsDialog")]
	partial class SettingsDialog
	{
		[Outlet]
		MonoMac.AppKit.NSButton AutoThreadsChBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSPopUpButton BitDepthCoBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton CancelButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField JpgQualityLabel { get; set; }

		[Outlet]
		MonoMac.AppKit.NSSlider JpgQualitySlider { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton KeepPP3ChBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSPopUpButton LanguageCoBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton OkButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSPopUpButton ProgramCoBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton RTBrowseButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField RTPathTextBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton RunRTChBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSPopUpButton SaveFormatCoBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField ThreadBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSStepper ThreadUpDo { get; set; }

		[Outlet]
		MonoMac.AppKit.NSPopUpButton TiffCompressionChBox { get; set; }

		[Action ("AutoThreadsChBox_Toggled:")]
		partial void AutoThreadsChBox_Toggled (MonoMac.Foundation.NSObject sender);

		[Action ("CancelButton_Clicked:")]
		partial void CancelButton_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("JpgQualitySlider_Changed:")]
		partial void JpgQualitySlider_Changed (MonoMac.Foundation.NSObject sender);

		[Action ("LanguageCoBox_Changed:")]
		partial void LanguageCoBox_Changed (MonoMac.Foundation.NSObject sender);

		[Action ("OkButton_Clicked:")]
		partial void OkButton_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("ProgramCoBox_Changed:")]
		partial void ProgramCoBox_Changed (MonoMac.Foundation.NSObject sender);

		[Action ("RTBrowseButton_Click:")]
		partial void RTBrowseButton_Click (MonoMac.Foundation.NSObject sender);

		[Action ("RunRTChBox_Toggled:")]
		partial void RunRTChBox_Toggled (MonoMac.Foundation.NSObject sender);

		[Action ("SaveFormatCoBox_Changed:")]
		partial void SaveFormatCoBox_Changed (MonoMac.Foundation.NSObject sender);

		[Action ("ThreadBox_Changed:")]
		partial void ThreadBox_Changed (MonoMac.Foundation.NSObject sender);

		[Action ("ThreadUpDo_Changed:")]
		partial void ThreadUpDo_Changed (MonoMac.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AutoThreadsChBox != null) {
				AutoThreadsChBox.Dispose ();
				AutoThreadsChBox = null;
			}

			if (BitDepthCoBox != null) {
				BitDepthCoBox.Dispose ();
				BitDepthCoBox = null;
			}

			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}

			if (JpgQualityLabel != null) {
				JpgQualityLabel.Dispose ();
				JpgQualityLabel = null;
			}

			if (JpgQualitySlider != null) {
				JpgQualitySlider.Dispose ();
				JpgQualitySlider = null;
			}

			if (KeepPP3ChBox != null) {
				KeepPP3ChBox.Dispose ();
				KeepPP3ChBox = null;
			}

			if (LanguageCoBox != null) {
				LanguageCoBox.Dispose ();
				LanguageCoBox = null;
			}

			if (OkButton != null) {
				OkButton.Dispose ();
				OkButton = null;
			}

			if (ProgramCoBox != null) {
				ProgramCoBox.Dispose ();
				ProgramCoBox = null;
			}

			if (RTBrowseButton != null) {
				RTBrowseButton.Dispose ();
				RTBrowseButton = null;
			}

			if (RTPathTextBox != null) {
				RTPathTextBox.Dispose ();
				RTPathTextBox = null;
			}

			if (RunRTChBox != null) {
				RunRTChBox.Dispose ();
				RunRTChBox = null;
			}

			if (SaveFormatCoBox != null) {
				SaveFormatCoBox.Dispose ();
				SaveFormatCoBox = null;
			}

			if (ThreadBox != null) {
				ThreadBox.Dispose ();
				ThreadBox = null;
			}

			if (ThreadUpDo != null) {
				ThreadUpDo.Dispose ();
				ThreadUpDo = null;
			}

			if (TiffCompressionChBox != null) {
				TiffCompressionChBox.Dispose ();
				TiffCompressionChBox = null;
			}
		}
	}
}

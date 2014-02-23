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
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSButton AlignXButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSSlider BrightnessSlider { get; set; }

		[Outlet]
		MonoMac.AppKit.NSToolbarItem BrightnessToolItem { get; set; }

		[Outlet]
		MonoMac.AppKit.NSToolbarItem CancelToolItem { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton EditThumbsButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField FrameSelectedLabel { get; set; }

		[Outlet]
		MonoMac.AppKit.NSSlider FrameSelectSlider { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton GraphResetButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSImageView MainGraphBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSProgressIndicator MainProgressBar { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTableView MainTable { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTabView MainTabView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSToolbarItem MetadataToolItem { get; set; }

		[Outlet]
		MonoMac.AppKit.NSToolbarItem OpenFileToolItem { get; set; }

		[Outlet]
		MonoMac.AppKit.NSToolbarItem ProcessToolItem { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField Statuslabel { get; set; }

		[Outlet]
		MonoMac.AppKit.NSSegmentedControl TabChangeButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSImageView ThumbEditView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSImageView ThumbViewGraph { get; set; }

		[Outlet]
		MonoMac.AppKit.NSImageView ThumbViewList { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton YToEndButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton YToStartButton { get; set; }

		[Action ("AlignXButton_Click:")]
		partial void AlignXButton_Click (MonoMac.Foundation.NSObject sender);

		[Action ("BrightnessSlider_Changed:")]
		partial void BrightnessSlider_Changed (MonoMac.Foundation.NSObject sender);

		[Action ("BrightnessToolItem_Clicked:")]
		partial void BrightnessToolItem_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("CancelToolItem_Clicked:")]
		partial void CancelToolItem_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("EditThumbsButton_Click:")]
		partial void EditThumbsButton_Click (MonoMac.Foundation.NSObject sender);

		[Action ("FrameSelectSlider_Changed:")]
		partial void FrameSelectSlider_Changed (MonoMac.Foundation.NSObject sender);

		[Action ("GraphResetButton_Click:")]
		partial void GraphResetButton_Click (MonoMac.Foundation.NSObject sender);

		[Action ("MainTable_Changed:")]
		partial void MainTable_Changed (MonoMac.Foundation.NSObject sender);

		[Action ("MetadataToolItem_Clicked:")]
		partial void MetadataToolItem_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("OpenFileToolItem_Clicked:")]
		partial void OpenFileToolItem_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("ProcessToolItem_Clicked:")]
		partial void ProcessToolItem_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("TabChangeButton_Changed:")]
		partial void TabChangeButton_Changed (MonoMac.Foundation.NSObject sender);

		[Action ("YToEndButton_Click:")]
		partial void YToEndButton_Click (MonoMac.Foundation.NSObject sender);

		[Action ("YToStartButton_Click:")]
		partial void YToStartButton_Click (MonoMac.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (TabChangeButton != null) {
				TabChangeButton.Dispose ();
				TabChangeButton = null;
			}

			if (MainTabView != null) {
				MainTabView.Dispose ();
				MainTabView = null;
			}

			if (MainTable != null) {
				MainTable.Dispose ();
				MainTable = null;
			}

			if (MainProgressBar != null) {
				MainProgressBar.Dispose ();
				MainProgressBar = null;
			}

			if (Statuslabel != null) {
				Statuslabel.Dispose ();
				Statuslabel = null;
			}

			if (ThumbViewList != null) {
				ThumbViewList.Dispose ();
				ThumbViewList = null;
			}

			if (OpenFileToolItem != null) {
				OpenFileToolItem.Dispose ();
				OpenFileToolItem = null;
			}

			if (BrightnessToolItem != null) {
				BrightnessToolItem.Dispose ();
				BrightnessToolItem = null;
			}

			if (MetadataToolItem != null) {
				MetadataToolItem.Dispose ();
				MetadataToolItem = null;
			}

			if (ProcessToolItem != null) {
				ProcessToolItem.Dispose ();
				ProcessToolItem = null;
			}

			if (CancelToolItem != null) {
				CancelToolItem.Dispose ();
				CancelToolItem = null;
			}

			if (YToEndButton != null) {
				YToEndButton.Dispose ();
				YToEndButton = null;
			}

			if (YToStartButton != null) {
				YToStartButton.Dispose ();
				YToStartButton = null;
			}

			if (AlignXButton != null) {
				AlignXButton.Dispose ();
				AlignXButton = null;
			}

			if (GraphResetButton != null) {
				GraphResetButton.Dispose ();
				GraphResetButton = null;
			}

			if (EditThumbsButton != null) {
				EditThumbsButton.Dispose ();
				EditThumbsButton = null;
			}

			if (FrameSelectSlider != null) {
				FrameSelectSlider.Dispose ();
				FrameSelectSlider = null;
			}

			if (BrightnessSlider != null) {
				BrightnessSlider.Dispose ();
				BrightnessSlider = null;
			}

			if (ThumbViewGraph != null) {
				ThumbViewGraph.Dispose ();
				ThumbViewGraph = null;
			}

			if (ThumbEditView != null) {
				ThumbEditView.Dispose ();
				ThumbEditView = null;
			}

			if (MainGraphBox != null) {
				MainGraphBox.Dispose ();
				MainGraphBox = null;
			}

			if (FrameSelectedLabel != null) {
				FrameSelectedLabel.Dispose ();
				FrameSelectedLabel = null;
			}
		}
	}

	[Register ("MainWindow")]
	partial class MainWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}

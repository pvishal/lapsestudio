using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MessageTranslation;
using Timelapse_API;
using Timelapse_UI;

namespace LapseStudioMacUI
{
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController
	{
		CocoaUI MainUI;
		CocoaMessageBox MsgBox = new CocoaMessageBox();

		#region Constructors

		// Called when created from unmanaged code
		public MainWindowController(IntPtr handle) : base(handle)
		{
			Initialize();
		}
		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public MainWindowController(NSCoder coder) : base(coder)
		{
			Initialize();
		}
		// Call to load from the XIB/NIB file
		public MainWindowController() : base("MainWindow")
		{
			Initialize();
		}
		// Shared initialization code
		void Initialize()
		{
			MainUI = new CocoaUI(this, new CocoaMessageBox(), new CocoaFileDialog());
			MainUI.MainGraph = new BrightnessGraph((int)MainGraph.FittingSize.Width, (int)MainGraph.FittingSize.Height);
			((Graph)MainGraph).Init(MainUI.MainGraph);
			MainUI.InitBaseUI();
		}

		#endregion

		#region General

		//strongly typed window accessor
		public new MainWindow Window {
			get
			{
				return (MainWindow)base.Window;
			}
		}

		partial void TabChangeButton_Changed(NSObject sender)
		{
			MainTabView.SelectAt(TabChangeButton.SelectedSegment);
		}

		#endregion

		#region ToolBar
	
		partial void MetadataToolItem_Clicked(NSObject sender)
		{
			MainUI.Click_RefreshMetadata();
		}

		partial void OpenFileToolItem_Clicked(NSObject sender)
		{
			MainUI.Click_AddFrames();
		}

		partial void ProcessToolItem_Clicked(NSObject sender)
		{
			MainUI.Click_Process();
		}

		partial void BrightnessToolItem_Clicked(NSObject sender)
		{
			MainUI.Click_Calculate();
		}

		partial void CancelToolItem_Clicked(NSObject sender)
		{
			ProjectManager.Cancel();
		}

		#endregion

		#region Other Elements

		partial void BrightnessSlider_Changed(NSObject sender)
		{
		}

		partial void EditThumbsButton_Click(NSObject sender)
		{
			MainUI.Click_ThumbEdit();
		}

		partial void FrameSelectSlider_Changed(NSObject sender)
		{
			FrameSelectChanged();
		}

		partial void MainTable_Changed(NSObject sender)
		{

		}

		partial void YToEndButton_Click(NSObject sender)
		{
			MainUI.MainGraph.YtoEnd();
		}

		partial void YToStartButton_Click(NSObject sender)
		{
			MainUI.MainGraph.YtoStart();
		}

		partial void AlignXButton_Click(NSObject sender)
		{
			MainUI.MainGraph.AlignX();
		}

		partial void GraphResetButton_Click(NSObject sender)
		{
			MainUI.MainGraph.Reset();
		}

		#endregion

		public void FrameSelectChanged()
		{
			FrameSelectedLabel.StringValue = FrameSelectSlider.IntValue.ToString();
		}

		#region Controls as Public

		public NSButton PublicAlignXButton { get { return AlignXButton; } set { AlignXButton = value; } }

		public NSSlider PublicBrightnessSlider { get { return BrightnessSlider; } set { BrightnessSlider = value; } }

		public NSToolbarItem PublicBrightnessToolItem { get { return BrightnessToolItem; } set { BrightnessToolItem = value; } }

		public NSToolbarItem PublicCancelToolItem  { get { return CancelToolItem; } set { CancelToolItem = value; } }

		public NSButton PublicEditThumbsButton  { get { return EditThumbsButton; } set { EditThumbsButton = value; } }

		public NSTextField PublicFrameSelectedLabel { get { return FrameSelectedLabel; } set { FrameSelectedLabel = value; } }

		public NSSlider PublicFrameSelectSlider  { get { return FrameSelectSlider; } set { FrameSelectSlider = value; } }

		public NSButton PublicGraphResetButton  { get { return GraphResetButton; } set { GraphResetButton = value; } }

		public NSView PublicMainGraph { get { return MainGraph; } set { MainGraph = value; } }

		public NSProgressIndicator PublicMainProgressBar { get { return MainProgressBar; } set { MainProgressBar = value; } }

		public NSTableView PublicMainTable  { get { return MainTable; } set { MainTable = value; } }

		public NSTabView PublicMainTabView  { get { return MainTabView; } set { MainTabView = value; } }

		public NSToolbarItem PublicMetadataToolItem  { get { return MetadataToolItem; } set { MetadataToolItem = value; } }

		public NSToolbarItem PublicOpenFileToolItem  { get { return OpenFileToolItem; } set { OpenFileToolItem = value; } }

		public NSToolbarItem PublicProcessToolItem  { get { return ProcessToolItem; } set { ProcessToolItem = value; } }

		public NSTextField PublicStatuslabel  { get { return Statuslabel; } set { Statuslabel = value; } }

		public NSSegmentedControl PublicTabChangeButton  { get { return TabChangeButton; } set { TabChangeButton = value; } }

		public NSImageView PublicThumbEditView  { get { return ThumbEditView; } set { ThumbEditView = value; } }

		public NSImageView PublicThumbViewGraph  { get { return ThumbViewGraph; } set { ThumbViewGraph = value; } }

		public NSImageView PublicThumbViewList { get { return ThumbViewList; } set { ThumbViewList = value; } }

		public NSButton PublicYToEndButton  { get { return YToEndButton; } set { YToEndButton = value; } }

		public NSButton PublicYToStartButton  { get { return YToStartButton; } set { YToStartButton = value; } }

		#endregion
	}
}


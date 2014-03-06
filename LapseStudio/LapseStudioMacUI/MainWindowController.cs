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
		internal CocoaUI MainUI;
		internal CocoaMessageBox MsgBox = new CocoaMessageBox();

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

		public override void AwakeFromNib ()
		{
			try
			{
				base.AwakeFromNib();

				MainUI = new CocoaUI(this, new CocoaMessageBox(), new CocoaFileDialog());
				MainUI.MainGraph = new BrightnessGraph((int)MainGraph.FittingSize.Width, (int)MainGraph.FittingSize.Height);
				((Graph)MainGraph).Init(MainUI.MainGraph);
				MainUI.InitBaseUI();
			}
			catch(Exception ex) { Error.Report("Init", ex); }
		}

		partial void TabChangeButton_Changed(NSObject sender)
		{
			try { MainTabView.SelectAt(TabChangeButton.SelectedSegment); }
			catch (Exception ex) { Error.Report("TabChangeButton_Changed", ex); }
		}

		#endregion

		#region ToolBar
	
		partial void MetadataToolItem_Clicked(NSObject sender)
		{
			try { MainUI.Click_RefreshMetadata(); }
			catch (Exception ex) { Error.Report("MetadataToolItem_Clicked", ex); }
		}

		partial void OpenFileToolItem_Clicked(NSObject sender)
		{
			try { MainUI.Click_AddFrames(); }
			catch (Exception ex) { Error.Report("OpenFileToolItem_Clicked", ex); }
		}

		partial void ProcessToolItem_Clicked(NSObject sender)
		{
			try { MainUI.Click_Process(); }
			catch (Exception ex) { Error.Report("ProcessToolItem_Clicked", ex); }
		}

		partial void BrightnessToolItem_Clicked(NSObject sender)
		{
			try { MainUI.Click_Calculate(); }
			catch (Exception ex) { Error.Report("BrightnessToolItem_Clicked", ex); }
		}

		partial void CancelToolItem_Clicked(NSObject sender)
		{
			try { ProjectManager.Cancel(); }
			catch (Exception ex) { Error.Report("CancelToolItem_Clicked", ex); }
		}

		#endregion

		#region Other Elements

		partial void BrightnessSlider_Changed(NSObject sender)
		{
			try { MainUI.Click_BrightnessSlider(BrightnessSlider.DoubleValue); }
			catch (Exception ex) { Error.Report("BrightnessSlider_Changed", ex); }
		}

		partial void EditThumbsButton_Click(NSObject sender)
		{
			try { MainUI.Click_ThumbEdit(); }
			catch (Exception ex) { Error.Report("EditThumbsButton_Click", ex); }
		}

		partial void FrameSelectSlider_Changed(NSObject sender)
		{
			try { FrameSelectChanged(); }
			catch (Exception ex) { Error.Report("FrameSelectSlider_Changed", ex); }
		}

		partial void MainTable_Changed(NSObject sender)
		{
			try { }
			catch (Exception ex) { Error.Report("MainTable_Changed", ex); }
		}

		partial void YToEndButton_Click(NSObject sender)
		{
			try { MainUI.MainGraph.YtoEnd(); }
			catch (Exception ex) { Error.Report("YToEndButton_Click", ex); }
		}

		partial void YToStartButton_Click(NSObject sender)
		{
			try { MainUI.MainGraph.YtoStart(); }
			catch (Exception ex) { Error.Report("YToStartButton_Click", ex); }
		}

		partial void AlignXButton_Click(NSObject sender)
		{
			try { MainUI.MainGraph.AlignX(); }
			catch (Exception ex) { Error.Report("AlignXButton_Click", ex); }
		}

		partial void GraphResetButton_Click(NSObject sender)
		{
			try { MainUI.MainGraph.Reset(); }
			catch (Exception ex) { Error.Report("GraphResetButton_Click", ex); }
		}

		partial void KFCell_Changed(NSObject sender)
		{
			try { MainUI.Click_KeyframeToggle(MainTable.SelectedRow, (bool)((TableDataSource)MainTable.DataSource).Rows[MainTable.SelectedRow][(int)TableLocation.Keyframe]); }
			catch (Exception ex) { Error.Report("KFCell_Changed", ex); }
		}

		#endregion

		public void FrameSelectChanged()
		{
			FrameSelectedLabel.StringValue = FrameSelectSlider.IntValue.ToString();
			//TODO: show NSImage in graphviews
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

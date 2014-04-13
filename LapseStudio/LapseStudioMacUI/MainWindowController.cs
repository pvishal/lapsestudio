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
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController, IUIHandler
	{
		internal LapseStudioUI MainUI;
		internal CocoaMessageBox MsgBox = new CocoaMessageBox();
		TableDataSource TableSource = new TableDataSource();

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

				MainUI = new LapseStudioUI(Platform.MacOSX, this, new CocoaMessageBox(), new CocoaFileDialog());
				MainUI.MainGraph = new BrightnessGraph((int)MainGraph.FittingSize.Width, (int)MainGraph.FittingSize.Height);
				((Graph)MainGraph).Init(MainUI.MainGraph);
				MainUI.InitBaseUI();
				Window.Delegate = new WindowDelegate();
				MainTable.Delegate = new TableDelegate();
				((TableDelegate)MainTable.Delegate).TableSelectionChanged += HandleTableSelectionChanged;
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
			try { MainUI.Click_Calculate(BrightnessCalcType.Advanced); }
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
			try { RefreshImages(); }
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

		private void HandleTableSelectionChanged()
		{
			try
			{
				if(MainTable.SelectedRow >= 0 && TabChangeButton.SelectedSegment == (int)TabLocation.Filelist)
				{
					ThumbViewList.Image = CocoaHelper.ToNSImage(ProjectManager.CurrentProject.GetThumb(MainTable.SelectedRow));
				}
			}
			catch(Exception ex) { Error.Report("HandleTableSelectionChanged", ex); }
		}

		private void HandleBrightnessCellEdited(int Row, string value)
		{
			try { MainUI.UpdateBrightness(Row, value); }
			catch (Exception ex) { Error.Report("HandleBrightnessCellEdited", ex); }
		}

		#region IUIHandler implementation

		public void ReleaseUIData()
		{
			ResetPictureBoxes();
		}

		public void InvokeUI(Action action)
		{
			InvokeOnMainThread(new NSAction(delegate { action(); } ));
		}

		public void QuitApplication()
		{
			NSApplication.SharedApplication.Terminate(this);
		}

		public void InitOpenedProject()
		{
			MainUI.UpdateTable(false);
			SelectTableRow(0);
			RefreshImages();
			FrameSelectSlider.MinValue = 0;
			FrameSelectSlider.MaxValue = ProjectManager.CurrentProject.Frames.Count - 1;
		}

		public void SetProgress(int percent)
		{
			InvokeUI(() => { MainProgressBar.DoubleValue = percent; });
		}

		public void ResetPictureBoxes()
		{
			if (ThumbEditView.Image != null) { ThumbEditView.Image.Dispose(); }
			if (ThumbViewList.Image != null) { ThumbViewList.Image.Dispose(); }
			if (ThumbViewGraph.Image != null) { ThumbViewGraph.Image.Dispose(); }
		}

		public void InitTable()
		{
			MainTable.DataSource = TableSource;
			TableSource.BrightnessCellEdited += HandleBrightnessCellEdited;
		}

		public void InitUI()
		{
			MetadataToolItem.Enabled = (LSSettings.UsedProgram != ProjectType.CameraRaw) ? false : true;
			if(TableSource != null) TableSource = new TableDataSource();
			InitTable();
		}

		public void SelectTableRow(int index)
		{
			MainTable.SelectRow(index, false);
		}

		public void RefreshImages()
		{
			int val = FrameSelectSlider.IntValue;
			FrameSelectedLabel.StringValue = val.ToString();

			if (val >= 0 && val < ProjectManager.CurrentProject.Frames.Count)
			{
				ThumbEditView.Image = CocoaHelper.ToNSImage(ProjectManager.CurrentProject.GetThumbEdited(val));
				ThumbViewGraph.Image = CocoaHelper.ToNSImage(ProjectManager.CurrentProject.GetThumb(val));
			}
		}

		public void SetTableRow(int index, System.Collections.ArrayList values, bool fill)
		{
			if(fill) TableSource.Add(values);
			else { for(int i = 0; i < values.Count; i++) TableSource.Rows[index][i] = values[i]; }
			MainTable.ReloadData();
		}

		public void SetStatusLabel(string text)
		{
			Statuslabel.StringValue = text;
		}

		public void SetWindowTitle(string text)
		{
			Window.Title = text;
		}

		public void InitAfterFrameLoad()
		{
			FrameSelectSlider.MinValue = 0;
			FrameSelectSlider.MaxValue = ProjectManager.CurrentProject.Frames.Count - 1;
		}

		#endregion
	}
}

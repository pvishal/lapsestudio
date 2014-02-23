using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace LapseStudioMacUI
{
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController
	{
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

		partial void TabChangeButton_Changed(NSObject sender)
		{
			MainTabView.SelectAt(TabChangeButton.SelectedSegment);
		}

		#endregion

		#region ToolBar



		partial void MetadataToolItem_Clicked(NSObject sender)
		{
			throw new System.NotImplementedException();
		}

		partial void OpenFileToolItem_Clicked(NSObject sender)
		{
			throw new System.NotImplementedException();
		}

		partial void ProcessToolItem_Clicked(NSObject sender)
		{
			throw new System.NotImplementedException();
		}

		partial void BrightnessToolItem_Clicked(NSObject sender)
		{
			throw new System.NotImplementedException();
		}

		partial void CancelToolItem_Clicked(NSObject sender)
		{
			throw new System.NotImplementedException();
		}

		#endregion

		#region Other Elements

		partial void BrightnessSlider_Changed(NSObject sender)
		{
			throw new System.NotImplementedException();
		}

		partial void EditThumbsButton_Click(NSObject sender)
		{
			throw new System.NotImplementedException();
		}

		partial void FrameSelectSlider_Changed(NSObject sender)
		{
			FrameSelectedLabel.StringValue = FrameSelectSlider.IntValue.ToString();
		}

		partial void MainTable_Changed(NSObject sender)
		{
			throw new System.NotImplementedException();
		}

		partial void YToEndButton_Click(NSObject sender)
		{
			throw new System.NotImplementedException();
		}

		partial void YToStartButton_Click(NSObject sender)
		{
			throw new System.NotImplementedException();
		}

		partial void AlignXButton_Click(NSObject sender)
		{
			throw new System.NotImplementedException();
		}

		partial void GraphResetButton_Click(NSObject sender)
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Timelapse_API;
using Timelapse_UI;
using System.Drawing;
using MonoMac.CoreGraphics;

namespace LapseStudioMacUI
{
	public partial class Graph : MonoMac.AppKit.NSView
	{
		private BrightnessGraph BaseGraph;

		#region Constructors

		// Called when created from unmanaged code
		public Graph(IntPtr handle) : base(handle)
		{
			Initialize();
		}
		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public Graph(NSCoder coder) : base(coder)
		{
			Initialize();
		}
		// Shared initialization code
		void Initialize()
		{
		}

		#endregion

		public void Init(BrightnessGraph BaseGraph)
		{
			this.BaseGraph = BaseGraph;
			this.BaseGraph.RefreshGraph = Refresh;
			this.BaseGraph.Init();
		}

		#region Events

		private void Refresh()
		{
			//this.InvokeOnMainThread(delegate { DrawRect(Frame); });
			DrawRect(Frame);
		}

		public override void MouseUp(NSEvent theEvent)
		{
			base.MouseUp(theEvent);
			BaseGraph.Mouse_Up();
		}

		public override void MouseDown(NSEvent theEvent)
		{
			base.MouseDown(theEvent);
			BaseGraph.Mouse_Down(theEvent.LocationInWindow.X-this.Frame.Left, this.Frame.Height-theEvent.LocationInWindow.Y);
		}

		public override void MouseMoved(NSEvent theEvent)
		{
			base.MouseMoved(theEvent);
			BaseGraph.Mouse_Move(theEvent.LocationInWindow.X-this.Frame.Left, this.Frame.Height-theEvent.LocationInWindow.Y);
		}

		public override void DrawRect(RectangleF dirtyRect)
		{
			base.DrawRect(dirtyRect);

			//if(Frame.Width != BaseGraph.Width || Frame.Height != BaseGraph.Height) BaseGraph.SetNewSize((int)Frame.Width, (int)Frame.Height);

			using (CGContext graph = NSGraphicsContext.CurrentContext.GraphicsPort)
			using (CocoaGraphDrawer drawer = new CocoaGraphDrawer(graph, BaseGraph.Width, BaseGraph.Height))
			{
				BaseGraph.DrawFullGraph(drawer);
			}
		}

		public override void SetFrameSize(SizeF newSize)
		{
			base.SetFrameSize(newSize);
			BaseGraph.SetNewSize((int)newSize.Width, (int)newSize.Height);
		}

		public override void ViewDidUnhide()
		{
			base.ViewDidUnhide();
			Refresh();
		}

		#endregion
	}
}


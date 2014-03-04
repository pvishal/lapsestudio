using System;
using System.Drawing;
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using Timelapse_UI;

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
			BaseGraph.SetNewSize((int)Frame.Width, (int)Frame.Height);
		}

		#region Events

		private void Refresh()
		{
			this.InvokeOnMainThread(new NSAction(delegate { this.SetNeedsDisplayInRect(Bounds); } ));
		}

		public override void MouseUp(NSEvent theEvent)
		{
			base.MouseUp(theEvent);
			BaseGraph.Mouse_Up();
		}

		public override void MouseDown(NSEvent theEvent)
		{
			base.MouseDown(theEvent);
			PointF np = ConvertPointFromView(theEvent.LocationInWindow, null);
			BaseGraph.Mouse_Down(np.X, Bounds.Height - np.Y);
		}

		public override void MouseDragged(NSEvent theEvent)
		{
			base.MouseDragged(theEvent);
			PointF np = ConvertPointFromView(theEvent.LocationInWindow, null);
			BaseGraph.Mouse_Move(np.X,  Bounds.Height - np.Y);
		}

		public override void DrawRect(RectangleF dirtyRect)
		{
			base.DrawRect(dirtyRect);

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
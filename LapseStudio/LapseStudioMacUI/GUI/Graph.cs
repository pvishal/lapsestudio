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
			//this.BaseGraph.RefreshGraph = Refresh;
			this.BaseGraph.Init();
		}

		//#region Events

		private void Refresh()
		{
			//this.InvokeOnMainThread(delegate { GraphEventBox_ExposeEvent(GraphView, null); });
		}

		public override void MouseUp(NSEvent theEvent)
		{
			base.MouseUp(theEvent);
		}

		public override void MouseDown(NSEvent theEvent)
		{
			base.MouseDown(theEvent);
		}

		public override void MouseMoved(NSEvent theEvent)
		{
			base.MouseMoved(theEvent);
		}

		public override void DrawRect(RectangleF dirtyRect)
		{
			base.DrawRect(dirtyRect);

			var context = NSGraphicsContext.CurrentContext.GraphicsPort;
			context.SetStrokeColor(new CGColor(1.0f, 0f, 0f)); // red
			context.SetLineWidth(1.0F);
			context.StrokeEllipseInRect(new RectangleF(5, 5, 10, 10));
		}

		/*protected void GraphEventBox_ExposeEvent(object o, ExposeEventArgs args)
		{
			//Draw all Graph-Parts together:
			using(Context MainGraph = Gdk.CairoHelper.Create(((DrawingArea)o).GdkWindow))
			using(ImageSurface tmpSurface = new ImageSurface(Format.Rgb24, ((DrawingArea)o).Allocation.Width, ((DrawingArea)o).Allocation.Height))
			using(Context GraphHelper = new Context(tmpSurface))
			{
				//Clear
				ClearGraph(GraphHelper);

				//Draw a gray Background
				GraphHelper.SetSourceRGB(0.9, 0.9, 0.9);
				GraphHelper.Rectangle(0, 0, BaseGraph.Width, BaseGraph.Height);
				GraphHelper.Fill();

				GraphHelper.Antialias = Antialias.Subpixel;
				GraphHelper.LineWidth = 1;
				GraphHelper.SetSourceRGB(0, 0, 0);

				//Draw the Scale and Graph
				DrawScale(GraphHelper);
				DrawCurve(GraphHelper);
				MainGraph.SetSourceSurface(tmpSurface, 0, 0);
				MainGraph.Paint();
			}
		}

		protected void GraphEventBox_SizeAllocated(object o, SizeAllocatedArgs args)
		{
			if (BaseGraph != null) BaseGraph.SetNewSize(this.Allocation.Width, this.Allocation.Height);
		}

		protected void OnGraphEventBoxButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			BaseGraph.Mouse_Down(args.Event.X, args.Event.Y);
		}

		protected void OnGraphEventBoxButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			BaseGraph.Mouse_Up();
		}

		protected void OnGraphEventBoxMotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			BaseGraph.Mouse_Move(args.Event.X, args.Event.Y);
		}

		#endregion

		#region Drawing

		private void DrawScale(Context ctx)
		{
			double px = 0.5;        //needed to get a clear, one pixel line w/o AA
			double lw = 10.5;       //width of the scale lines

			//draw Y axe
			ctx.MoveTo(BrightnessGraph.Left - px, BrightnessGraph.Top);
			ctx.LineTo(BrightnessGraph.Left - px, BaseGraph.Height - BrightnessGraph.Bottom);
			ctx.Stroke();

			ctx.MoveTo(BaseGraph.Width - BrightnessGraph.Right - px, BrightnessGraph.Top);
			ctx.LineTo(BaseGraph.Width - BrightnessGraph.Right - px, BaseGraph.Height - BrightnessGraph.Bottom + lw);
			ctx.Stroke();

			//draw X axe
			ctx.MoveTo(BrightnessGraph.Left - px, BaseGraph.Height - BrightnessGraph.Bottom - px);
			ctx.LineTo(BaseGraph.Width - BrightnessGraph.Right, BaseGraph.Height - BrightnessGraph.Bottom - px);
			ctx.Stroke();

			ctx.MoveTo(BrightnessGraph.Left - lw, BrightnessGraph.Top - px);
			ctx.LineTo(BaseGraph.Width - BrightnessGraph.Right, BrightnessGraph.Top - px);
			ctx.Stroke();

			double Xs = (BaseGraph.Width - BrightnessGraph.Left - BrightnessGraph.Right) / 10d;
			double Ys = (BaseGraph.Height - BrightnessGraph.Top - BrightnessGraph.Bottom) / 10d;

			//draw X and Y scales
			for (int i = 1; i < 10; i++)
			{
				//Y scale
				ctx.MoveTo(BrightnessGraph.Left - lw, (int)(BaseGraph.Height - BrightnessGraph.Bottom - i * Ys) - px);
				ctx.LineTo(BaseGraph.Width - BrightnessGraph.Right - px, (int)(BaseGraph.Height - BrightnessGraph.Bottom - i * Ys) - px);
				ctx.Stroke();

				//X scale
				ctx.MoveTo((int)(BrightnessGraph.Left + i * Xs) - px, BrightnessGraph.Top - px);
				ctx.LineTo((int)(BrightnessGraph.Left + i * Xs) - px, BaseGraph.Height - BrightnessGraph.Bottom + lw);
				ctx.Stroke();
			}
		}

		private void DrawCurve(Context ctx)
		{
			PointD[] pout;

			#region Drawing Brightness Curve

			if (ProjectManager.CurrentProject.Frames.Count > 0)
			{
				ctx.SetSourceRGB(0.8, 0.8, 0);

				List<PointD> BrP = new List<PointD>();
				for (int i = 0; i < ProjectManager.CurrentProject.Frames.Count; i++)
				{
					BrP.Add(new PointD(i, (float)ProjectManager.CurrentProject.Frames[i].AlternativeBrightness));
				}

				pout = BaseGraph.RealToGraph(BrP);

				for (int i = 1; i < pout.Length; i++)
				{
					ctx.MoveTo(pout[i - 1].X, pout[i - 1].Y);
					ctx.LineTo(pout[i].X, pout[i].Y);
					ctx.Stroke();
				}
			}

			#endregion

			if (BaseGraph.Points.Count > 0 && ProjectManager.CurrentProject.IsBrightnessCalculated)
			{
				#region Drawing Custom Curve

				ctx.SetSourceRGB(0, 0, 0);

				pout = BaseGraph.RealToGraph(Interpolation.Do(BaseGraph.Points.ToArray(), BrightnessGraph.Smoothness));

				for (int i = 1; i < pout.Length; i++)
				{
					ctx.MoveTo(pout[i - 1].X, pout[i - 1].Y);
					ctx.LineTo(pout[i].X, pout[i].Y);
					ctx.Stroke();
				}

				#endregion

				#region Drawing Points

				ctx.Save();
				ctx.SetSourceRGB(0.8, 0, 0);
				for (int i = 0; i < BaseGraph.Points.Count; i++)
				{
					PointD tmp = BaseGraph.RealToGraph(BaseGraph.Points[i]);
					ctx.Arc(tmp.X, tmp.Y, 3, 0, 2 * Math.PI);
					ctx.Fill();
				}
				ctx.SetSourceRGB(0, 0.8, 0.8);
				PointD tmpSel = BaseGraph.RealToGraph(BaseGraph.Points[BaseGraph.SelectedPoint]);
				ctx.Arc(tmpSel.X, tmpSel.Y, 3, 0, 2 * Math.PI);
				ctx.Fill();
				ctx.Restore();

				#endregion
			}
		}

		private void ClearGraph(Context ctx)
		{
			ctx.Save();
			ctx.Operator = Operator.Clear;
			ctx.Paint();
			ctx.Restore();
		}

		#endregion*/
	}
}


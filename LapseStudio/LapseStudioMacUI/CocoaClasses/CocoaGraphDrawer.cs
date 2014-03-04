using System;
using System.Drawing;
using MonoMac.CoreGraphics;
using Timelapse_UI;

namespace LapseStudioMacUI
{
	public class CocoaGraphDrawer : IGraphDrawer
	{
		CGContext g;
		int w, h;
		private static CGColor BaseColor = new CGColor(BrightnessGraph.BasePenColor[0],BrightnessGraph.BasePenColor[1],BrightnessGraph.BasePenColor[2]);
		private static CGColor CurveColor = new CGColor(BrightnessGraph.CurvePenColor[0],BrightnessGraph.CurvePenColor[1],BrightnessGraph.CurvePenColor[2]);
		private static CGColor CustomCurveColor = new CGColor(BrightnessGraph.CustomCurvePenColor[0],BrightnessGraph.CustomCurvePenColor[1],BrightnessGraph.CustomCurvePenColor[2]);
		private static CGColor PointColor = new CGColor(BrightnessGraph.PointPenColor[0],BrightnessGraph.PointPenColor[1],BrightnessGraph.PointPenColor[2]);
		private static CGColor SelectedPointColor = new CGColor(BrightnessGraph.SelPointPenColor[0],BrightnessGraph.SelPointPenColor[1],BrightnessGraph.SelPointPenColor[2]);
		private static CGColor BackgroundColor = new CGColor(BrightnessGraph.GraphBackground[0],BrightnessGraph.GraphBackground[1],BrightnessGraph.GraphBackground[2]);

		public CocoaGraphDrawer(CGContext g, int w, int h)
		{
			this.g = g;
			this.w = w;
			this.h = h;
			g.SetAllowsAntialiasing(true);
			g.InterpolationQuality = CGInterpolationQuality.High;
		}
			
		public void DrawLine(float xB, float yB, float xE, float yE)
		{
			g.StrokeLineSegments(new PointF[]{ new PointF(xB, h-yB), new PointF(xE, h-yE) });
		}

		public void DrawCircle(float x, float y, float d)
		{
			g.FillEllipseInRect(new RectangleF(x, h - y - d, d, d));
		}

		public void SetBaseLinePen()
		{
			g.SetStrokeColor(BaseColor);
			g.SetLineWidth(1f);
		}

		public void SetCurveLinePen()
		{
			g.SetStrokeColor(CurveColor);
			g.SetLineWidth(1f);
		}

		public void SetCustomCurveLinePen()
		{
			g.SetStrokeColor(CustomCurveColor);
			g.SetLineWidth(1f);
		}

		public void SetPointPen()
		{
			g.SetFillColor(PointColor);
		}

		public void SetSelectedPointPen()
		{
			g.SetFillColor(SelectedPointColor);
		}

		public void ClearGraph()
		{
			g.SetFillColor(BackgroundColor);
			g.FillRect(new RectangleF(0, 0, w, h));
		}

		public void DrawFull()
		{
			//Nothing to do here
		}

		public void Dispose()
		{
			g.Dispose();
		}
	}
}
using System;
using Cairo;
using Timelapse_UI;

namespace LapseStudioGtkUI
{
    public class GtkGraphDrawer : IGraphDrawer
    {
        Context g;
        Context gh;
        ImageSurface srfc;
        const float Pi2 = (float)(2 * Math.PI);

        public GtkGraphDrawer(Context g, Context gh, ImageSurface srfc)
        {
            this.g = g;
            this.gh = gh;
            this.srfc = srfc;
            gh.Antialias = Antialias.Subpixel;
        }

        public void DrawLine(float xB, float yB, float xE, float yE)
        {
            gh.MoveTo(xB, yB);
            gh.LineTo(xE, yE);
            gh.Stroke();
        }

        public void DrawCircle(float x, float y, float d)
        {
            gh.Arc(x + d / 2, y + d / 2, d / 2, 0, Pi2);
            gh.Fill();
        }

        public void SetBaseLinePen()
        {
            gh.SetSourceRGB(BrightnessGraph.BasePenColor[0], BrightnessGraph.BasePenColor[1], BrightnessGraph.BasePenColor[2]);
            gh.LineWidth = 1;
        }

        public void SetCurveLinePen()
        {
            gh.SetSourceRGB(BrightnessGraph.CurvePenColor[0], BrightnessGraph.CurvePenColor[1], BrightnessGraph.CurvePenColor[2]);
            gh.LineWidth = 1;
        }

        public void SetCustomCurveLinePen()
        {
            gh.SetSourceRGB(BrightnessGraph.CustomCurvePenColor[0], BrightnessGraph.CustomCurvePenColor[1], BrightnessGraph.CustomCurvePenColor[2]);
            gh.LineWidth = 1;
        }

        public void SetPointPen()
        {
            gh.SetSourceRGB(BrightnessGraph.PointPenColor[0], BrightnessGraph.PointPenColor[1], BrightnessGraph.PointPenColor[2]);
            gh.LineWidth = 1;
        }

        public void SetSelectedPointPen()
        {
            gh.SetSourceRGB(BrightnessGraph.SelPointPenColor[0], BrightnessGraph.SelPointPenColor[1], BrightnessGraph.SelPointPenColor[2]);
            gh.LineWidth = 1;
        }

        public void ClearGraph()
        {
            gh.SetSourceRGB(BrightnessGraph.GraphBackground[0], BrightnessGraph.GraphBackground[1], BrightnessGraph.GraphBackground[2]);
            gh.Rectangle(0, 0, srfc.Width, srfc.Height);
            gh.Fill();
        }

        public void DrawFull()
        {
            g.SetSourceSurface(srfc, 0, 0);
            g.Paint();
        }

        public void Dispose()
		{
			((IDisposable)gh.Target).Dispose ();
			((IDisposable)gh).Dispose ();
			((IDisposable)g.Target).Dispose ();
			((IDisposable)g).Dispose ();
		}
    }
}

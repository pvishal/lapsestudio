using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Timelapse_UI;

namespace LapseStudioWinFormsUI
{
    public class WinFormGraphDrawer : IGraphDrawer
    {
        private Graphics g;
        private Graphics gh;
        private Bitmap bmp;
        private Pen BasePen;
        private Brush BaseBrush;
        private static Color BaseColor = Color.FromArgb((int)(BrightnessGraph.BasePenColor[0] * 255), (int)(BrightnessGraph.BasePenColor[1] * 255), (int)(BrightnessGraph.BasePenColor[2] * 255));
        private static Color CurveColor = Color.FromArgb((int)(BrightnessGraph.CurvePenColor[0] * 255), (int)(BrightnessGraph.CurvePenColor[1] * 255), (int)(BrightnessGraph.CurvePenColor[2] * 255));
        private static Color CustomCurveColor = Color.FromArgb((int)(BrightnessGraph.CustomCurvePenColor[0] * 255), (int)(BrightnessGraph.CustomCurvePenColor[1] * 255), (int)(BrightnessGraph.CustomCurvePenColor[2] * 255));
        private static Color PointColor = Color.FromArgb((int)(BrightnessGraph.PointPenColor[0] * 255), (int)(BrightnessGraph.PointPenColor[1] * 255), (int)(BrightnessGraph.PointPenColor[2] * 255));
        private static Color SelectedPointColor = Color.FromArgb((int)(BrightnessGraph.SelPointPenColor[0] * 255), (int)(BrightnessGraph.SelPointPenColor[1] * 255), (int)(BrightnessGraph.SelPointPenColor[2] * 255));
        private static Color BackgroundColor = Color.FromArgb((int)(BrightnessGraph.GraphBackground[0] * 255), (int)(BrightnessGraph.GraphBackground[1] * 255), (int)(BrightnessGraph.GraphBackground[2] * 255));

        public WinFormGraphDrawer(Graphics g, Graphics gh, Bitmap bmp)
        {
            this.g = g;
            this.gh = gh;
            this.bmp = bmp;
            gh.SmoothingMode = SmoothingMode.HighQuality;
        }
        
        public void DrawLine(float xB, float yB, float xE, float yE)
        {
            gh.DrawLine(BasePen, xB, yB, xE, yE);
        }

        public void DrawCircle(float x, float y, float d)
        {
            gh.FillEllipse(BaseBrush, x, y, d, d);
        }

        public void SetBaseLinePen()
        {
            BasePen = new Pen(BaseColor);
            BasePen.Width = 1f;
        }

        public void SetCurveLinePen()
        {
            BasePen = new Pen(CurveColor);
            BasePen.Width = 1f;
        }

        public void SetCustomCurveLinePen()
        {
            BasePen = new Pen(CustomCurveColor);
            BasePen.Width = 1f;
        }

        public void SetPointPen()
        {
            BaseBrush = new SolidBrush(PointColor);
        }

        public void SetSelectedPointPen()
        {
            BaseBrush = new SolidBrush(SelectedPointColor);
        }

        public void ClearGraph()
        {
            gh.Clear(BackgroundColor);
        }

        public void DrawFull()
        {
            g.DrawImage(bmp, 0, 0);
        }

        public void Dispose()
        {
            gh.Dispose();
            g.Dispose();
            if (BasePen != null) BasePen.Dispose();
            if (BaseBrush != null) BaseBrush.Dispose();
        }
    }
}

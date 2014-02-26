using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Timelapse_API;
using Timelapse_UI;
using PointD = Timelapse_API.PointD;

namespace LapseStudioWinFormsUI
{
    public partial class Graph : UserControl
    {
        private BrightnessGraph BaseGraph;
        private Pen BasePen = new Pen(Color.Black, 1);
        private Pen CurvePen = new Pen(Color.FromArgb(204, 204, 0), 1);
        private Brush PointBrush = new SolidBrush(Color.FromArgb(204, 0, 0));
        private Brush SelPointBrush = new SolidBrush(Color.FromArgb(0, 204, 204));
        private int lw = 10; //width of the scale lines

        public Graph()
        {
            InitializeComponent();
        }

        public void Init(BrightnessGraph BaseGraph)
        {
            this.BaseGraph = BaseGraph;
            this.BaseGraph.RefreshGraph = Refresh;
            this.BaseGraph.Init();
        }

        #region Events

        private new void Refresh()
        {
            Graph_Paint(this, null);
        }

        private void Graph_Paint(object sender, PaintEventArgs e)
        {
            if (BaseGraph != null)
            {
                using (Graphics g = this.CreateGraphics())
                using (Bitmap bmp = new Bitmap(this.Width, this.Height, PixelFormat.Format24bppRgb))
                using (Graphics gh = Graphics.FromImage(bmp))
                {
                    gh.Clear(Color.DarkGray);
                    gh.SmoothingMode = SmoothingMode.HighQuality;

                    DrawScale(gh);
                    DrawCurve(gh);
                    g.DrawImage(bmp, 0, 0);
                }
            }
        }

        private void Graph_MouseMove(object sender, MouseEventArgs e)
        {
            BaseGraph.Mouse_Move(e.X, e.Y);
        }

        private void Graph_MouseUp(object sender, MouseEventArgs e)
        {
            BaseGraph.Mouse_Up();
        }

        private void Graph_MouseDown(object sender, MouseEventArgs e)
        {
            BaseGraph.Mouse_Down(e.X, e.Y);
        }

        private void Graph_SizeChanged(object sender, EventArgs e)
        {
            if (BaseGraph != null) BaseGraph.SetNewSize(this.Width, this.Height);
        }

        #endregion

        #region Drawing

        private void DrawScale(Graphics g)
        {
            //draw Y axe
            g.DrawLine(BasePen, BrightnessGraph.Left, BrightnessGraph.Top, BrightnessGraph.Left, BaseGraph.Height - BrightnessGraph.Bottom);
            g.DrawLine(BasePen, BaseGraph.Width - BrightnessGraph.Right, BrightnessGraph.Top, BaseGraph.Width - BrightnessGraph.Right, BaseGraph.Height - BrightnessGraph.Bottom + lw);

            //draw X axe
            g.DrawLine(BasePen, BrightnessGraph.Left, BaseGraph.Height - BrightnessGraph.Bottom, BaseGraph.Width - BrightnessGraph.Right, BaseGraph.Height - BrightnessGraph.Bottom);
            g.DrawLine(BasePen, BrightnessGraph.Left - lw, BrightnessGraph.Top, BaseGraph.Width - BrightnessGraph.Right, BrightnessGraph.Top);

            double Xs = (BaseGraph.Width - BrightnessGraph.Left - BrightnessGraph.Right) / 10d;
            double Ys = (BaseGraph.Height - BrightnessGraph.Top - BrightnessGraph.Bottom) / 10d;

            //draw X and Y scales
            for (int i = 1; i < 10; i++)
            {
                //Y scale
                g.DrawLine(BasePen, BrightnessGraph.Left - lw, (int)(BaseGraph.Height - BrightnessGraph.Bottom - i * Ys), BaseGraph.Width - BrightnessGraph.Right, (int)(BaseGraph.Height - BrightnessGraph.Bottom - i * Ys));

                //X scale
                g.DrawLine(BasePen, (int)(BrightnessGraph.Left + i * Xs), BrightnessGraph.Top, (int)(BrightnessGraph.Left + i * Xs), BaseGraph.Height - BrightnessGraph.Bottom + lw);
            }
        }

        private void DrawCurve(Graphics g)
        {
            PointD[] pout;

            #region Drawing Brightness Curve

            if (ProjectManager.CurrentProject.Frames.Count > 0)
            {
                List<PointD> BrP = new List<PointD>();
                for (int i = 0; i < ProjectManager.CurrentProject.Frames.Count; i++)
                {
                    BrP.Add(new PointD(i, (float)ProjectManager.CurrentProject.Frames[i].AlternativeBrightness));
                }

                pout = BaseGraph.RealToGraph(BrP);

                for (int i = 1; i < pout.Length; i++) g.DrawLine(CurvePen, (float)pout[i - 1].X, (float)pout[i - 1].Y, (float)pout[i].X, (float)pout[i].Y);
            }

            #endregion

            if (BaseGraph.Points.Count > 0 && ProjectManager.CurrentProject.IsBrightnessCalculated)
            {
                #region Drawing Custom Curve

                pout = BaseGraph.RealToGraph(Interpolation.Do(BaseGraph.Points.ToArray(), BrightnessGraph.Smoothness));

                for (int i = 1; i < pout.Length; i++) g.DrawLine(BasePen, (float)pout[i - 1].X, (float)pout[i - 1].Y, (float)pout[i].X, (float)pout[i].Y);

                #endregion

                #region Drawing Points

                for (int i = 0; i < BaseGraph.Points.Count; i++)
                {
                    PointD tmp = BaseGraph.RealToGraph(BaseGraph.Points[i]);
                    g.FillEllipse(PointBrush, (float)tmp.X - 3f, (float)tmp.Y - 3f, 6f, 6f);
                }
                PointD tmpSel = BaseGraph.RealToGraph(BaseGraph.Points[BaseGraph.SelectedPoint]);
                g.FillEllipse(SelPointBrush, (float)tmpSel.X - 3f, (float)tmpSel.Y - 3f, 6f, 6f);
                #endregion
            }
        }

        #endregion
    }
}
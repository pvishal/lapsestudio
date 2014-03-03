using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Timelapse_UI;

namespace LapseStudioWinFormsUI
{
    public partial class Graph : UserControl
    {
        private BrightnessGraph BaseGraph;

        public Graph()
        {
            InitializeComponent();
        }

        public void Init(BrightnessGraph BaseGraph)
        {
            this.BaseGraph = BaseGraph;
            this.BaseGraph.RefreshGraph = RefreshGraph;
            this.BaseGraph.Init();
        }

        #region Events

        private void RefreshGraph()
        {
            Graph_Paint(null, null);
        }

        private void Graph_Paint(object sender, PaintEventArgs e)
        {
            if (BaseGraph != null)
            {
                using (Bitmap bmp = new Bitmap(this.Width, this.Height, PixelFormat.Format24bppRgb))
                using (WinFormGraphDrawer drawer = new WinFormGraphDrawer(MainPanel.CreateGraphics(), Graphics.FromImage(bmp), bmp))
                {
                    BaseGraph.DrawFullGraph(drawer);
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
    }
}
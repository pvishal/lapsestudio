using Gtk;
using Cairo;
using Timelapse_UI;

namespace LapseStudioGtkUI
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class Graph : Bin
    {
		private BrightnessGraph BaseGraph;

        #region Init

        public Graph()
        {
            this.Build();
        }

		public void Init(BrightnessGraph BaseGraph)
        {
			this.BaseGraph = BaseGraph;
			this.BaseGraph.RefreshGraph = Refresh;
			this.BaseGraph.Init();
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
            BaseGraph.Dispose();
        }

        #region Events
        
		private void Refresh()
		{
			Application.Invoke(delegate { GraphEventBox_ExposeEvent(GraphView, null); });
		}

        protected void GraphEventBox_ExposeEvent(object o, ExposeEventArgs args)
		{
            using(ImageSurface tmpSurface = new ImageSurface(Format.Rgb24, ((DrawingArea)o).Allocation.Width, ((DrawingArea)o).Allocation.Height))
            using(GtkGraphDrawer drawer = new GtkGraphDrawer(Gdk.CairoHelper.Create(((DrawingArea)o).GdkWindow), new Context(tmpSurface), tmpSurface))
            {
                BaseGraph.DrawFullGraph(drawer);
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
    }
}


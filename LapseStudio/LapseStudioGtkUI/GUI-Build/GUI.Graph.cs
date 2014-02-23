using Gtk;
using Stetic;

namespace LapseStudioGtkUI
{
    public partial class Graph
    {
        private EventBox GraphEventBox;
        private DrawingArea GraphView;

        protected virtual void Build()
        {
            Gui.Initialize(this);

            //Main
            BinContainer.Attach(this);
            Name = "LapseStudioGUI.Graph";

            //GraphEventBox
            GraphEventBox = new EventBox();
            GraphEventBox.Name = "GraphEventBox";

            //GraphView
            GraphView = new DrawingArea();
            GraphView.Name = "GraphView";
            GraphEventBox.Add(GraphView);
            Add(GraphEventBox);

            //Final
            if ((Child != null)) { Child.ShowAll(); }
            Hide();

            //Events
            GraphEventBox.ExposeEvent += GraphEventBox_ExposeEvent;
            GraphEventBox.SizeAllocated += GraphEventBox_SizeAllocated;
            GraphEventBox.ButtonPressEvent += OnGraphEventBoxButtonPressEvent;
            GraphEventBox.ButtonReleaseEvent += OnGraphEventBoxButtonReleaseEvent;
            GraphEventBox.MotionNotifyEvent += OnGraphEventBoxMotionNotifyEvent;
        }
    }
}

using System;

namespace Timelapse_UI
{
    public interface IGraphDrawer : IDisposable
    {
        /// <summary>
        /// Draws a line from beginning point to end point
        /// </summary>
        /// <param name="xB">Beginning point X-coordinate</param>
        /// <param name="yB">Beginning point Y-coordinate</param>
        /// <param name="xE">End point X-coordinate</param>
        /// <param name="yE">End point Y-coordinate</param>
        void DrawLine(float xB, float yB, float xE, float yE);
        /// <summary>
        /// Draws a circle
        /// </summary>
        /// <param name="x">X-coordinate of center</param>
        /// <param name="y">Y-coordinate of center</param>
        /// <param name="d">Diameter of circle</param>
        void DrawCircle(float x, float y, float d);
        /// <summary>
        /// Set color and line width for general lines
        /// </summary>
        void SetBaseLinePen();
        /// <summary>
        /// Set color and line width for curve line
        /// </summary>
        void SetCurveLinePen();
        /// <summary>
        /// Set color and line width for custom curve line
        /// </summary>
        void SetCustomCurveLinePen();
        /// <summary>
        /// Set color for drawing points
        /// </summary>
        void SetPointPen();
        /// <summary>
        /// Set color for drawing selected point
        /// </summary>
        void SetSelectedPointPen();
        /// <summary>
        /// Clear the graph with given color
        /// </summary>
        /// <param name="R">Red value: 0.0 - 1.0</param>
        /// <param name="G">Green value: 0.0 - 1.0</param>
        /// <param name="B">Blue value: 0.0 - 1.0</param>
        void ClearGraph();
        /// <summary>
        /// Draw all parts together
        /// </summary>
        void DrawFull();
    }
}

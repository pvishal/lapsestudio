using System;

namespace Timelapse_API
{
    [Serializable]
    public struct PointD
    {
        public double X { get { return _X; } set { _X = value; } }
        public double Y { get { return _Y; } set { _Y = value; } }

        private double _X;
        private double _Y;

        public PointD(double x, double y)
        {
            _X = x;
            _Y = y;
        }
    }
}

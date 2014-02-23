using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace Timelapse_API
{
    [Serializable()]
    internal abstract class PP3Curve
    {
        public static PP3Curve GetCurve(string rawdata)
        {
            string[] vals = rawdata.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            CultureInfo culture = new CultureInfo("en-US");

            switch (vals.Length)
            {
                case 1:
                    return new PP3Curve_Linear();

                case 2:
                case 3:
                case 4:
                    PP3Curve_Array Curve_Arr = new PP3Curve_Array();
                    for (int i = 0; i < vals.Length; i++) { Curve_Arr.DataPoints.Add(Convert.ToInt32(vals[i])); }
                    return Curve_Arr;

                default:
                    switch (vals[0])
                    {
                        case "1":
                            PP3Curve_Custom Curve_Cus = new PP3Curve_Custom();
                            for (int i = 1; i < vals.Length; i += 2)
                            {
                                float x = (float)Convert.ToDouble(vals[i], culture);
                                float y = (float)Convert.ToDouble(vals[i + 1], culture);
                                Curve_Cus.DataPoints.Add(new PointD(x, y));
                            }
                            return Curve_Cus;

                        case "2":
                            PP3Curve_Parametric Curve_Par = new PP3Curve_Parametric();
                            Curve_Par.Zones[0] = Convert.ToDouble(vals[1], culture);
                            Curve_Par.Zones[1] = Convert.ToDouble(vals[2], culture);
                            Curve_Par.Zones[2] = Convert.ToDouble(vals[3], culture);
                            Curve_Par.Highlights = Convert.ToDouble(vals[4], culture);
                            Curve_Par.Lights = Convert.ToDouble(vals[5], culture);
                            Curve_Par.Darks = Convert.ToDouble(vals[6], culture);
                            Curve_Par.Shadows = Convert.ToDouble(vals[7], culture);                             
                            return Curve_Par;

                        case "3":
                            PP3Curve_ControlCage Curve_Con = new PP3Curve_ControlCage();
                            for (int i = 1; i < vals.Length; i += 2)
                            {
                                float x = (float)Convert.ToDouble(vals[i], culture);
                                float y = (float)Convert.ToDouble(vals[i + 1], culture);
                                Curve_Con.DataPoints.Add(new PointD(x, y));
                            }
                            return Curve_Con;

                        default:
                            return null;
                    }
            }
        }

        protected PP3Curve() { }

        public abstract override string ToString();
    }

    [Serializable()]
    internal class PP3Curve_Array : PP3Curve
    {
        public List<int> DataPoints
        {
            get { return _DataPoints; }
            internal set { _DataPoints = value; }
        }
        private List<int> _DataPoints;

        public PP3Curve_Array() { _DataPoints = new List<int>(); }

        public override string ToString()
        {
            string output = String.Empty;
            for (int i = 0; i < DataPoints.Count; i++) { output += DataPoints[i] + ";"; }
            return output;
        }
    }

    [Serializable()]
    internal class PP3Curve_Linear : PP3Curve
    {
        public override string ToString()
        {
            return "0;";
        }
    }

    [Serializable()]
    internal class PP3Curve_HSV : PP3Curve
    {
        public List<KeyValuePair<PointD, PointD>> DataPoints
        {
            get { return _DataPoints; }
            internal set { _DataPoints = value; }
        }
        private List<KeyValuePair<PointD, PointD>> _DataPoints;

        public PP3Curve_HSV(string rawdata)
        {
            string[] vals = rawdata.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            CultureInfo culture = new CultureInfo("en-US");
            _DataPoints = new List<KeyValuePair<PointD, PointD>>();

            if (vals.Length > 1)
            {
                for (int i = 1; i < vals.Length; i += 4)
                {
                    float x1 = (float)Convert.ToDouble(vals[i], culture);
                    float y1 = (float)Convert.ToDouble(vals[i + 1], culture);
                    float x2 = (float)Convert.ToDouble(vals[i + 2], culture);
                    float y2 = (float)Convert.ToDouble(vals[i + 3], culture);
                    DataPoints.Add(new KeyValuePair<PointD, PointD>(new PointD(x1, y1), new PointD(x2, y2)));
                }
            }
        }

        public override string ToString()
        {
            if (DataPoints.Count > 1)
            {
                CultureInfo culture = new CultureInfo("en-US");
                string digits = "r16";
                string output = "1;";
                for (int i = 0; i < DataPoints.Count; i++)
                {
                    output += ((double)DataPoints[i].Key.X).ToString(digits, culture) + ";" + ((double)DataPoints[i].Key.Y).ToString(digits, culture) + ";" +
                        ((double)DataPoints[i].Value.X).ToString(digits, culture) + ";" + ((double)DataPoints[i].Value.Y).ToString(digits, culture) + ";";
                }
                return output;
            }
            else { return "0;"; }
        }
    }

    [Serializable()]
    internal class PP3Curve_Custom : PP3Curve
    {
        public List<PointD> DataPoints
        {
            get { return _DataPoints; }
            internal set { _DataPoints = value; }
        }
        private List<PointD> _DataPoints;

        public PP3Curve_Custom() { _DataPoints = new List<PointD>(); }

        public override string ToString()
        {
            CultureInfo culture = new CultureInfo("en-US");
            string digits = "r16";
            string output = "1;";
            for (int i = 0; i < DataPoints.Count; i++) { output += ((double)DataPoints[i].X).ToString(digits, culture) + ";" + ((double)DataPoints[i].Y).ToString(digits, culture) + ";"; }

            return output;
        }
    }

    [Serializable()]
    internal class PP3Curve_Parametric : PP3Curve
    {
        public double[] Zones
        {
            get { return _Zones; }
            internal set { _Zones = value; }
        }           //3 values from 0.0 to 1.0
        public double Highlights
        {
            get { return _Highlights; }
            internal set { _Highlights = value; }
        }        //-100 to 100
        public double Lights
        {
            get { return _Lights; }
            internal set { _Lights = value; }
        }            //-100 to 100
        public double Darks
        {
            get { return _Darks; }
            internal set { _Darks = value; }
        }             //-100 to 100
        public double Shadows
        {
            get { return _Shadows; }
            internal set { _Shadows = value; }
        }           //-100 to 100

        private double[] _Zones;
        private double _Highlights;
        private double _Lights;
        private double _Darks;
        private double _Shadows;

        public PP3Curve_Parametric() { _Zones = new double[3]; }

        public override string ToString()
        {
            CultureInfo culture = new CultureInfo("en-US");
            string digits = "r16";
            string output = "2;";
            for (int i = 0; i < Zones.Length; i++) { output += Zones[i].ToString(digits, culture) + ";"; }
            output += Highlights.ToString(digits, culture) + ";";
            output += Lights.ToString(digits, culture) + ";";
            output += Darks.ToString(digits, culture) + ";";
            output += Shadows.ToString(digits, culture) + ";";

            return output;
        }
    }

    [Serializable()]
    internal class PP3Curve_ControlCage : PP3Curve
    {
        public List<PointD> DataPoints
        {
            get { return _DataPoints; }
            internal set { _DataPoints = value; }
        }
        private List<PointD> _DataPoints;

        public PP3Curve_ControlCage() { _DataPoints = new List<PointD>(); }

        public override string ToString()
        {
            CultureInfo culture = new CultureInfo("en-US");
            string digits = "r16";
            string output = "3;";
            for (int i = 0; i < DataPoints.Count; i++) { output += ((double)DataPoints[i].X).ToString(digits, culture) + ";" + ((double)DataPoints[i].Y).ToString(digits, culture) + ";"; }

            return output;
        }
    }
}

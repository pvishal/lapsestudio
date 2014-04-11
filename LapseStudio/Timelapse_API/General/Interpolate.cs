using System;
using System.Collections.Generic;
using System.Linq;

namespace Timelapse_API
{
    public static class Interpolation
    {
        /// <summary>
        /// Interpolates between x and y points
        /// </summary>
        /// <param name="x">X coordinate of points</param>
        /// <param name="y">Y coordinate of points </param>
        /// <param name="xs">X coordinats of output. has to have output length</param>
        /// <returns>a double array with Y coordinates</returns>
        public static double[] Do(double[] x, double[] y, double[] xs)
        {
            if (x == null || y == null || xs == null) { throw new ArgumentNullException(); }
            if (xs.Length <= x.Length) { throw new ArgumentException("xs has to be longer than x/y"); }

            CubicSpline Spline = new CubicSpline();
            return Spline.FitAndEval(x, y, xs);
        }

        /// <summary>
        /// Interpolates between x and y points
        /// </summary>
        /// <param name="x">X coordinate of points</param>
        /// <param name="y">Y coordinate of points </param>
        /// <param name="length">length of output point array</param>
        /// <returns>a point array with interpolated points</returns>
        public static double[] Do(double[] x, double[] y, int length)
        {
            if (x == null || y == null) { throw new ArgumentNullException(); }

            CubicSpline Spline = new CubicSpline();
            //PointD[] Output = new PointD[length];
            double[] xs = new double[length];
            //double[] ys;

            double d = Math.Abs(x[0] - x[x.Length - 1]);
            for (int i = 0; i < length; i++) { xs[i] = i * d / (double)(length - 1); }

            return Spline.FitAndEval(x, y, xs);

            //for (int i = 0; i < length; i++) { Output[i] = new PointD((float)xs[i], (float)ys[i]); }
            //return Output;
        }

        /// <summary>
        /// Interpolates between a set of points
        /// </summary>
        /// <param name="points">a set of points to interpolate</param>
        /// <param name="length">length of output point array</param>
        /// <returns>a point array with interpolated points</returns>
        public static PointD[] Do(PointD[] points, int length)
        {
            if (points == null) { throw new ArgumentNullException(); }

            CubicSpline Spline = new CubicSpline();
            PointD[] Output = new PointD[length];
            double[] xs = new double[length];
            double[] x = new double[points.Length];
            double[] y = new double[points.Length];
            double[] ys;

            double d = Math.Abs(points[0].X - points[points.Length - 1].X);
            for (int i = 0; i < length; i++) { xs[i] = i * d / (double)(length - 1); }
            for (int i = 0; i < points.Length; i++) { x[i] = points[i].X; y[i] = points[i].Y; }

            ys = Spline.FitAndEval(x, y, xs);

            for (int i = 0; i < length; i++) { Output[i] = new PointD((float)xs[i], (float)ys[i]); }
            return Output;
        }

        internal static PP3[] Do(ProjectRT CurProj)
        {
            PP3[] Output = new PP3[CurProj.Frames.Count];
            PP3 typepp3 = ((FrameRT)CurProj.Frames.First(t => t.IsKeyframe)).PP3File;

            if (CurProj.KeyframeCount == 1)
            {
                for (int i = 0; i < CurProj.Frames.Count; i++) { Output[i] = typepp3.Copy(); }
                return Output;
            }

            for (int j = 0; j < CurProj.Frames.Count; j++) { Output[j] = typepp3.Copy(); }

            for (int i = 0; i < typepp3.Values.Count; i++)
            {
                List<KeyValuePair<int, object>> Values = new List<KeyValuePair<int, object>>();
                Type CurType = typepp3.Values.ElementAt(i).Value.Value.GetType();
                string valname = typepp3.Values.ElementAt(i).Key;

                for (int j = 0; j < CurProj.Frames.Count; j++)
                {
                    if (CurProj.Frames[j].IsKeyframe) { Values.Add(new KeyValuePair<int, object>(j, ((FrameRT)CurProj.Frames[j]).PP3File.Values[valname].Value)); }
                    else if (j == 0) { Values.Add(new KeyValuePair<int, object>(j, ((FrameRT)CurProj.Frames.First(t => t.IsKeyframe)).PP3File.Values[valname].Value)); }
                    else if (j == CurProj.Frames.Count - 1) { Values.Add(new KeyValuePair<int, object>(j, ((FrameRT)CurProj.Frames.Last(t => t.IsKeyframe)).PP3File.Values[valname].Value)); }
                }

                if (CurType != typeof(bool) && CurType != typeof(string))
                {
                    PointD[] InVals = new PointD[Values.Count];
                    PointD[] OutVals = new PointD[CurProj.Frames.Count];

                    if (CurType == typeof(int))
                    {
                        for (int k = 0; k < Values.Count; k++) { InVals[k] = new PointD(Values[k].Key, (int)Convert.ChangeType(Values[k].Value, typeof(int))); }
                        OutVals = Do(InVals, CurProj.Frames.Count);
                        for (int k = 0; k < CurProj.Frames.Count; k++)
                        {
                            if (((FrameRT)CurProj.Frames[k]).PP3File != null)
                            {
                                Output[k].Path = ((FrameRT)CurProj.Frames[k]).PP3File.Path;
                                Output[k].Compensation = ((FrameRT)CurProj.Frames[k]).PP3File.Compensation;
                                Output[k].FileVersion = ((FrameRT)CurProj.Frames[k]).PP3File.FileVersion;
                                Output[k].NewCompensation = ((FrameRT)CurProj.Frames[k]).PP3File.NewCompensation;
                            }
                            Output[k].Values[valname] = new PP3.PP3entry(typepp3.Values[valname].Name, (int)OutVals[k].Y, typepp3.Values[valname].min, typepp3.Values[valname].max);
                        }
                    }
                    else if (CurType == typeof(double))
                    {
                        for (int k = 0; k < Values.Count; k++) { InVals[k] = new PointD(Values[k].Key, (float)Convert.ChangeType(Values[k].Value, typeof(float))); }
                        OutVals = Do(InVals, CurProj.Frames.Count);
                        for (int k = 0; k < CurProj.Frames.Count; k++)
                        {
                            if (((FrameRT)CurProj.Frames[k]).PP3File != null)
                            {
                                Output[k].Path = ((FrameRT)CurProj.Frames[k]).PP3File.Path;
                                Output[k].Compensation = ((FrameRT)CurProj.Frames[k]).PP3File.Compensation;
                                Output[k].FileVersion = ((FrameRT)CurProj.Frames[k]).PP3File.FileVersion;
                                Output[k].NewCompensation = ((FrameRT)CurProj.Frames[k]).PP3File.NewCompensation;
                            }
                            Output[k].Values[valname] = new PP3.PP3entry(typepp3.Values[valname].Name, (double)OutVals[k].Y, typepp3.Values[valname].min, typepp3.Values[valname].max);
                        }
                    }
                    else if (CurType.BaseType == typeof(PP3Curve))
                    {
                        if (CurType == typeof(PP3Curve_Linear))
                        {
                            int index = 0;
                            for (int k = 0; k < CurProj.Frames.Count; k++)
                            {
                                if (((FrameRT)CurProj.Frames[k]).PP3File != null)
                                {
                                    Output[k].Path = ((FrameRT)CurProj.Frames[k]).PP3File.Path;
                                    Output[k].Compensation = ((FrameRT)CurProj.Frames[k]).PP3File.Compensation;
                                    Output[k].FileVersion = ((FrameRT)CurProj.Frames[k]).PP3File.FileVersion;
                                    Output[k].NewCompensation = ((FrameRT)CurProj.Frames[k]).PP3File.NewCompensation;
                                }
                                Output[k].Values[valname] = new PP3.PP3entry(typepp3.Values[valname].Name, Values[index].Value, typepp3.Values[valname].min, typepp3.Values[valname].max);
                                if (Values[index].Key == k) { index++; }
                            }
                        }
                        else
                        {
                            PP3Curve[] OutCurves = Do(Values);
                            for (int k = 0; k < CurProj.Frames.Count; k++)
                            {
                                if (((FrameRT)CurProj.Frames[k]).PP3File != null)
                                {
                                    Output[k].Path = ((FrameRT)CurProj.Frames[k]).PP3File.Path;
                                    Output[k].Compensation = ((FrameRT)CurProj.Frames[k]).PP3File.Compensation;
                                    Output[k].FileVersion = ((FrameRT)CurProj.Frames[k]).PP3File.FileVersion;
                                    Output[k].NewCompensation = ((FrameRT)CurProj.Frames[k]).PP3File.NewCompensation;
                                }
                                Output[k].Values[valname] = new PP3.PP3entry(typepp3.Values[valname].Name, OutCurves[k], typepp3.Values[valname].min, typepp3.Values[valname].max);
                            }
                        }
                    }
                    else { throw new InterpolationNotPossibleException("Couldn't interpolate this kind of Value: " + CurType.FullName); }

                }
                else
                {
                    int index = 0;
                    for (int k = 0; k < CurProj.Frames.Count; k++)
                    {
                        Output[k].Values[valname] = new PP3.PP3entry(typepp3.Values[valname].Name, Values[index].Value, typepp3.Values[valname].min, typepp3.Values[valname].max);
                        if (Values[index].Key == k) { index++; }
                    }
                }
            }
            return Output;
        }
        
        internal static PP3Curve[] Do(List<KeyValuePair<int, object>> Input)
        {
            Type CurType = Input[0].Value.GetType();
            int Pointcount, step = 1;

            if (CurType == typeof(PP3Curve_Array))
            {
                Pointcount = ((PP3Curve_Array)Input[0].Value).DataPoints.Count;
                if (Input.Any(t => ((PP3Curve_Array)t.Value).DataPoints.Count != Pointcount)) { throw new InterpolationNotPossibleException("Pointcount of curve is not consistet!"); }
            }
            else if (CurType == typeof(PP3Curve_ControlCage))
            {
                Pointcount = ((PP3Curve_ControlCage)Input[0].Value).DataPoints.Count;
                if (Input.Any(t => ((PP3Curve_ControlCage)t.Value).DataPoints.Count != Pointcount)) { throw new InterpolationNotPossibleException("Pointcount of curve is not consistet!"); }
            }
            else if (CurType == typeof(PP3Curve_Custom))
            {
                step = 2;
                Pointcount = ((PP3Curve_Custom)Input[0].Value).DataPoints.Count * 2;
                if (Input.Any(t => ((PP3Curve_Custom)t.Value).DataPoints.Count * 2 != Pointcount)) { throw new InterpolationNotPossibleException("Pointcount of curve is not consistet!"); }
            }
            else if (CurType == typeof(PP3Curve_Parametric)) { Pointcount = 7; }
            else { throw new InterpolationNotPossibleException(); }
            
            //check if it's possible to interpolate
            if (Input.Any(t => t.Value.GetType() != CurType)) { throw new InterpolationNotPossibleException("Curvetype is not consistet!"); }

            //start to interpolate
            PointD[][] InPoints = new PointD[Pointcount][];

            //Curves to points
            for (int i = 0; i < Pointcount; i += step)
            {
                if (InPoints[i] == null) { InPoints[i] = new PointD[Input.Count]; }

                for (int j = 0; j < Input.Count; j++)
                {
                    if (CurType == typeof(PP3Curve_Array)) { InPoints[i][j] = new PointD(Input[j].Key, ((PP3Curve_Array)Input[j].Value).DataPoints[i]); }
                    else if (CurType == typeof(PP3Curve_ControlCage)) { InPoints[i][j] = ((PP3Curve_ControlCage)Input[j].Value).DataPoints[i]; }
                    else if (CurType == typeof(PP3Curve_Custom))
                    {
                        if (InPoints[i + 1] == null) { InPoints[i + 1] = new PointD[Input.Count]; }
                        InPoints[i][j] = new PointD(j, ((PP3Curve_Custom)Input[j].Value).DataPoints[i / 2].X);
                        InPoints[i + 1][j] = new PointD(j, ((PP3Curve_Custom)Input[j].Value).DataPoints[i / 2].Y);
                    }
                    else if (CurType == typeof(PP3Curve_Parametric))
                    {
                        InPoints[i][0] = new PointD(Input[j].Key, (float)((PP3Curve_Parametric)Input[0].Value).Zones[0]);
                        InPoints[i][1] = new PointD(Input[j].Key, (float)((PP3Curve_Parametric)Input[1].Value).Zones[1]);
                        InPoints[i][2] = new PointD(Input[j].Key, (float)((PP3Curve_Parametric)Input[2].Value).Zones[2]);
                        InPoints[i][3] = new PointD(Input[j].Key, (float)((PP3Curve_Parametric)Input[3].Value).Highlights);
                        InPoints[i][4] = new PointD(Input[j].Key, (float)((PP3Curve_Parametric)Input[4].Value).Lights);
                        InPoints[i][5] = new PointD(Input[j].Key, (float)((PP3Curve_Parametric)Input[5].Value).Darks);
                        InPoints[i][6] = new PointD(Input[j].Key, (float)((PP3Curve_Parametric)Input[6].Value).Shadows);
                    }
                }
            }

            PointD[][] OutPoints = new PointD[Pointcount][];

            //interpolate points
            for (int i = 0; i < Pointcount; i++) { OutPoints[i] = Do(InPoints[i], ProjectManager.CurrentProject.Frames.Count); }

            PP3Curve[] Output = new PP3Curve[ProjectManager.CurrentProject.Frames.Count];

            //points back to curves
            for (int i = 0; i < ProjectManager.CurrentProject.Frames.Count; i++)
            {
                for (int j = 0; j < Pointcount; j += step)
                {
                    if (CurType == typeof(PP3Curve_Array))
                    {
                        if (Output[i] == null) { Output[i] = new PP3Curve_Array(); }
                        ((PP3Curve_Array)Output[i]).DataPoints.Add((int)OutPoints[j][i].Y);
                    }
                    else if (CurType == typeof(PP3Curve_ControlCage))
                    {
                        if (Output[i] == null) { Output[i] = new PP3Curve_ControlCage(); }
                        ((PP3Curve_ControlCage)Output[i]).DataPoints.Add(OutPoints[j][i]);
                    }
                    else if (CurType == typeof(PP3Curve_Custom))
                    {
                        if (Output[i] == null) { Output[i] = new PP3Curve_Custom(); }
                        ((PP3Curve_Custom)Output[i]).DataPoints.Add(new PointD(OutPoints[j][i].Y, OutPoints[j + 1][i].Y));
                    }
                    else if (CurType == typeof(PP3Curve_Parametric))
                    {
                        if (Output[i] == null) { Output[i] = new PP3Curve_Parametric(); }
                        ((PP3Curve_Parametric)Output[i]).Zones[0] = OutPoints[0][i].Y;
                        ((PP3Curve_Parametric)Output[i]).Zones[1] = OutPoints[1][i].Y;
                        ((PP3Curve_Parametric)Output[i]).Zones[2] = OutPoints[2][i].Y;
                        ((PP3Curve_Parametric)Output[i]).Highlights = OutPoints[3][i].Y;
                        ((PP3Curve_Parametric)Output[i]).Lights = OutPoints[4][i].Y;
                        ((PP3Curve_Parametric)Output[i]).Darks = OutPoints[5][i].Y;
                        ((PP3Curve_Parametric)Output[i]).Shadows = OutPoints[6][i].Y;
                        j = Pointcount;
                    }
                    else if (CurType == typeof(PP3Curve_HSV))
                    {
                        //TODO: check for HSV curve type (also in PP3Curve.cs); handling is the same as custom curve
                    }
                }
            }

            return Output;
        }
        
        internal static XMP[] Do(ProjectACR CurProj)
        {
            XMP[] Output = new XMP[CurProj.Frames.Count];
            XMP typexmp = ((FrameACR)CurProj.Frames.First(t => t.IsKeyframe)).XMPFile;
            
            for (int j = 0; j < CurProj.Frames.Count; j++) { Output[j] = typexmp.Copy(); }

            for (int i = 0; i < typexmp.Values.Count; i++)
            {
                List<KeyValuePair<int, object>> Values = new List<KeyValuePair<int, object>>();
                Type CurType = typexmp.Values.ElementAt(i).Value.type;
                string valname = typexmp.Values.ElementAt(i).Key;
                
                for (int j = 0; j < CurProj.Frames.Count; j++)
                {
                    if (CurProj.Frames[j].IsKeyframe) { Values.Add(new KeyValuePair<int, object>(j, ((FrameACR)CurProj.Frames[j]).XMPFile.Values[valname].Value)); }
                    else if (j == 0) { Values.Add(new KeyValuePair<int, object>(j, ((FrameACR)CurProj.Frames.First(t => t.IsKeyframe)).XMPFile.Values[valname].Value)); }
                    else if (j == CurProj.Frames.Count - 1) { Values.Add(new KeyValuePair<int, object>(j, ((FrameACR)CurProj.Frames.Last(t => t.IsKeyframe)).XMPFile.Values[valname].Value)); }
                }

                if (CurType != typeof(bool) && CurType != typeof(string))
                {
                    PointD[] InVals = new PointD[Values.Count];
                    PointD[] OutVals = new PointD[CurProj.Frames.Count];

                    if (CurType == typeof(int))
                    {
                        for (int k = 0; k < Values.Count; k++) { InVals[k] = new PointD(Values[k].Key, (int)Convert.ChangeType(Values[k].Value, typeof(int))); }
                        OutVals = Do(InVals, CurProj.Frames.Count);
                        for (int k = 0; k < CurProj.Frames.Count; k++)
                        {
                            if (((FrameACR)CurProj.Frames[k]).XMPFile != null)
                            {
                                Output[k].Path = ((FrameACR)CurProj.Frames[k]).XMPFile.Path;
                                Output[k].Exposure = ((FrameACR)CurProj.Frames[k]).XMPFile.Exposure;
                                Output[k].FileVersion = ((FrameACR)CurProj.Frames[k]).XMPFile.FileVersion;
                                Output[k].NewExposure = ((FrameACR)CurProj.Frames[k]).XMPFile.NewExposure;
                            }
                            Output[k].Values[valname] = new XMP.XMPentry(typexmp.Values[valname].Name, (int)OutVals[k].Y, typeof(int), typexmp.Values[valname].sign, typexmp.Values[valname].min, typexmp.Values[valname].max);
                        }
                    }
                    else if (CurType == typeof(double))
                    {
                        for (int k = 0; k < Values.Count; k++) { InVals[k] = new PointD(Values[k].Key, (float)Convert.ChangeType(Values[k].Value, typeof(float))); }
                        OutVals = Do(InVals, CurProj.Frames.Count);
                        for (int k = 0; k < CurProj.Frames.Count; k++)
                        {
                            if (((FrameACR)CurProj.Frames[k]).XMPFile != null)
                            {
                                Output[k].Path = ((FrameACR)CurProj.Frames[k]).XMPFile.Path;
                                Output[k].Exposure = ((FrameACR)CurProj.Frames[k]).XMPFile.Exposure;
                                Output[k].FileVersion = ((FrameACR)CurProj.Frames[k]).XMPFile.FileVersion;
                                Output[k].NewExposure = ((FrameACR)CurProj.Frames[k]).XMPFile.NewExposure;
                            }
                            Output[k].Values[valname] = new XMP.XMPentry(typexmp.Values[valname].Name, OutVals[k].Y, typeof(double), typexmp.Values[valname].sign, typexmp.Values[valname].min, typexmp.Values[valname].max);
                        }
                    }
                    else { throw new InterpolationNotPossibleException("Couldn't interpolate this kind of Value: " + CurType.FullName); }

                }
                else
                {
                    int index = 0;
                    for (int k = 0; k < CurProj.Frames.Count; k++)
                    {
                        if (((FrameACR)CurProj.Frames[k]).XMPFile != null)
                        {
                            Output[k].Path = ((FrameACR)CurProj.Frames[k]).XMPFile.Path;
                            Output[k].Exposure = ((FrameACR)CurProj.Frames[k]).XMPFile.Exposure;
                            Output[k].FileVersion = ((FrameACR)CurProj.Frames[k]).XMPFile.FileVersion;
                            Output[k].NewExposure = ((FrameACR)CurProj.Frames[k]).XMPFile.NewExposure;
                        }
                        Output[k].Values[valname] = new XMP.XMPentry(typexmp.Values[valname].Name, Values[index].Value, CurType, typexmp.Values[valname].sign, typexmp.Values[valname].min, typexmp.Values[valname].max);
                        if (Values[index].Key == k) { index++; }
                    }
                }
            }
            return Output;
        }
    }
}

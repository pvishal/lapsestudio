using System;
using System.Collections.Generic;
using System.Globalization;

namespace Timelapse_API
{    
    /// <summary>
    /// Stores and handles all data from a XMP file
    /// </summary>
    [Serializable()]
    public class XMP
    {
        /// <summary>
        /// Path to the orignal file (may be an image too)
        /// </summary>
        public string Path
        {
            get { return _Path; }
            internal set { _Path = value; }
        }
        /// <summary>
        /// The file version of this XMP
        /// </summary>
        public string FileVersion
        {
            get { return _FileVersion; }
            internal set { _FileVersion = value; }
        }
        /// <summary>
        /// The exposure value of this XMP
        /// </summary>
        public double Exposure
        {
            get { return _Exposure; }
            internal set { _Exposure = value; }
        }
        /// <summary>
        /// The newly calculated exposure value of this XMP
        /// </summary>
        public double NewExposure
        {
            get { return _NewExposure; }
            internal set { _NewExposure = value; }
        }
        /// <summary>
        /// List of all XMP values
        /// </summary>
        public Dictionary<string, XMPentry> Values
        {
            get { return _Values; }
            internal set { _Values = value; }
        }

        private string _Path;
        private string _FileVersion;
        private double _Exposure;
        private double _NewExposure;
        private Dictionary<string, XMPentry> _Values;

        /// <summary>
        /// Initiates a new and empty XMP file
        /// </summary>
        public XMP()
        {
            Values = new Dictionary<string, XMPentry>();
        }

        /// <summary>
        /// Initiates and reads a new XMP file
        /// </summary>
        /// <param name="Path">Path to the XMP file</param>
        public XMP(string path)
        {
            Values = new Dictionary<string, XMPentry>();
            Path = path;
            Read();
        }

        /// <summary>
        /// Write all values to disk
        /// </summary>
        public void Write()
        {
            Exiftool.WriteXMPMetadata(Path, this);
        }

        /// <summary>
        /// Copies all values
        /// </summary>
        /// <returns>a new XMP with values duplicated from this XMP</returns>
        public XMP Copy()
        {
            XMP output = new XMP();
            output.Path = this.Path;
            output.FileVersion = this.FileVersion;
            output.Exposure = this.Exposure;
            output.NewExposure = this.NewExposure;
            output.Values = new Dictionary<string, XMPentry>(this.Values);
            return output;
        }
        
        internal void Read()
        {
            string[] lines = Exiftool.GetXMPMetadata(Path);
            if (lines == null) { return; }
            Values.Clear();
            CultureInfo culture = new CultureInfo("en-US");

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
                string name = lines[i].Substring(0, lines[i].IndexOf(':')).Trim();
                string value = lines[i].Substring(lines[i].IndexOf(':') + 1).Replace("\"", "").Trim();

                if (name == "RawFileName") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "Version") { Values.Add(name, new XMPentry(name, Convert.ToDouble(value, culture), typeof(double), false, 7.4, 7.4)); }
                else if (name == "ProcessVersion") { FileVersion = value; }
                else if (name == "WhiteBalance") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "Temperature") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 2000, 50000)); }
                else if (name == "Tint") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), true, -150, 150)); }
                else if (name == "Saturation") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "Sharpness") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 150)); }
                else if (name == "LuminanceSmoothing") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "ColorNoiseReduction") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "VignetteAmount") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "ShadowTint") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "RedHue") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "RedSaturation") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "GreenHue") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "GreenSaturation") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "BlueHue") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "BlueSaturation") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "Vibrance") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "HueAdjustmentRed") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "HueAdjustmentOrange") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "HueAdjustmentYellow") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "HueAdjustmentGreen") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "HueAdjustmentAqua") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "HueAdjustmentBlue") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "HueAdjustmentPurple") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "HueAdjustmentMagenta") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "SaturationAdjustmentRed") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "SaturationAdjustmentOrange") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "SaturationAdjustmentYellow") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "SaturationAdjustmentGreen") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "SaturationAdjustmentAqua") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "SaturationAdjustmentBlue") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "SaturationAdjustmentPurple") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "SaturationAdjustmentMagenta") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "LuminanceAdjustmentRed") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "LuminanceAdjustmentOrange") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "LuminanceAdjustmentYellow") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "LuminanceAdjustmentGreen") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "LuminanceAdjustmentAqua") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "LuminanceAdjustmentBlue") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "LuminanceAdjustmentPurple") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "LuminanceAdjustmentMagenta") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "GrayMixerRed") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), true, -100, 100)); }
                else if (name == "GrayMixerOrange") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), true, 0, 360)); }
                else if (name == "GrayMixerYellow") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), true, 0, 100)); }
                else if (name == "GrayMixerGreen") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), true, 0, 360)); }
                else if (name == "GrayMixerAqua") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), true, 0, 100)); }
                else if (name == "GrayMixerBlue") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), true, -100, 100)); }
                else if (name == "GrayMixerPurple") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), true, 0, 0)); }
                else if (name == "GrayMixerMagenta") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), true, 0, 0)); }
                else if (name == "SplitToningShadowHue") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 0)); }
                else if (name == "SplitToningShadowSaturation") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 0)); }
                else if (name == "SplitToningHighlightHue") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 25, 25)); }
                else if (name == "SplitToningHighlightSaturation") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 50, 50)); }
                else if (name == "SplitToningBalance") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 75, 75)); }
                else if (name == "ParametricShadows") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "ParametricDarks") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "ParametricLights") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "ParametricHighlights") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "ParametricShadowSplit") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "ParametricMidtoneSplit") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "ParametricHighlightSplit") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "SharpenRadius") { Values.Add(name, new XMPentry(name, Convert.ToDouble(value, culture), typeof(double), true, 1, 1)); }
                else if (name == "SharpenDetail") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "SharpenEdgeMasking") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "PostCropVignetteAmount") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 1, 100)); }
                else if (name == "GrainAmount") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "ColorNoiseReductionDetail") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "LensProfileEnable") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "LensManualDistortionAmount") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "PerspectiveVertical") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 50, 100)); }
                else if (name == "PerspectiveHorizontal") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "PerspectiveRotate") { Values.Add(name, new XMPentry(name, Convert.ToDouble(value, culture), typeof(double), false, -5, 100)); }
                else if (name == "PerspectiveScale") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "AutoLateralCA") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 150)); }
                else if (name == "Exposure2012") { Exposure = Convert.ToDouble(value, culture); }
                else if (name == "Contrast2012") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "Highlights2012") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "Shadows2012") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, -100, 100)); }
                else if (name == "Whites2012") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "Blacks2012") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "Clarity2012") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 10, 100)); }
                else if (name == "DefringePurpleAmount") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "DefringePurpleHueLo") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 20)); }
                else if (name == "DefringePurpleHueHi") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 10, 90)); }
                else if (name == "DefringeGreenAmount") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "DefringeGreenHueLo") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "DefringeGreenHueHi") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "ConvertToGrayscale") { Values.Add(name, new XMPentry(name, Convert.ToBoolean(value), typeof(bool), false, false, true)); }
                else if (name == "ToneCurveName2012") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "CameraProfile") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "CameraProfileDigest") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "LensProfileSetup") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "LensProfileName") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "LensProfileFilename") { Values.Add(name, new XMPentry(name, value, typeof(string), true, String.Empty, String.Empty)); }
                else if (name == "LensProfileDigest") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "LensProfileDistortionScale") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "LensProfileChromaticAberrationScale") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "LensProfileVignettingScale") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }
                else if (name == "AlreadyApplied") { Values.Add(name, new XMPentry(name, Convert.ToBoolean(value), typeof(bool), false, false, true)); }
                else if (name == "Shadows") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), false, 0, 100)); }
                else if (name == "Brightness") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), true, -150, 150)); }
                else if (name == "Contrast") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), true, -50, 100)); }
                else if (name == "FillLight") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), true, -100, 100)); }
                else if (name == "HighlightRecovery") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), true, -100, 100)); }
            }

            NewExposure = Exposure;
        }
        
        /// <summary>
        /// Stores data for a XMP value
        /// </summary>
        [Serializable()]
        public class XMPentry
        {
            public string Name
            {
                get { return _Name; }
                private set { _Name = value; }
            }
            public object Value;
            public Type type;
            public object min
            {
                get { return _min; }
                private set { _min = value; }
            }
            public object max
            {
                get { return _max; }
                private set { _max = value; }
            }
            public bool sign
            {
                get { return _sign; }
                private set { _sign = value; }
            }

            private string _Name;
            private object _min;
            private object _max;
            private bool _sign;

            public XMPentry(string Name, object Value, Type type, bool sign, object min, object max)
            {
                this.Name = Name;
                this.Value = Value;
                this.type = type;
                this.min = min;
                this.max = max;
                this.sign = sign;
            }
        }

    }
}

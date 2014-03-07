using System;
using System.Linq;
using System.Collections.Generic;

namespace Timelapse_API
{
    /// <summary>
    /// A project for use with CameraRaw
    /// </summary>
    [Serializable()]
    public class ProjectACR : Project
	{
		/// <summary>
		/// Type of project
		/// </summary>
		public override ProjectType Type { get { return ProjectType.CameraRaw; } }

        /// <summary>
        /// States if a frame of this project has a XMP file
        /// </summary>
        public bool HasXMP
        {
            get { return this.Frames.Any(t => ((FrameACR)t).XMPFile != null); }
        }
        /// <summary>
        /// The version of the used XMP file(s) in this project
        /// </summary>
        public string XMPVersion
        {
            get
            {
                if (this.Frames.Count > 0 && HasXMP) { return ((FrameACR)this.Frames.First(t => ((FrameACR)t).XMPFile != null)).XMPFile.FileVersion; }
                else { return null; }
            }
        }
        /// <summary>
        /// The number of XMP's that are set as a keyframe
        /// </summary>
        public int XMPKFcount
        {
            get
            {
                return this.Frames.Count(t => ((FrameACR)t).XMPFile != null && t.IsKeyframe);
            }
        }
        /// <summary>
        /// File extensions supported by the project. (e.g. ".jpg")
        /// </summary>
        public override string[] AllowedFileExtensions
        {
            get { return extensions; }
        }

        private static readonly string[] extensions = { ".jpg", ".jpeg", ".png", ".tif", ".tiff",".dng", ".cr2", ".crw", ".x3f", ".nef", ".srw", ".srf", ".sr2", ".arw",
                ".erf", ".pef", ".raf", ".3fr", ".fff", ".dcr", ".dcs", ".kdc", ".kdc", ".rwl", ".mrw", ".mdc", ".nrw",".orf", ".rw2" };


        /// <summary>
        /// Add a XMP keyframe
        /// </summary>
        /// <param name="path">Path to the XMP file</param>
        /// <param name="index">Index where the keyframe is set</param>
        internal override void AddKeyframe(int index, string path)
        {
            if (index < 0 && index >= Frames.Count) { throw new ArgumentException("Index too high or low"); }
            XMP nxmp = new XMP(path);
            if (Frames.Any(t => (((FrameACR)t).XMPFile == null) ? false : ((FrameACR)t).XMPFile.FileVersion != nxmp.FileVersion)) { throw new FileVersionException("Different XMP version than previously added XMP's"); }
            ((FrameACR)Frames[index]).XMPFile = new XMP(path);
            Frames[index].IsKeyframe = true;
        }

        /// <summary>
        /// Add a XMP keyframe
        /// </summary>
        /// <param name="index">Index where the keyframe is set</param>
        internal override void AddKeyframe(int index)
        {
            if (index < 0 && index >= Frames.Count) { throw new ArgumentException("Index too high or low"); }
            if (((FrameACR)Frames[index]).XMPFile == null) { throw new ArgumentNullException("Can't set Keyframe because there is no XMP connected to"); }
            Frames[index].IsKeyframe = true;
        }

        /// <summary>
        /// Removes a keyframe
        /// </summary>
        /// <param name="index">Index where the keyframe will be removed</param>
        /// <param name="RemoveXMP">States if the connection to the XMP file should be removed too</param>
        internal override void RemoveKeyframe(int index, bool removeLink)
        {
            if (index < 0 && index >= Frames.Count) { throw new ArgumentException("Index too high or low"); }
            Frames[index].IsKeyframe = false;
            if (removeLink) { ((FrameACR)Frames[index]).XMPFile = null; }
        }
        

        /// <summary>
        /// Reads and updates XMP from all frames
        /// </summary>
        internal void ReadXMP()
        {
            MainWorker.RunWorkerAsync(new KeyValuePair<Work, object>(Work.ReadXMP, null));
        }

        protected override void readXMP()
        {
            for (int i = 0; i < Frames.Count; i++)
            {
                if (MainWorker.CancellationPending) { return; }
                FrameACR frame = (FrameACR)Frames[i];
                if (frame.XMPFile == null) { frame.XMPFile = new XMP(frame.FilePath); }
                else { frame.XMPFile.Read(); }
                MainWorker.ReportProgress(0, new ProgressChangeEventArgs(i * 100 / (Frames.Count - 1), ProgressType.ReadXMP));
            }
        }
        
        protected override void WriteFiles()
        {
            XMP[] nxmp;
            double[] ys = new double[Frames.Count];

            if (KeyframeCount > 1)
            {
                nxmp = Interpolation.Do(this);
                double[] y = Frames.Where(t => t.IsKeyframe).Select(t => ((FrameACR)t).XMPFile.Exposure).ToArray();
                double[] x = new double[y.Length];
                int k = 0;
                for (int j = 0; j < Frames.Count; j++) { if (Frames[j].IsKeyframe) { x[k] = j; k++; } }
                ys = Interpolation.Do(x, y, Frames.Count);
            }
            else
            {
                XMP firstxmp = ((FrameACR)Frames.First(t => t.IsKeyframe)).XMPFile;
                nxmp = new XMP[Frames.Count];
                for (int j = 0; j < Frames.Count; j++)
                {
                    nxmp[j] = firstxmp.Copy();
                    nxmp[j].Path = ((FrameACR)Frames[j]).XMPFile.Path;
                    ys[j] = firstxmp.Exposure;
                }
            }

            for (int i = 0; i < nxmp.Length; i++)
            {
                if (IsBrightnessCalculated)
                {
                    if (i == 0 || i == nxmp.Length - 1) { nxmp[i].NewExposure = (ys[i] > 10) ? 10 : (ys[i] < -5) ? -5 : ys[i]; }
                    else
                    {
                        nxmp[i].NewExposure = Math.Log(Frames[i].NewBrightness / Frames[i].AlternativeBrightness, 2) + ((ys[i] > 5) ? 5 : (ys[i] < -5) ? -5 : ys[i]);
                        if (double.IsNaN(nxmp[i].NewExposure)) { throw new NotFiniteNumberException(); }
                    }
                }
                else { nxmp[i].NewExposure = (ys[i] > 5) ? 5 : (ys[i] < -5) ? -5 : ys[i]; }
                nxmp[i].Write();
                MainWorker.ReportProgress(0, new ProgressChangeEventArgs(i * 100 / (nxmp.Length - 1), ProgressType.WriteXMP));
            }
        }

        protected override void SetFrames(string[] files)
        {
            for (int i = 0; i < files.Length; i++) { Frames.Add(new FrameACR(files[i])); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using ColorManagment;
using System.Globalization;

namespace Timelapse_API
{
    /// <summary>
    /// Stores and handles all data for a project
    /// </summary>
    public abstract class Project : IDisposable
    {
        #region Variables

		/// <summary>
		/// Type of project
		/// </summary>
		public abstract ProjectType Type { get; }
        /// <summary>
        /// A list of all frames
        /// </summary>
        public List<Frame> Frames
        {
            get { return _Frames; }
            protected set { _Frames = value; }
        }
        /// <summary>
        /// States if the brightness of all frames are calculated
        /// </summary>
        public bool IsBrightnessCalculated
        {
            get { return _IsBrightnessCalculated; }
            internal set { _IsBrightnessCalculated = value; }
        }
        /// <summary>
        /// States if currently something is calculating or working
        /// </summary>
        public bool IsWorking
        {
            get { return MainWorker.IsBusy; }
        }
        /// <summary>
        /// The number of all keyframes within this project
        /// </summary>
        public int KeyframeCount
        {
            get { return this.Frames.Count(t => t.IsKeyframe); }
        }
        /// <summary>
        /// The area that will be used for the simple brightness calculation
        /// </summary>
        public Rectangle SimpleCalculationArea;
        /// <summary>
        /// File extensions supported by the project. (e.g. ".jpg")
        /// </summary>
        public abstract string[] AllowedFileExtensions
        {
            get;
        }
        /// <summary>
        /// If the current project is busy, use this to wait for it to end
        /// </summary>
        public readonly AutoResetEvent IsWorkingWaitHandler = new AutoResetEvent(false);
        /// <summary>
        /// Path, where the rendered images will be saved to. null if not set
        /// </summary>
        internal string ImageSavePath;
        
        internal BackgroundWorker MainWorker;
        internal ThumbHandler ThumbsStorage;
        private List<Frame> _Frames;
        private bool _IsBrightnessCalculated;

        #endregion

        #region Events

        internal void OnProgressChanged(object sender, ProgressChangeEventArgs e)
        {
            if (ProgressChanged != null) { ProgressChanged(sender, e); }
        }
        internal void OnFramesLoaded(object sender, WorkFinishedEventArgs e)
        {
            if (FramesLoaded != null) { FramesLoaded(sender, e); }
        }
        internal void OnBrightnessCalculated(object sender, WorkFinishedEventArgs e)
        {
            if (BrightnessCalculated != null) { BrightnessCalculated(sender, e); }
        }
        internal void OnWorkDone(object sender, WorkFinishedEventArgs e)
        {
            if (WorkDone != null) { WorkDone(sender, e); }
        }

        /// <summary>
        /// Fires when any progress has been done within a project
        /// </summary>
        internal event ProgressChangeHandler ProgressChanged;
        /// <summary>
        /// Fires when all frames are loaded
        /// </summary>
        internal event WorkFinishedHandler FramesLoaded;
        /// <summary>
        /// Fires when the brightness calculation is finished
        /// </summary>
        internal event WorkFinishedHandler BrightnessCalculated;
        /// <summary>
        /// Fires when all files are processed
        /// </summary>
        internal event WorkFinishedHandler WorkDone;

        #endregion

        /// <summary>
        /// Initiates a new project
        /// </summary>
        internal Project()
        {
            _Frames = new List<Frame>();
            IsBrightnessCalculated = false;
            ThumbsStorage = new ThumbHandler(10);

            MainWorker = new BackgroundWorker();
            MainWorker.WorkerReportsProgress = true;
            MainWorker.WorkerSupportsCancellation = true;
            MainWorker.DoWork += new DoWorkEventHandler(MainWorker_DoWork);
            MainWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(MainWorker_RunWorkerCompleted);
            MainWorker.ProgressChanged += new ProgressChangedEventHandler(MainWorker_ProgressChanged);
        }

        #region Public Methods

        public void Dispose()
        {
            MainWorker.Dispose();
            Frames.Clear();
            ThumbsStorage.Dispose();
        }
        
        public BitmapEx GetThumb(int index, bool UseBuffer = true)
        {
            return ThumbsStorage[index * 2, UseBuffer];
        }

        public void SetThumb(int index, BitmapEx thumb)
        {
            ThumbsStorage[index * 2] = thumb;
        }

        public BitmapEx GetThumbEdited(int index, bool UseBuffer = true)
        {
            return ThumbsStorage[index * 2 +1, UseBuffer];
        }

        public void SetThumbEdited(int index, BitmapEx thumb)
        {
            ThumbsStorage[index * 2 + 1] = thumb;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Adds a thumb to the thumb storage
        /// </summary>
        /// <param name="thumb">The thumbnail</param>
        internal void AddThumb(BitmapEx thumb)
        {
            ThumbsStorage.AddThumb(thumb);
        }

        /// <summary>
        /// Add all frames from a certain directory to the project
        /// </summary>
        /// <param name="directory">The directory from where the frames will be loaded</param>
        internal bool AddFrames(string directory)
        {
            if (Frames.Count == 0)
            {
                if (!Directory.Exists(directory)) { throw new DirectoryNotFoundException(); }
                string[] files = Directory.GetFiles(directory);
                files = SortOutImages(files);
                if (files.Length < 2) return false;
                AddFrames(files);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Cancel a currently running work
        /// </summary>
        internal void Cancel()
        {
            if (MainWorker.IsBusy && !MainWorker.CancellationPending)
            {
                MainWorker.CancelAsync();
                if (ProjectManager.UsedProgram == ProjectType.RawTherapee)
                {
                    try { ((ProjectRT)this).RT.Kill(); }
                    catch (InvalidOperationException) { }
                }
                try { Exiftool.exiftool.Kill(); }
                catch (InvalidOperationException) { }
            }
        }

        /// <summary>
        /// Writes processing data or images to disk
        /// </summary>
        internal void ProcessFiles()
        {
            if (Frames.Count < 2) { throw new RequirementsNotFulfilledException(); }
            MainWorker.RunWorkerAsync(new KeyValuePair<Work, object>(Work.ProcessFiles, null));
        }

        /// <summary>
        /// Calculates the brightness of all frames
        /// </summary>
        internal void CalculateBrightness(BrightnessCalcType CalcType)
        {
            MainWorker.RunWorkerAsync(new KeyValuePair<Work, object>(Work.CalculateBrightness, CalcType));
        }

        /// <summary>
        /// Sets the AltBrightness for each frame
        /// </summary>
        /// <param name="Keypoints">The keypoints which will be used for interpolation (x has to be between 0 an framecout)</param>
        internal void SetNewBrightness(List<PointD> Keypoints)
        {
            PointD[] points = Interpolation.Do(Keypoints.ToArray(), Frames.Count);
            for (int i = 0; i < Frames.Count; i++) { Frames[i].NewBrightness = points[i].Y; }
        }

        /// <summary>
        /// Process all thumbs for a preview
        /// </summary>
        internal void ProcessThumbs()
        {
            MainWorker.RunWorkerAsync(new KeyValuePair<Work, object>(Work.ProcessThumbs, null));
        }

        /// <summary>
        /// Set a keyframe
        /// </summary>
        /// <param name="index">Index where the keyframe is set</param>
        internal abstract void AddKeyframe(int index);

        /// <summary>
        /// Set a keyframe
        /// </summary>
        /// <param name="index">Index where the keyframe is set</param>
        /// <param name="path">Path to the metadata file</param>
        internal abstract void AddKeyframe(int index, string path);

        /// <summary>
        /// Removes a keyframe
        /// </summary>
        /// <param name="index">Index where the keyframe will be removed</param>
        /// <param name="removeLink">States if the connection to the metadata file should be removed too</param>
        internal abstract void RemoveKeyframe(int index, bool removeLink);

        /// <summary>
        /// After deserializing the project, this has to be called
        /// </summary>
        internal void ProjectLoad(SavingStorage st)
        {
            Frames = st.Frames;
            IsBrightnessCalculated = st.IsBrightnessCalculated;
            SimpleCalculationArea = st.SimpleCalculationArea;
            ImageSavePath = st.ImageSavePath;
            MainWorker.RunWorkerAsync(new KeyValuePair<Work, object>(Work.LoadProject, null));
        }

        /// <summary>
        /// Report progress from outside the Project class
        /// </summary>
        /// <param name="percent">Percent of finished work (0-100)</param>
        /// <param name="type">Type of work</param>
        internal void ReportWorkProgress(int percent, ProgressType type)
        {
            switch (type)
            {
                case ProgressType.LoadThumbnails:
                    MainWorker.ReportProgress(0, new ProgressChangeEventArgs(percent, ProgressType.LoadThumbnails));
                    break;
                case ProgressType.LoadFrames:
                    MainWorker.ReportProgress(0, new ProgressChangeEventArgs((int)(33 * percent / 100f), ProgressType.LoadFrames));
                    break;

                default:
                    MainWorker.ReportProgress(0, new ProgressChangeEventArgs(percent, type));
                    break;
            }
        }

        #endregion

        #region Protected Methods

        protected void MainWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnProgressChanged(this, (ProgressChangeEventArgs)e.UserState);
        }

        protected void MainWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null) { throw e.Error; } //TODO: handle error (no direct UI call => no UI handle)
            KeyValuePair<Work, object> arg = (KeyValuePair<Work, object>)e.Result;
            WorkFinishedEventArgs eArgs = new WorkFinishedEventArgs(e.Cancelled, arg.Key);
            switch (arg.Key)
            {
                case Work.LoadFrames:
                    if (e.Cancelled)
                    {
                        foreach (string s in (string[])arg.Value)
                        {
                            Frame f = Frames.FirstOrDefault(t => t.FilePath == s);
                            if (f == null) { break; }
                            else { Frames.Remove(f); }
                        }
                    }
                    OnFramesLoaded(this, eArgs);
                    break;

                case Work.CalculateBrightness:
                    IsBrightnessCalculated = !e.Cancelled;
                    OnBrightnessCalculated(this, eArgs);
                    break;
                default:
                    OnWorkDone(this, eArgs);
                    break;
            }
            IsWorkingWaitHandler.Set();
        }

        protected void MainWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            IsWorkingWaitHandler.Reset();

            KeyValuePair<Work, object> arg = (KeyValuePair<Work, object>)e.Argument;
            e.Result = arg;

            MainWorker.ReportProgress(0, new ProgressChangeEventArgs(0, ProgressType.StartingWork));

            switch (arg.Key)
            {
                case Work.LoadFrames:
                    LoadFrames((string[])arg.Value);
                    break;
                case Work.CalculateBrightness:
                    CalculateBrightnessWork((BrightnessCalcType)arg.Value);
                    break;
                case Work.ProcessFiles:
                    WriteFiles();
                    break;
                case Work.ProcessThumbs:
                    ThumbProcessing();
                    break;
                case Work.ReadXMP:
                    readXMP();
                    break;
                case Work.LoadProject:
                    LoadThumbnails(Frames.Select(t => t.FilePath).ToArray());
                    break;
            }
        }
        
        #region Loading Frames, Thumbs and Metadata

        protected virtual void AddFrames(string[] files)
        {
            MainWorker.RunWorkerAsync(new KeyValuePair<Work, object>(Work.LoadFrames, files));
        }

        protected void LoadFrames(string[] files)
        {
            string directory = Path.GetDirectoryName(files[0]);

            SetFrames(files);
            MainWorker.ReportProgress(0, new ProgressChangeEventArgs(33, ProgressType.LoadFrames));

            LoadMetadata(directory);
            MainWorker.ReportProgress(0, new ProgressChangeEventArgs(66, ProgressType.LoadMetadata));

            LoadThumbnails(files);
        }

        protected virtual void SetFrames(string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                switch (Type)
                {
                    case ProjectType.CameraRaw:
                        Frames.Add(new FrameACR(files[i]));
                        break;
                    case ProjectType.LapseStudio:
                        Frames.Add(new FrameLS(files[i]));
                        break;
                    case ProjectType.RawTherapee:
                        Frames.Add(new FrameRT(files[i]));
                        break;
                }
            }
        }

        protected void LoadMetadata(string directory)
        {
            string[] lines = Exiftool.GetExifMetadata(directory);
            CultureInfo culture = new CultureInfo("en-US");

            if (lines != null)
            {
                string TVtmp;
                string AVtmp;
                string SVtmp;
                double TVdenom;
                double TVnume;
                double tmpD;
                int tmpI;
                int f = -1;

                for (int i = 0; i < lines.Length; i++)
                {
                    if (MainWorker.CancellationPending) { return; }
                    

					if(lines[i].StartsWith("======== ")) f = Frames.FindIndex(t => lines[i].ToLower() == "======== " + t.FilePath.ToLower().Replace(@"\", "/"));
                    else if (f >= 0)
                    {
                        if (lines[i].ToLower().StartsWith("aperturevalue"))
                        {
                            AVtmp = lines[i].Substring(lines[i].IndexOf(":") + 2);
                            if (double.TryParse(AVtmp, NumberStyles.Any, culture, out tmpD))
                            {
                                Frames[f].AVstring = AVtmp;
                                Frames[f].Av = tmpD;
                                Frames[f].Av = Math.Log(Math.Pow(Frames[f].Av, 2), 2);
                            }
                            else
                            {
                                Frames[f].Av = 0;
                                Frames[f].AVstring = "N/A";
                            }
                        }
                        else if (lines[i].ToLower().StartsWith("shutterspeedvalue"))
                        {
                            TVtmp = lines[i].Substring(lines[i].IndexOf(":") + 2);

                            if (TVtmp.Contains(@"/"))
                            {
                                if (double.TryParse(TVtmp.Substring(0, TVtmp.IndexOf(@"/")), NumberStyles.Any, culture, out TVnume)
                                    && double.TryParse(TVtmp.Substring(TVtmp.IndexOf(@"/") + 1), NumberStyles.Any, culture, out TVdenom))
                                {
                                    Frames[f].Tv = TVnume / TVdenom;
                                    Frames[f].TVstring = TVtmp;
                                }
                                else Frames[f].TVstring = "N/A";
                            }
                            else if (double.TryParse(TVtmp, NumberStyles.Any, culture, out tmpD))
                            {
                                Frames[f].Tv = tmpD;
                                Frames[f].TVstring = TVtmp;
                            }
                            else Frames[f].TVstring = "N/A";

                            Frames[f].Tv = Math.Log(1 / Frames[f].Tv, 2);
                        }
                        else if (lines[i].ToLower().StartsWith("iso"))
                        {
                            SVtmp = lines[i].Substring(lines[i].IndexOf(":") + 2);
                            if (int.TryParse(SVtmp, out tmpI))
                            {
                                Frames[f].Sv = Math.Log(tmpI, 3.125f);
                                Frames[f].SVstring = SVtmp;
                            }
                            else Frames[f].SVstring = "N/A";
                        }
                        else if (lines[i].ToLower().StartsWith("wb_rggblevelsasshot"))
                        {
                            //string[] WBtmp = lines[i].Substring(lines[i].IndexOf(":") + 2).Split(' ');
                            //int[] WBvals = new int[4] { Convert.ToInt32(WBtmp[0]), Convert.ToInt32(WBtmp[1]), Convert.ToInt32(WBtmp[2]), Convert.ToInt32(WBtmp[3]) };
                            //LTODO: calc the resulting brightness steps and write to Frames[f].WB
                        }
                        else if (lines[i].ToLower().StartsWith("colorspace"))
                        {
                            Frames[f].ColorSpace = ColorManagment.RGBColorspace.GetColorspaceName(lines[i].Substring(lines[i].IndexOf(":") + 2));
                        }
                        else if (lines[i].ToLower().StartsWith("imagesize"))
                        {
                            string[] tmp = lines[i].Substring(lines[i].IndexOf(":") + 2).Split('x');
                            if (int.TryParse(tmp[0], out tmpI)) Frames[f].Width = tmpI;
                            else Frames[f].Width = 1;
                            if (int.TryParse(tmp[1], out tmpI)) Frames[f].Height = tmpI;
                            else Frames[f].Height = 1;
                        }
                    }

                    if (f >= 0 && f < Frames.Count)
                    {
                        if (Frames[f].AVstring == "N/A" && Frames[f].TVstring == "N/A" && Frames[f].SVstring == "N/A") { Frames[f].HasMetadata = false; }
                        else { Frames[f].HasMetadata = true; }

                        //Brightness Value
                        Frames[f].Bv = Frames[f].Av + Frames[f].Tv - Frames[f].Sv;
                    }
                }
            }
        }

        protected virtual void LoadThumbnails(string[] files)
        {
            Exiftool.ExtractThumbnails(files);
        }

        #endregion

        #region Brightness Calculation

        private void CalculateBrightnessWork(BrightnessCalcType CalculationType)
        {
            switch (CalculationType)
            {
                case BrightnessCalcType.Advanced:
                    BrCalc_Advanced();
                    break;
                case BrightnessCalcType.Exif:
                    BrCalc_Exif();
                    break;
                case BrightnessCalcType.Simple:
                    BrCalc_Simple();
                    break;
                case BrightnessCalcType.AdvancedII:
                    BrCalc_AdvancedII();
                    break;
                case BrightnessCalcType.Lab:
                    BrCalc_Lab();
                    break;

                default:
                    BrCalc_Advanced();
                    break;
            }
        }

        private void BrCalc_Advanced()
        {
            #region Variables

            BitmapEx bmp1, bmp2, bmp3;
            const int min = 5;
            const int max = 250;

            bmp1 = GetThumb(0, true);

            int isdark, xS, yS;
            const uint ThumbWidth = 300;
            const uint ThumbHeight = 200;
            uint x, y, rowstride, n = bmp1.ChannelCount;
            long index, count;
            double br1, br2, br3, newBr, maxiBrDiff;

            double[,] BrightChangeMask = new double[ThumbWidth, ThumbHeight];
            bool[,] NonUseMask = new bool[ThumbWidth, ThumbHeight];
            double[][] PixelBrightness = new double[ThumbWidth * ThumbHeight][];

            ColorRGB c;

            #endregion

            //TODO: make calculation parallel

            for (int f = 0; f < Frames.Count; f += 2)
            {
                if (MainWorker.CancellationPending) { return; }
                
                #region Variables

                if (f == 0) { f++; }
                if (f + 2 >= Frames.Count) { f = Frames.Count - 2; }

                bmp1 = GetThumb(f - 1, false).Scale(ThumbWidth, ThumbHeight);
                bmp2 = GetThumb(f, false).Scale(ThumbWidth, ThumbHeight);
                bmp3 = GetThumb(f + 1, false).Scale(ThumbWidth, ThumbHeight);
                rowstride = bmp1.Stride;

                n = bmp1.ChannelCount;
                index = count = isdark = 0;
                br1 = br2 = br3 = newBr = maxiBrDiff = 0;
                
                #endregion

                unsafe
                {
                    bmp1.LockBits();
                    bmp2.LockBits();
                    bmp3.LockBits();

                    byte* pix1 = (byte*)bmp1.Scan0;
                    byte* pix2 = (byte*)bmp2.Scan0;
                    byte* pix3 = (byte*)bmp3.Scan0;

                    #region Mask
                    
                    for (y = 0; y < ThumbHeight; y++)
                    {
                        for (x = 0; x < rowstride; x += n)
                        {
                            index = y * rowstride + x;

                            c = new ColorRGB(RGBSpaceName.sRGB, pix1[index], pix1[index + 1], pix1[index + 2], false);
                            c = c.ToLinear();
                            br1 = (c.R + c.G + c.B) * 255d / 3d;
                            c = new ColorRGB(RGBSpaceName.sRGB, pix2[index], pix2[index + 1], pix2[index + 2], false);
                            c = c.ToLinear();
                            br2 = (c.R + c.G + c.B) * 255d / 3d;
                            c = new ColorRGB(RGBSpaceName.sRGB, pix3[index], pix3[index + 1], pix3[index + 2], false);
                            c = c.ToLinear();
                            br3 = (c.R + c.G + c.B) * 255d / 3d;

                            if (br1 > min && br2 > min && br3 > min && br1 < max && br2 < max && br3 < max) { NonUseMask[x / n, y] = false; }
                            else { NonUseMask[x / n, y] = true; }
                        }
                    }

                    List<double> brightnessDiff1;
                    List<double> brightnessDiff2;

                    for (y = 0; y < ThumbHeight; y++)
                    {
                        for (x = 0; x < bmp1.Stride; x += n)
                        {
                            if (NonUseMask[x / n, y] == false)
                            {
                                count = 0;
                                brightnessDiff1 = new List<double>();
                                brightnessDiff2 = new List<double>();

                                for (yS = -1; yS <= 1; yS++)
                                {
                                    if (y + yS < ThumbHeight && y + yS >= 0)
                                    {
                                        for (xS = -1; xS <= 1; xS++)
                                        {
                                            if (x + xS < rowstride && x + xS >= 0)
                                            {
                                                if (NonUseMask[(x + xS) / n, y + yS] == false)
                                                {
                                                    index = (y + yS) * rowstride + x + xS;

                                                    c = new ColorRGB(RGBSpaceName.sRGB, pix1[index], pix1[index + 1], pix1[index + 2], false);
                                                    c = c.ToLinear();
                                                    br1 = (c.R + c.G + c.B) * 255d / 3d;
                                                    c = new ColorRGB(RGBSpaceName.sRGB, pix2[index], pix2[index + 1], pix2[index + 2], false);
                                                    c = c.ToLinear();
                                                    br2 = (c.R + c.G + c.B) * 255d / 3d;
                                                    c = new ColorRGB(RGBSpaceName.sRGB, pix3[index], pix3[index + 1], pix3[index + 2], false);
                                                    c = c.ToLinear();
                                                    br3 = (c.R + c.G + c.B) * 255d / 3d;

                                                    brightnessDiff1.Add(Math.Abs(br1 - br2));
                                                    brightnessDiff2.Add(Math.Abs(br2 - br3));

                                                    count++;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (count > 0) { BrightChangeMask[x / n, y] = Math.Max(brightnessDiff1.Average(), brightnessDiff2.Average()); }
                                else { BrightChangeMask[x / n, y] = 0; }

                                if (maxiBrDiff < BrightChangeMask[x / n, y]) { maxiBrDiff = BrightChangeMask[x / n, y]; }
                            }
                            else { BrightChangeMask[x / n, y] = 0; }
                        }
                    }
                    bmp1.UnlockBits();
                    bmp2.UnlockBits();
                    bmp3.UnlockBits();
                    
                    for (y = 0; y < ThumbHeight; y++)
                    {
                        for (x = 0; x < ThumbWidth; x++)
                        {
                            if (NonUseMask[x, y] == false)
                            {
                                if (maxiBrDiff > 0)
                                {
                                    newBr = (BrightChangeMask[x, y] * 100) / maxiBrDiff;
                                    if (newBr > 100) { newBr = 100; }
                                    BrightChangeMask[x, y] = Math.Abs(newBr - 100);
                                }
                                else { BrightChangeMask[x, y] = 100; }
                            }
                            else { BrightChangeMask[x, y] = 0; }

                            if (BrightChangeMask[x, y] < 0.5) { isdark++; }
                        }
                    }

                    if ((isdark * 100) / (ThumbHeight * ThumbWidth) > 97) { throw new ImageTooDarkException(); }
                    if (this.GetType() == typeof(ProjectLS)) { ((FrameLS)Frames[f]).UsageMask = BrightChangeMask; }

                    #endregion

                    MainWorker.ReportProgress(0, new ProgressChangeEventArgs((f + 1) * 100 / (Frames.Count - 1), ProgressType.CalculateBrightness));

                    #region Brightness

                    count = 0;

                    for (y = 0; y < ThumbHeight; y++)
                    {
                        for (x = 0; x < rowstride; x += n)
                        {
                            index = y * rowstride + x;
                            c = new ColorRGB(RGBSpaceName.sRGB, pix1[index], pix1[index + 1], pix1[index + 2], false);
                            c = c.ToLinear();
                            br1 = (c.R + c.G + c.B) * 255d / 3d;
                            c = new ColorRGB(RGBSpaceName.sRGB, pix2[index], pix2[index + 1], pix2[index + 2], false);
                            c = c.ToLinear();
                            br2 = (c.R + c.G + c.B) * 255d / 3d;
                            c = new ColorRGB(RGBSpaceName.sRGB, pix3[index], pix3[index + 1], pix3[index + 2], false);
                            c = c.ToLinear();
                            br3 = (c.R + c.G + c.B) * 255d / 3d;

                            double factor = BrightChangeMask[x / n, y] / 100;
                            PixelBrightness[count] = new double[3] { br1 * factor, br2 * factor, br3 * factor };
                            count++;
                        }
                    }


                    double val0 = (PixelBrightness.Average(p => p[0]));
                    double val1 = (PixelBrightness.Average(p => p[1]));
                    double val2 = (PixelBrightness.Average(p => p[2]));

                    if (f == 1) { Frames[0].OriginalBrightness = val0; }
                    double val1P = (val1 * 100) / val0;
                    Frames[f].OriginalBrightness = (Frames[f - 1].OriginalBrightness * val1P) / 100;
                    double val2P = (val2 * 100) / val0;
                    Frames[f + 1].OriginalBrightness = (Frames[f - 1].OriginalBrightness * val2P) / 100;

                    Frames[f - 1].AlternativeBrightness = Frames[f - 1].OriginalBrightness;
                    Frames[f].AlternativeBrightness = Frames[f].OriginalBrightness;
                    Frames[f + 1].AlternativeBrightness = Frames[f + 1].OriginalBrightness;

                    Frames[f - 1].NewBrightness = Frames[f - 1].OriginalBrightness;
                    Frames[f].NewBrightness = Frames[f].OriginalBrightness;
                    Frames[f + 1].NewBrightness = Frames[f + 1].OriginalBrightness;

                    #endregion
                }
            }

            //LTODO: write statistic/BV-value check
            #region Statistics/BV-Values Check

            /*Status = CalcState.Statistics;

                for (int f = 1; f < filecount; f++)
                {
                    if (AllFiles[f].HasExif)
                    {
                        double EvP = (Math.Min(AllFiles[f].Brightness, AllFiles[f - 1].Brightness) * 100) / Math.Max(AllFiles[f].Brightness, AllFiles[f - 1].Brightness);
                        double bv1 = Math.Abs(AllFiles[f - 1].Bv);
                        double bv2 = Math.Abs(AllFiles[f].Bv);
                        double BvP = (Math.Min(bv1, bv2) * 100) / Math.Max(bv1, bv2);
                        if (bv1 < bv2) { BvP *= -1; }
                        double res = BvP - EvP;
                        if (AllFiles[f].Brightness < AllFiles[f - 1].Brightness) { res *= -1; }
                        AllFiles[f].StatisticalError = res;
                    }
                    else
                    {
                        //do statistical check
                    }

                    CalcWorker[0].ReportProgress((int)((((double)f / (double)filecount)) * 100f), Status);
                }*/

            #endregion Statistics/BV-Values Check
        }

        private void BrCalc_AdvancedII()
        {
            //Check whole image for changes
            //split changes in 4(5) groups: dark, light dark, (mid), light bright, bright
            //adjust each group individually with each programmatical equivalent or use curves if not available
        }

        private void BrCalc_Lab()
        {
            BitmapEx bmp1, bmp2, bmp3;
            const int min = 5;
            const int max = 95;

            bmp1 = GetThumb(0, true);

            const uint ThumbWidth = 300;
            const uint ThumbHeight = 200;
            uint rowstride;
            int n = bmp1.ChannelCount;

            long index = 0;
            uint x, y;
            int x1, y1, maxCol = 8;
            double fact, d1, d2;
            ColorConverter Converter = new ColorConverter();
            List<double[]> PixelBrightness;

            ColorLab[,] labimg1 = new ColorLab[ThumbWidth, ThumbHeight];
            ColorLab[,] labimg2 = new ColorLab[ThumbWidth, ThumbHeight];
            ColorLab[,] labimg3 = new ColorLab[ThumbWidth, ThumbHeight];

            bool[,] NonUseMask = new bool[ThumbWidth, ThumbHeight];

            for (int f = 1; f < Frames.Count; f += 2)
            {
                if (MainWorker.CancellationPending) { return; }

                if (f + 2 >= Frames.Count) { f = Frames.Count - 2; }

                bmp1 = GetThumb(f - 1, false).Scale(ThumbWidth, ThumbHeight);
                bmp2 = GetThumb(f, false).Scale(ThumbWidth, ThumbHeight);
                bmp3 = GetThumb(f + 1, false).Scale(ThumbWidth, ThumbHeight);
                rowstride = bmp1.Stride;

                unsafe
                {
                    bmp1.LockBits();
                    bmp2.LockBits();
                    bmp3.LockBits();

                    byte* pix1 = (byte*)bmp1.Scan0;
                    byte* pix2 = (byte*)bmp2.Scan0;
                    byte* pix3 = (byte*)bmp3.Scan0;

                    //Non use Mask and Lab conversion
                    for (y = 0; y < ThumbHeight; y++)
                    {
                        for (x = 0; x < ThumbWidth; x++)
                        {
                            index = y * rowstride + (x * n);

                            labimg1[x, y] = Converter.ToLab(new ColorRGB(RGBSpaceName.sRGB, pix1[index], pix1[index + 1], pix1[index + 2]));
                            labimg2[x, y] = Converter.ToLab(new ColorRGB(RGBSpaceName.sRGB, pix2[index], pix2[index + 1], pix2[index + 2]));
                            labimg3[x, y] = Converter.ToLab(new ColorRGB(RGBSpaceName.sRGB, pix3[index], pix3[index + 1], pix3[index + 2]));

                            if (labimg1[x, y].L < min || labimg1[x, y].L > max || labimg2[x, y].L < min || labimg2[x, y].L > max || labimg3[x, y].L < min || labimg3[x, y].L > max ||
                                Math.Abs(labimg2[x, y].a - labimg1[x, y].a) > maxCol || Math.Abs(labimg3[x, y].a - labimg2[x, y].a) > maxCol ||
                                Math.Abs(labimg2[x, y].b - labimg1[x, y].b) > maxCol || Math.Abs(labimg3[x, y].b - labimg2[x, y].b) > maxCol)
                            {
                                NonUseMask[x, y] = true;
                            }
                        }
                    }
                    bmp1.UnlockBits();
                    bmp2.UnlockBits();
                    bmp3.UnlockBits();
                }

                PixelBrightness = new List<double[]>();

                //Brightness calculation
                for (y = 0; y < ThumbHeight; y++)
                {
                    for (x = 0; x < ThumbWidth; x++)
                    {
                        if (!NonUseMask[x, y])
                        {
                            fact = 0;
                            if (y > 0 && x > 0 && y < ThumbHeight - 1 && x < ThumbWidth - 1)
                            {
                                d1 = 0;
                                d2 = 0;
                                for (y1 = -1; y1 <= 1; y1++)
                                {
                                    for (x1 = -1; x1 <= 1; x1++)
                                    {
                                        d1 += Math.Abs(labimg1[x, y].L - labimg2[x, y].L);
                                        d2 += Math.Abs(labimg2[x, y].L - labimg3[x, y].L);
                                    }
                                }
                                fact = Math.Max(d1 / 9d, d2 / 9d);
                            }
                            if (fact > 0.2) PixelBrightness.Add(new double[] { fact * Math.Log(labimg1[x, y].L), fact * Math.Log(labimg2[x, y].L), fact * Math.Log(labimg3[x, y].L) });
                        }
                    }
                }

                Frames[f - 1].OriginalBrightness = PixelBrightness.Average(p => p[0]);
                Frames[f].OriginalBrightness = PixelBrightness.Average(p => p[1]);
                Frames[f + 1].OriginalBrightness = PixelBrightness.Average(p => p[2]);

                Frames[f - 1].AlternativeBrightness = Frames[f - 1].OriginalBrightness;
                Frames[f].AlternativeBrightness = Frames[f].OriginalBrightness;
                Frames[f + 1].AlternativeBrightness = Frames[f + 1].OriginalBrightness;

                Frames[f - 1].NewBrightness = Frames[f - 1].OriginalBrightness;
                Frames[f].NewBrightness = Frames[f].OriginalBrightness;
                Frames[f + 1].NewBrightness = Frames[f + 1].OriginalBrightness;

                MainWorker.ReportProgress(0, new ProgressChangeEventArgs(f * 100 / (Frames.Count - 1), ProgressType.CalculateBrightness));
            }
        }

        private void BrCalc_Simple()
        {
            if (SimpleCalculationArea.Height < 0 || SimpleCalculationArea.Width < 0 || SimpleCalculationArea.X < 0 || SimpleCalculationArea.Y < 0 ||
                       SimpleCalculationArea.Height > Frames[0].Height || SimpleCalculationArea.Width > Frames[0].Width)
                throw new ArgumentException("Calculation area must be on thumb image");

            for (int f = 0; f < Frames.Count; f++)
            {
                double Brightness = 0;
                long index = 0;
                int pixcount = 0;
                BitmapEx bmp = GetThumb(f, false).Scale(300, 200);

                unsafe
                {
                    bmp.LockBits();
                    byte* pix = (byte*)bmp.Scan0;

                    for (int y = SimpleCalculationArea.X; y < SimpleCalculationArea.Height; y++)
                    {
                        for (int x = SimpleCalculationArea.Y; x < SimpleCalculationArea.Width; x++)
                        {
                            index = y * bmp.Stride + x * bmp.ChannelCount;

                            Brightness += Math.Sqrt(Math.Pow(pix[index], 2) * 0.241 + Math.Pow(pix[index + 1], 2) * 0.691 + Math.Pow(pix[index + 2], 2) * 0.068);
                            pixcount++;
                        }
                    }
                    bmp.UnlockBits();
                }

                Frames[f].OriginalBrightness = Brightness / pixcount;
                Frames[f].AlternativeBrightness = Frames[f].OriginalBrightness;
                Frames[f].NewBrightness = Frames[f].OriginalBrightness;
            }
        }

        private void BrCalc_Exif()
        {
            //LTODO: write br calc exif
        }

        #endregion

        protected abstract void WriteFiles();

        protected virtual void ThumbProcessing()
        {
            double ld = Math.Log(2);
            long index = 0;
            ColorRGB crgb = new ColorRGB(RGBSpaceName.sRGB, false);
            ColorLab cl = new ColorLab();
            ColorConverter Converter = new ColorConverter();
            BitmapEx bmp, bmpE;

            for (int i = 0; i < Frames.Count; i++)
            {
                double exposure = Math.Log(Frames[i].NewBrightness / Frames[i].AlternativeBrightness, 2);
                bmp = GetThumb(i, false);
                bmpE = GetThumbEdited(i, false);
                
                unsafe
                {
                    bmp.LockBits();
                    bmpE.LockBits();
                    byte* pix1 = (byte*)bmp.Scan0;
                    byte* pix2 = (byte*)bmpE.Scan0;

                    for (uint y = 0; y < bmp.Height; y++)
                    {
                        for (uint x = 0; x < bmp.Stride; x += bmp.ChannelCount)
                        {
                            index = y * bmp.Stride + x;

                            //LTODO: doesn't calculate correctly
                            crgb.R = pix1[index] / 255d; crgb.G = pix1[index + 1] / 255d; crgb.B = pix1[index + 2] / 255d;
                            cl = Converter.ToLab(crgb);
                            if (exposure < 0) { cl.L = (cl.L < 0.94) ? cl.L : Math.Exp(exposure * ld) * cl.L; }
                            else if (exposure > 0) { cl.L = (cl.L > 0.06) ? cl.L : Math.Exp(exposure * ld) * cl.L; }
                            crgb = Converter.ToRGB(cl, RGBSpaceName.sRGB);
                            crgb = crgb.ToNonLinear();

                            pix2[index] = (byte)(crgb.R * byte.MaxValue);
                            pix2[index + 1] = (byte)(crgb.G * byte.MaxValue);
                            pix2[index + 2] = (byte)(crgb.B * byte.MaxValue);
                        }
                    }
                    bmp.UnlockBits();
                    bmpE.UnlockBits();
                }

                SetThumbEdited(i, bmpE);
                MainWorker.ReportProgress(0, new ProgressChangeEventArgs(i * 100 / (Frames.Count - 1), ProgressType.ProcessingThumbs));
            }
        }

        protected string[] SortOutImages(string[] AllFiles)
        {
            return AllFiles.Where(t => AllowedFileExtensions.Any(k => k == Path.GetExtension(t).ToLower())).ToArray();
        }

        protected virtual void readXMP()
        {
            //Nothing to do here (only ProjectACR needs it)
        }
        
        #endregion
    }    
}

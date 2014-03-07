using System;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace Timelapse_API
{
    /// <summary>
    /// A project for use with RawTherapee
    /// </summary>
    public class ProjectRT : Project
	{
		/// <summary>
		/// Type of project
		/// </summary>
		public override ProjectType Type { get { return ProjectType.RawTherapee; } }

        /// <summary>
        /// States if a frame of this project has a PP3 file
        /// </summary>
        public bool HasPP3
        {
            get { return this.Frames.Any(t => ((FrameRT)t).PP3File != null); } 
        }
        /// <summary>
        /// The version of the used PP3 file(s) in this project
        /// </summary>
        public int PP3Version
        {
            get
            {
                if (this.Frames.Count > 0 && HasPP3) { return ((FrameRT)this.Frames.First(t => ((FrameRT)t).PP3File != null)).PP3File.FileVersion; }
                else { return -1; }
            }
        }
        /// <summary>
        /// The number of PP3's that are set as a keyframe
        /// </summary>
        public int PP3KFcount
        {
            get { return this.Frames.Count(t => ((FrameRT)t).PP3File != null && t.IsKeyframe); }
        }
        /// <summary>
        /// States of RawTherapee will be started or if only PP3s are generated
        /// </summary>
		public bool RunRT;
        /// <summary>
        /// States if PP3 are kept after processing is done
        /// </summary>
		public bool KeepPP3;
        /// <summary>
        /// Path to RawTherapee
        /// </summary>
		public string RTPath;
        /// <summary>
        /// Image format in which images will be saved
        /// </summary>
		public FileFormat SaveFormat;
        /// <summary>
        /// Tiff compression of image
        /// </summary>
		public bool TiffCompression;
        /// <summary>
        /// Quality of jpg image
        /// </summary>
        public int JpgQuality;
        /// <summary>
        /// File extensions supported by the project. (e.g. ".jpg")
        /// </summary>
        public override string[] AllowedFileExtensions
        {
            get { return extensions; }
        }

        private static readonly string[] extensions = { ".jpg", ".jpeg", ".png", ".tif", ".tiff",".dng", ".cr2", ".crw", ".x3f", ".nef", ".srw", ".srf", ".sr2", ".arw",
                ".erf", ".pef", ".raf", ".3fr", ".fff", ".dcr", ".dcs", ".kdc", ".kdc", ".rwl", ".mrw", ".mdc", ".nrw",".orf", ".rw2" };


        internal Process RT = new Process();
        private FileSystemWatcher OutputWatcher = new FileSystemWatcher();

        /// <summary>
        /// Initiates a new RawTherapee project
        /// </summary>
        internal ProjectRT()
            : base()
        {
            OutputWatcher.Created += OutputWatcher_Created;
        }

        /// <summary>
        /// Add a PP3 keyframe
        /// </summary>
        /// <param name="path">Path to the PP3 file</param>
        /// <param name="index">Index where the keyframe is set</param>
        internal override void AddKeyframe(int index, string path)
        {
            if (index < 0 && index >= Frames.Count) { throw new ArgumentException("Index too high or low"); }
            PP3 npp3 = new PP3(path);
            if (Frames.Any(t => (((FrameRT)t).PP3File == null) ? false : ((FrameRT)t).PP3File.FileVersion != npp3.FileVersion)) { throw new FileVersionException("Different PP3 version than previously added PP3's"); }
            ((FrameRT)Frames[index]).PP3File = new PP3(path);
            Frames[index].IsKeyframe = true;
        }

        /// <summary>
        /// Add a PP3 keyframe
        /// </summary>
        /// <param name="index">Index where the keyframe is set</param>
        internal override void AddKeyframe(int index)
        {
            if (index < 0 && index >= Frames.Count) { throw new ArgumentException("Index too high or low"); }
            if (((FrameRT)Frames[index]).PP3File == null) { throw new ArgumentNullException("Can't set Keyframe because there is no PP3 connected to"); }
            Frames[index].IsKeyframe = true;
        }

        /// <summary>
        /// Removes a keyframe
        /// </summary>
        /// <param name="index">Index where the keyframe will be removed</param>
        /// <param name="RemovePP3">States if the connection to the PP3 file should be removed too</param>
        internal override void RemoveKeyframe(int index, bool removeLink)
        {
            if (index < 0 && index >= Frames.Count) { throw new ArgumentException("Index too high or low"); }
            Frames[index].IsKeyframe = false;
            if (removeLink) { ((FrameRT)Frames[index]).PP3File = null; }
        }

        /// <summary>
        /// This method searches for the RawTherapee executable depending on the platform
        /// </summary>
        /// <returns>The patht to the RawTherapee executable or null if not found</returns>
        public static string SearchForRT()
        {
            switch (ProjectManager.RunningPlatform)
            {
                case Platform.MacOSX:
                    string MacRTPath = "/Applications/RawTherapee.app";
					if (Directory.Exists(MacRTPath)) return MacRTPath;
                    else return null;

                case Platform.Unix:
                    string UnixRTPath = "/usr/bin/rawtherapee";
                    if (File.Exists(UnixRTPath)) return UnixRTPath;
                    else return null;

                case Platform.Windows:
                    string[] FoldersX64 = Directory.GetDirectories(@"C:\Program Files\");
                    int idx = FoldersX64.ToList().FindIndex(t => t.ToLower().Contains("rawtherapee"));
                    if (idx >= 0 && File.Exists(Path.Combine(FoldersX64[idx], "rawtherapee.exe"))) { return Path.Combine(FoldersX64[idx], "rawtherapee.exe"); }
                    else
                    {
                        string[] FoldersX86 = Directory.GetDirectories(@"C:\Program Files (x86)\");
                        idx = FoldersX86.ToList().FindIndex(t => t.ToLower().Contains("rawtherapee"));
                        if (idx >= 0 && File.Exists(Path.Combine(FoldersX86[idx], "rawtherapee.exe"))) { return Path.Combine(FoldersX86[idx], "rawtherapee.exe"); }
                    }
                    return null;

                default:
                    return null;
            }
        }

        protected override void WriteFiles()
        {
            InterpolatePP3();
            if (RunRT) { StartRT(); }
            if (!KeepPP3) { DeletePP3(); }
        }
        
        private void InterpolatePP3()
        {
            PP3[] npp3;
            double[] ys = new double[Frames.Count];

            if (KeyframeCount > 1)
            {
                npp3 = Interpolation.Do(this);                
                double[] y = Frames.Where(t => t.IsKeyframe).Select(t => ((FrameRT)t).PP3File.Compensation).ToArray();
                double[] x = new double[y.Length];
                int k = 0;
                for (int j = 0; j < Frames.Count; j++) { if (Frames[j].IsKeyframe) { x[k] = j; k++; } }
                ys = Interpolation.Do(x, y, Frames.Count);
            }
            else
            {
                PP3 firstpp3 = ((FrameRT)Frames.First(t => t.IsKeyframe)).PP3File;
                npp3 = new PP3[Frames.Count];
                for (int j = 0; j < Frames.Count; j++) 
                {
                    npp3[j] = firstpp3.Copy();
                    ys[j] = firstpp3.Compensation;
                }
            }

            for (int i = 0; i < npp3.Length; i++)
            {
                if (IsBrightnessCalculated)
                {
                    if (i == 0 || i == npp3.Length - 1) { npp3[i].NewCompensation = ys[i]; }
                    else
                    {
                        npp3[i].NewCompensation = Math.Log(Frames[i].NewBrightness / Frames[i].AlternativeBrightness, 2) + ys[i];
                        if (double.IsNaN(npp3[i].NewCompensation)) { throw new NotFiniteNumberException(); }
                    }
                }
                else { npp3[i].NewCompensation = ys[i]; }

                npp3[i].NewCompensation = (npp3[i].NewCompensation > 10) ? 10 : (npp3[i].NewCompensation < -5) ? -5 : npp3[i].NewCompensation;
                string path = Frames[i].FilePath + ".pp3";
                npp3[i].Write(path);
                MainWorker.ReportProgress(0, new ProgressChangeEventArgs(i * 100 / (npp3.Length - 1), ProgressType.WritePP3));
            }
        }

        private void StartRT()
        {
			string ExecPath = RTPath;
			if (ProjectManager.RunningPlatform == Platform.MacOSX) ExecPath = Path.Combine(RTPath, "Contents/MacOS/rawtherapee");
			if (!File.Exists(ExecPath)) { throw new FileNotFoundException("RawTherapee execution file not found"); }
            FileHandle.CreateDirectory(ProjectManager.ImageSavePath);

            ProcessStartInfo RTStartInfo = new ProcessStartInfo();
			RTStartInfo = new ProcessStartInfo(ExecPath);

            RTStartInfo.UseShellExecute = false;
            RTStartInfo.CreateNoWindow = true;
            RT.StartInfo = RTStartInfo;

            #region Build command

            string saveformat;
            string SaveFileType;
            switch (SaveFormat)
            {
                case FileFormat.png:
                    saveformat = "-n";
                    SaveFileType = "png";
                    break;
                case FileFormat.tiff:
                    saveformat = TiffCompression ? "-t1" : "-t";
                    SaveFileType = "tiff";
                    break;
                case FileFormat.jpg:
                default:
                    saveformat = "-j[" + JpgQuality + "]";
                    SaveFileType = "jpg";
                    break;
            }

            string basecmd = " -o \"" + ProjectManager.ImageSavePath + "\" -S " + saveformat + " -c";
            string command = basecmd;
            bool startRT = false;

            #endregion

            OutputWatcher.Filter = "*." + SaveFileType;
            OutputWatcher.Path = ProjectManager.ImageSavePath;
            OutputWatcher.EnableRaisingEvents = true;

            MainWorker.ReportProgress(0, new ProgressChangeEventArgs(0, ProgressType.ProcessingImages));

            #region Run RawTherapee

            //Add all the files to the command and start Rawtherapee
            for (int i = 0; i < Frames.Count; i++)
            {
                if (MainWorker.CancellationPending) { return; }

                //the windows command line can't take longer commands than 8191 characters
                if (command.Length + Frames[i].FilePath.Length < 8191) { command += " \"" + Frames[i].FilePath + "\""; }
                else { startRT = true; i--; }

                if (startRT || i == Frames.Count -1)
                {
                    //Start RT
                    RT.StartInfo.Arguments = command;
                    RT.Start();
                    RT.WaitForExit();   //TODO: newer versions of RT wait for a response when finished->handle that

                    //when finished, set back the command:
                    command = basecmd;
                    startRT = false;
                }
            }

            #endregion

            OutputWatcher.EnableRaisingEvents = false;
        }

        private void DeletePP3()
        {
            for (int i = 0; i < Frames.Count; i++) { FileHandle.DeleteFile(Frames[i].FilePath + ".pp3"); }
        }

        private void OutputWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                int p = Directory.GetFiles(Path.GetDirectoryName(e.FullPath), "*" + Path.GetExtension(e.FullPath)).Length;
                MainWorker.ReportProgress(0, new ProgressChangeEventArgs(p * 100 / Frames.Count, ProgressType.ProcessingImages));
            }
        }

        protected override void SetFrames(string[] files)
        {
            for (int i = 0; i < files.Length; i++) { Frames.Add(new FrameRT(files[i])); }
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Timelapse_API
{
    /// <summary>
    /// Master class which manages the project
    /// </summary>
    public static class ProjectManager
    {
        /// <summary>
        /// The platform this project is running on
        /// </summary>
        public static Platform RunningPlatform { get; private set; }
        /// <summary>
        /// The program that will be used to render the frames
        /// </summary>
        public static ProjectType UsedProgram { get; internal set; }
        /// <summary>
        /// The project where all data is stored
        /// </summary>
        public static Project CurrentProject { get; internal set; }
        /// <summary>
        /// Path, where the rendered images will be saved to. null if not set
        /// </summary>
        public static string ImageSavePath
        {
            get { return CurrentProject.ImageSavePath; }
            set { CurrentProject.ImageSavePath = value; }
        }
		/// <summary>
		/// Path to where the application is located
		/// </summary>
		public static string ApplicationPath {
			get
			{
				switch(RunningPlatform)
				{
					case Platform.MacOSX:
						return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "..", "..", "..");

					default:
						return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
				}
			}
		}
        /// <summary>
        /// Path to the folder where the thumb images are stored
        /// </summary>
		public static string ThumbPath { get { return Path.Combine(ApplicationPath, "Thumbs"); } }
        /// <summary>
        /// The number of threads the API should use
        /// </summary>
        public static int Threadcount;

        /// <summary>
        /// Fires when any progress has been done within a project
        /// </summary>
        public static event ProgressChangeHandler ProgressChanged;
        /// <summary>
        /// Fires when all frames are loaded
        /// </summary>
        public static event WorkFinishedHandler FramesLoaded;
        /// <summary>
        /// Fires when the brightness calculation is finished
        /// </summary>
        public static event WorkFinishedHandler BrightnessCalculated;
        /// <summary>
        /// Fires when work ist done
        /// </summary>
        public static event WorkFinishedHandler WorkDone;

        internal static void OnProgressChanged(object sender, ProgressChangeEventArgs e)
        {
            if (ProgressChanged != null) { ProgressChanged(sender, e); }
        }
        internal static void OnFramesLoaded(object sender, WorkFinishedEventArgs e)
        {
            if (FramesLoaded != null) { FramesLoaded(sender, e); }
        }
        internal static void OnBrightnessCalculated(object sender, WorkFinishedEventArgs e)
        {
            if (BrightnessCalculated != null) { BrightnessCalculated(sender, e); }
        }
        internal static void OnWorkDone(object sender, WorkFinishedEventArgs e)
        {
            if (WorkDone != null) { WorkDone(sender, e); }
        }
        
        /// <summary>
        /// Initiates the project manager
        /// </summary>
        /// <param name="RunningPlatform">The platform this project is running on</param>
        public static void Init(Platform RunningPlatform)
        {
            ProjectManager.RunningPlatform = RunningPlatform;
            NewProject(ProjectType.LapseStudio);
			FileHandle.CreateDirectory(ThumbPath);
			FileHandle.ClearDirectory(ThumbPath, true);
        }
        
        
        /// <summary>
        /// Loads the project from disk
        /// </summary>
        /// <param name="path">Path, from where the project will be loaded</param>
        public static void OpenProject(SavingStorage SavedValues)
        {
            NewProject(SavedValues.UsedProgram);
            CurrentProject.ProjectLoad(SavedValues);
        }

        /// <summary>
        /// Creates a new project
        /// </summary>
        /// <param name="type">The type of the project</param>
        public static void NewProject(ProjectType type)
        {
            UsedProgram = type;
            switch (type)
            {
                case ProjectType.LapseStudio:
                    CurrentProject = new ProjectLS();
                    break;
                case ProjectType.CameraRaw:
                    CurrentProject = new ProjectACR();
                    break;
                case ProjectType.RawTherapee:
                    CurrentProject = new ProjectRT();
                    break;
            }

            CurrentProject.ProgressChanged += OnProgressChanged;
            CurrentProject.FramesLoaded += OnFramesLoaded;
            CurrentProject.BrightnessCalculated += OnBrightnessCalculated;
            CurrentProject.WorkDone += OnWorkDone;
        }
        
        /// <summary>
        /// Calculates the brightness of all frames
        /// </summary>
        public static void CalculateBrightness(BrightnessCalcType CalcType)
        {
            CurrentProject.CalculateBrightness(CalcType);
        }

        /// <summary>
        /// Writes processing data or images to disk
        /// </summary>
        public static void ProcessFiles()
        {
            if ((CurrentProject.Type == ProjectType.LapseStudio && !Directory.Exists(ImageSavePath)) ||
            (CurrentProject.Type == ProjectType.RawTherapee && ((ProjectRT)CurrentProject).RunRT && !Directory.Exists(ImageSavePath))) { throw new DirectoryNotFoundException("Output directory doesn't exist"); }
            CurrentProject.ProcessFiles();
        }

        /// <summary>
        /// Writes processing data or images to disk
        /// </summary>
        /// <param name="outputDirectory">Path to the output directory</param>
        public static void ProcessFiles(string outputDirectory)
        {
            ImageSavePath = outputDirectory;
            Directory.CreateDirectory(outputDirectory);
            ProcessFiles();
        }

        /// <summary>
        /// Sets the AltBrightness for each frame
        /// </summary>
        /// <param name="Keypoints">The keypoints which will be used for interpolation (x has to be between 0 an framecout)</param>
        public static void SetAltBrightness(List<PointD> Keypoints)
        {
            CurrentProject.SetNewBrightness(Keypoints);
        }

        /// <summary>
        /// Process all thumbs for a preview
        /// </summary>
        public static void ProcessThumbs()
        {
            CurrentProject.ProcessThumbs();
        }

        /// <summary>
        /// Cancel a currently running work
        /// </summary>
        public static void Cancel()
        {
            CurrentProject.Cancel();
        }

        /// <summary>
        /// Add all frames from a certain directory to the project
        /// </summary>
        /// <param name="directory">The directory from where the frames will be loaded</param>
        public static bool AddFrames(string directory)
        {
            return CurrentProject.AddFrames(directory);
        }

        /// <summary>
        /// Set a keyframe
        /// </summary>
        /// <param name="index">Index where the keyframe is set</param>
        public static void AddKeyframe(int index)
        {
            CurrentProject.AddKeyframe(index);
        }

        /// <summary>
        /// Set a keyframe
        /// </summary>
        /// <param name="index">Index where the keyframe is set</param>
        /// <param name="path">Path to the metadata file</param>
        public static void AddKeyframe(int index, string path)
        {
            CurrentProject.AddKeyframe(index, path);
        }

        /// <summary>
        /// Removes a keyframe
        /// </summary>
        /// <param name="index">Index where the keyframe will be removed</param>
        /// <param name="removeLink">States if the connection to the metadata file should be removed too</param>
        public static void RemoveKeyframe(int index, bool removeLink)
        {
            CurrentProject.RemoveKeyframe(index, removeLink);
        }

        /// <summary>
        /// Sets the movement of frames in x direction (in percent)
        /// </summary>
        /// <param name="x">The x movement</param>
        public static void SetXMovement(int x)
        {
            for (int i = 1; i < CurrentProject.Frames.Count; i++) { CurrentProject.Frames[i].Xmovement = x; }
        }

        /// <summary>
        /// Sets the movement of frames in y direction (in percent)
        /// </summary>
        /// <param name="x">The y movement</param>
        public static void SetYMovement(int y)
        {
            for (int i = 1; i < CurrentProject.Frames.Count; i++) { CurrentProject.Frames[i].Ymovement = y; }
        }

        /// <summary>
        /// Reads and updates XMP from all frames
        /// </summary>
        public static void ReadXMP()
        {
            if (CurrentProject.GetType() == typeof(ProjectACR)) { ((ProjectACR)CurrentProject).ReadXMP(); }
            else { throw new ProjectTypeException(); }
        }                
    }
}

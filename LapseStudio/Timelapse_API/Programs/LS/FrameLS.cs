using System;

namespace Timelapse_API
{
    [Serializable()]
    public class FrameLS : Frame
    {
        internal double[,] UsageMask;

        /// <summary>
        /// Init a new LapseStudio frame
        /// </summary>
        /// <param name="FilePath">Path to the image file</param>
        public FrameLS(string FilePath) : base(FilePath) { }
    }
}

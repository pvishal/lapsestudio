using System;

namespace Timelapse_API
{
    /// <summary>
    /// A frame for use with RawTherapee
    /// </summary>
    [Serializable()]
    public class FrameRT : Frame
    {
        /// <summary>
        /// The PP3 file related to this frame
        /// </summary>
        public PP3 PP3File
        {
            get { return _PP3File; }
            internal set { _PP3File = value; }
        }

        private PP3 _PP3File;

        /// <summary>
        /// Init a new RawTherapee frame
        /// </summary>
        /// <param name="FilePath">Path to the image file</param>
        public FrameRT(string FilePath) : base(FilePath) { }
       
    }
}

using System;

namespace Timelapse_API
{
    /// <summary>
    /// A frame for use with a CameraRaw project
    /// </summary>
    [Serializable()]
    public class FrameACR : Frame
    {
        /// <summary>
        /// The XMP file related to this frame
        /// </summary>
        public XMP XMPFile
        {
            get { return _XMPFile; }
            internal set { _XMPFile = value; }
        }

        private XMP _XMPFile;

        /// <summary>
        /// Init a new CameraRaw frame
        /// </summary>
        /// <param name="FilePath">Path to the image file</param>
        public FrameACR(string FilePath) : base(FilePath)
        {
            XMPFile = new XMP(FilePath);
        }

    }
}

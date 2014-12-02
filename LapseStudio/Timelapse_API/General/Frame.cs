using System;
using System.IO;
using ColorManager;

namespace Timelapse_API
{
    /// <summary>
    /// Stores and handles all data for a frame
    /// </summary>
    [Serializable()]
    public abstract class Frame
    {
        /// <summary>
        /// The calculated brightness of this frame
        /// </summary>
        public double OriginalBrightness { get; internal set; }
        /// <summary>
        /// The altered brightness
        /// </summary>
        public double AlternativeBrightness;
        /// <summary>
        /// The newly set brightness
        /// </summary>
        public double NewBrightness;
        /// <summary>
        /// Aperture value of this frame
        /// </summary>
        public double Av { get; internal set; }
        /// <summary>
        /// Time value of this frame
        /// </summary>
        public double Tv { get; internal set; }
        /// <summary>
        /// Speed value (ISO) of this frame
        /// </summary>
        public double Sv { get; internal set; }
        /// <summary>
        /// Brightness value of this frame
        /// </summary>
        public double Bv { get; internal set; }

        /// <summary>
        /// Path to the original file
        /// </summary>
        public string FilePath { get; internal set; }
        /// <summary>
        /// The name of the file without extension
        /// </summary>
        public string Filename { get; internal set; }
        /// <summary>
        /// Aperture value as string
        /// </summary>
        public string AVstring { get; internal set; }
        /// <summary>
        /// Time value as string
        /// </summary>
        public string TVstring { get; internal set; }
        /// <summary>
        /// Speed value (ISO) as string
        /// </summary>
        public string SVstring { get; internal set; }

        /// <summary>
        /// Width of original file
        /// </summary>
        public int Width { get; internal set; }
        /// <summary>
        /// Height of original file
        /// </summary>
        public int Height { get; internal set; }

        /// <summary>
        /// X-Movement of frame relative to previous frame (-100% to +100%)
        /// </summary>
        public double Xmovement { get; internal set; }
        /// <summary>
        /// Y-Movement of frame relative to previous frame (-100% to +100%)
        /// </summary>
        public double Ymovement { get; internal set; }

        /// <summary>
        /// States if this frame is a keyframe
        /// </summary>
        public bool IsKeyframe { get; internal set; }
        /// <summary>
        /// States if this frame has metadata
        /// </summary>
        public bool HasMetadata { get; internal set; }

        internal IRGBSpace ColorSpace;
        
        /// <summary>
        /// Init a new frame
        /// </summary>
        /// <param name="FilePath">Path to the image file</param>
        public Frame(string FilePath)
        {
            this.FilePath = FilePath;
            this.Filename = Path.GetFileNameWithoutExtension(FilePath);
        }
    }
}

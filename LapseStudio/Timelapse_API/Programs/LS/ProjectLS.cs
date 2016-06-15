using System;
using System.Collections.Generic;
using System.IO;
using ColorManager;
using System.Drawing;
using System.Drawing.Imaging;

namespace Timelapse_API
{
    /// <summary>
    /// A project for use with LapseStudio
    /// </summary>
    [Serializable()]
    public class ProjectLS : Project
	{
		/// <summary>
		/// Type of project
		/// </summary>
		public override ProjectType Type { get { return ProjectType.LapseStudio; } }

		/// <summary>
		/// Image format in which the images will be saved
		/// </summary>
        public FileFormat SaveFormat;
        /// <summary>
        /// File extensions supported by the project. (e.g. ".jpg")
        /// </summary>
        public override string[] AllowedFileExtensions
        {
            get { return extensions; }
        }

        private static readonly string[] extensions = { ".jpg", ".jpeg", ".png", ".tif", ".tiff" };
        private const double ln2 = 0.69314718055994530941723212145818;


        /// <summary>
        /// Initiates a new project
        /// </summary>
        internal ProjectLS()
            : base()
        { }


        protected override void AddFrames(string[] filepath)
        {
            if (filepath == null) { throw new ArgumentNullException(); }
            MainWorker.RunWorkerAsync(new KeyValuePair<Work, object>(Work.LoadFrames, filepath));
        }

        internal override void AddKeyframe(int index)
        {
            AddKeyframe(index, null);
        }

        internal override void AddKeyframe(int index, string path)
        {
            this.Frames[index].IsKeyframe = true;
        }

        internal override void RemoveKeyframe(int index, bool removeLink)
        {
            this.Frames[index].IsKeyframe = false;
        }
        

        protected override void WriteFiles()
        {
            if (!IsBrightnessCalculated) { throw new RequirementsNotFulfilledException(); }

            for (int i = 0; i < Frames.Count; i++)
            {
                double exposure;
                if (Frames[i].AlternativeBrightness == 0) { exposure = 0; }
                else if (Frames[i].NewBrightness == 0) { exposure = Math.Log(1 / Math.Abs(Frames[i].AlternativeBrightness), 2) * Math.Sign(Frames[i].AlternativeBrightness); }
                else { exposure = Math.Log(Math.Abs(Frames[i].NewBrightness / Frames[i].AlternativeBrightness), 2) * Math.Sign(Frames[i].AlternativeBrightness); }

                Bitmap input = new Bitmap(Frames[i].FilePath);
                string savepath = Path.Combine(ProjectManager.ImageSavePath, Frames[i].Filename + "." + SaveFormat.ToString());
                
                switch (input.PixelFormat)
                {
                    case PixelFormat.Format24bppRgb:
                    case PixelFormat.Format32bppArgb:
                    case PixelFormat.Format32bppRgb:
                        Process8bit(ref input, exposure, Frames[i].ColorSpace).Save(savepath);
                        break;
                    case PixelFormat.Format48bppRgb:
                    case PixelFormat.Format64bppArgb:
                        Process16bit(Frames[i].FilePath, savepath, exposure, Frames[i].ColorSpace);
                        break;
                    default:
                        throw new FormatException("Pixelformat of image is not supported");
                }

                MainWorker.ReportProgress(0, new ProgressChangeEventArgs(i * 100 / (Frames.Count - 1), ProgressType.ProcessingImages));
            }
        }

        private Bitmap Process8bit(ref Bitmap input, double exposure, RGBSpace cspace)
        {
            int index, factor;
            if (input.PixelFormat == PixelFormat.Format24bppRgb) { factor = 3; }
            else { factor = 4; }

            Bitmap output = (Bitmap)input.Clone();
            System.Drawing.Rectangle rec = new System.Drawing.Rectangle(0, 0, input.Width, input.Height);
            BitmapData bmdIn = input.LockBits(rec, ImageLockMode.ReadOnly, input.PixelFormat);
            BitmapData bmdOut = output.LockBits(rec, ImageLockMode.ReadOnly, output.PixelFormat);
            
            unsafe
            {
                for (int y = 0; y < input.Height; y++)
                {
                    byte* row1 = (byte*)bmdIn.Scan0 + (y * bmdIn.Stride);
                    byte* row2 = (byte*)bmdOut.Scan0 + (y * bmdOut.Stride);

                    for (int x = 0; x < input.Width; x++)
                    {
                        index = x * factor;
                        ColorRGB c = new ColorRGB(row1[index + 2], row1[index + 1], row1[index], cspace);
                        BaseProcess(c, exposure);

                        row2[index + 2] = (byte)(c.R * byte.MaxValue);
                        row2[index + 1] = (byte)(c.G * byte.MaxValue);
                        row2[index] = (byte)(c.B * byte.MaxValue);
                    }
                }

            }
            input.UnlockBits(bmdIn);
            output.UnlockBits(bmdOut);
            return output;
        }

        private void Process16bit(string path, string savepath, double exposure, RGBSpace cspace)
        {
            //TODO: make 16bit processing
        }

        private unsafe void BaseProcess(ColorRGB c, double exposure)
        {
            //TODO: Make processing better (highlight and dark preserve)
            fixed (double* pt = c.ValueArray) { ((RGBSpace)c.Space).ToLinear(pt, pt); }
            c.R = Math.Exp(exposure * ln2) * c.R;
            c.G = Math.Exp(exposure * ln2) * c.G;
            c.B = Math.Exp(exposure * ln2) * c.B;
            fixed (double* pt = c.ValueArray) { ((RGBSpace)c.Space).ToNonLinear(pt, pt); }
        }

        protected override void LoadThumbnails(string[] files)
        {
            BitmapEx bmpTmp = null;
            for (int i = 0; i < Frames.Count; i++)
            {
                if (MainWorker.CancellationPending) { return; }

                bmpTmp = new BitmapEx(Frames[i].FilePath).ScaleW(300);
                ProjectManager.CurrentProject.AddThumb(bmpTmp);   //Normal Thumb
                ProjectManager.CurrentProject.AddThumb(bmpTmp);   //Edit Thumb
                SetThumb(i, bmpTmp);
                Frames[i].Width = (int)bmpTmp.Width;
                Frames[i].Height = (int)bmpTmp.Height;
                ProjectManager.CurrentProject.ReportWorkProgress(i, ProgressType.LoadThumbnails);
            }
            if (bmpTmp != null) bmpTmp.Dispose();
        }
    }
}

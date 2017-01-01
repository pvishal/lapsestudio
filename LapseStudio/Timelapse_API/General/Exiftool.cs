using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Timelapse_API
{
    public static class Exiftool
    {
        internal static Process exiftool;
        private const ushort JpgMarker = (ushort)0xFFD8;
        private enum ExifArgument
        {
            Metadata,
            Thumbnails,
        }

        /// <summary>
        /// Reads aperture, shutterspeed, WB levels, colorspace and imagesize of all files within a directory
        /// </summary>
        /// <param name="directory">The directory which will be scanned</param>
        /// <returns>a string array with all values</returns>
        public static string[] GetExifMetadata(string directory)
        {
            SetProcess("-s -ApertureValue -ShutterSpeedValue -ISO -WB_RGGBLevelsAsShot -ColorSpace -ImageSize \"" + directory + "\"", ExifArgument.Metadata);
            exiftool.Start();
            string CameraData = exiftool.StandardOutput.ReadToEnd();
            exiftool.StandardOutput.Close();
            exiftool.WaitForExit();
            exiftool.Dispose();

            if (!String.IsNullOrWhiteSpace(CameraData)) { return CameraData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); }
            else { return null; }
        }

        /// <summary>
        /// Extracts thumbnails from metadata and saves them to thumb directory
        /// </summary>
        /// <param name="directory">The directory which will be scanned</param>
        public static void ExtractThumbnails(string[] files)
		{
			SetProcess ("-q -q -previewImage -b -@ -", ExifArgument.Thumbnails);
			exiftool.Start ();

			byte[] data = Encoding.UTF8.GetBytes (String.Join ("\r\n", files));
			exiftool.StandardInput.BaseStream.Write (data, 0, data.Length);
			exiftool.StandardInput.BaseStream.Close ();

			using(MemoryStream str = new MemoryStream())
            {
				exiftool.StandardOutput.BaseStream.CopyTo (str);
				str.Position = 0;
				List<long> idx = new List<long>();
				byte[] tmp = new byte[2];
				for (long ct = 0; ct < str.Length; ct++)
                {
					str.Position = ct;
					str.Read (tmp, 0, 2);
					if (BitConverter.IsLittleEndian) Array.Reverse (tmp);
					if (BitConverter.ToUInt16 (tmp, 0) == JpgMarker) idx.Add (ct);
                }
                
				if (idx.Count != ProjectManager.CurrentProject.Frames.Count) throw new Exception("Not all images have a thumbnail");

				BitmapEx tmpBmp = null;
				byte[] buffer;
				long length;
				for (int i = 0; i < idx.Count; i++)
                {
					if (ProjectManager.CurrentProject.MainWorker.CancellationPending) break;

					if (i + 1 < idx.Count) length = idx[i + 1] - idx[i];
					else length = str.Length - idx[i];
					buffer = new byte[length];
					str.Position = idx[i];
					str.Read (buffer, 0, (int)length);
					using(MemoryStream str2 = new MemoryStream(buffer))
                    {
						tmpBmp = new BitmapEx(str2).ScaleW (300);
						ProjectManager.CurrentProject.AddThumb (tmpBmp);   //Normal Thumb
						ProjectManager.CurrentProject.AddThumb (tmpBmp);   //Edit Thumb
						ProjectManager.CurrentProject.ReportWorkProgress (i, ProgressType.LoadThumbnails);
					}
                }
				if (tmpBmp != null) tmpBmp.Dispose ();
				buffer = null;
			}
            exiftool.WaitForExit();
            exiftool.Dispose();
        }
        
        /// <summary>
        /// Writes XMP data to a file
        /// </summary>
        /// <param name="filepath">Path to file</param>
        /// <param name="values">XMP values to be written</param>
        public static void WriteXMPMetadata(string filepath, XMP xmp)
        {
            //TODO: make XMP write faster (like thumbnail read)
            SetProcess("", ExifArgument.Metadata);
            string command = String.Empty;
            bool run = false;
            Dictionary<string, XMP.XMPentry> values = xmp.Values;
            CultureInfo culture = new CultureInfo("en-US");

            for (int i = 0; i < values.Count; i++)
            {
                if (ProjectManager.CurrentProject.MainWorker.CancellationPending) { return; }
                XMP.XMPentry entry = values.ElementAt(i).Value;
                string nc = "-xmp:" + entry.Name + "=" + entry.Value.ToString() + " ";
                if (i == 0)
                {
                    nc += "-xmp:ProcessVersion=" + xmp.FileVersion + " ";
                    nc += "-xmp:Exposure2012=" + xmp.NewExposure.ToString("F4", culture) + " ";
                }

                //the windows command line can't take longer commands than 8191 characters
                if (command.Length + nc.Length + filepath.Length < 8191) { command += nc; }
                else { run = true; i--; }

                if (run || i == values.Count - 1)
                {
                    exiftool.StartInfo.Arguments = "-m " + command + "\"" + filepath + "\"";
                    exiftool.Start();
                    exiftool.WaitForExit();
                    run = false;
                }
            }
            exiftool.Dispose();
        }
        
        /// <summary>
        /// Extracts XMP data from a file
        /// </summary>
        /// <param name="filepath">Path to the file</param>
        /// <returns>a string array with all data</returns>
        public static string[] GetXMPMetadata(string filepath)
        {
            SetProcess("-s -XMP-crs:all -xmp:ColorTemperature " + filepath, ExifArgument.Metadata);
            exiftool.Start();
            string data = exiftool.StandardOutput.ReadToEnd();
            exiftool.StandardOutput.Close();
            exiftool.WaitForExit();
            exiftool.Dispose();

            if (String.IsNullOrWhiteSpace(data)) { return null; }
            string[] lines = data.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            return lines;
        }
        
        private static void SetProcess(string command, ExifArgument arg)
        {
            exiftool = new Process();
            string ExiftoolName = "";

			if (ProjectManager.RunningPlatform == Platform.Windows) { ExiftoolName = Path.Combine(Directory.GetCurrentDirectory(), "exiftool.exe"); }
            else if (ProjectManager.RunningPlatform == Platform.Unix || ProjectManager.RunningPlatform == Platform.MacOSX) { ExiftoolName = "/usr/bin/exiftool"; }
            else { throw new PlatformNotSupportedException(); }

            ProcessStartInfo exiftoolStartInfo = new ProcessStartInfo(ExiftoolName, command);
            exiftool.EnableRaisingEvents = false;
            exiftoolStartInfo.UseShellExecute = false;
            exiftoolStartInfo.CreateNoWindow = true;
            exiftoolStartInfo.RedirectStandardError = false;
            exiftoolStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            switch (arg)
            {
                case ExifArgument.Metadata:
                    exiftoolStartInfo.RedirectStandardOutput = true;
                    break;
                case ExifArgument.Thumbnails:
                    exiftoolStartInfo.RedirectStandardOutput = true;
                    exiftoolStartInfo.RedirectStandardInput = true;
                    exiftoolStartInfo.StandardOutputEncoding = Encoding.UTF8;
                    break;
            }
            exiftool.StartInfo = exiftoolStartInfo;
        }
    }
}

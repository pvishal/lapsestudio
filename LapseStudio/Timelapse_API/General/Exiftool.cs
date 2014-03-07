using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Linq;

namespace Timelapse_API
{
    internal static class Exiftool
    {
        internal static Process exiftool;

        /// <summary>
        /// Reads aperture, shutterspeed, WB levels, colorspace and imagesize of all files within a directory
        /// </summary>
        /// <param name="directory">The directory which will be scanned</param>
        /// <returns>a string array with all values</returns>
        public static string[] GetExifMetadata(string directory)
        {
            SetProcess("-s -ApertureValue -ShutterSpeedValue -ISO -WB_RGGBLevelsAsShot -ColorSpace -ImageSize " + directory, true);
            exiftool.Start();
            string CameraData = exiftool.StandardOutput.ReadToEnd();
            exiftool.StandardOutput.Close();
            exiftool.WaitForExit();

            if (!String.IsNullOrWhiteSpace(CameraData)) { return CameraData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); }
            else { return null; }
        }

        /// <summary>
        /// Extracts thumbnails from metadata and saves them to thumb directory
        /// </summary>
        /// <param name="directory">The directory which will be scanned</param>
        public static void ExtractThumbnails(string directory)
        {
            if (directory.EndsWith("\\")) { directory = directory.Substring(0, directory.Length - 1); }
            SetProcess("-thumbnailimage -previewImage -b -w " + "\"" + Path.Combine(ProjectManager.ThumbPath, "%f_Thumb.jpg") + "\" " + "\"" + directory + "\"", true);
            exiftool.Start();
            exiftool.WaitForExit();
        }
        
        /// <summary>
        /// Writes XMP data to a file
        /// </summary>
        /// <param name="filepath">Path to file</param>
        /// <param name="values">XMP values to be written</param>
        public static void WriteXMPMetadata(string filepath, XMP xmp)
        {
            SetProcess("", false);
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
        }
        
        /// <summary>
        /// Extracts XMP data from a file
        /// </summary>
        /// <param name="filepath">Path to the file</param>
        /// <returns>a string array with all data</returns>
        public static string[] GetXMPMetadata(string filepath)
        {
            SetProcess("-s -XMP-crs:all -xmp:ColorTemperature " + filepath, true);
            exiftool.Start();
            string data = exiftool.StandardOutput.ReadToEnd();
            exiftool.StandardOutput.Close();
            exiftool.WaitForExit();

            if (String.IsNullOrWhiteSpace(data)) { return null; }
            string[] lines = data.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            return lines;
        }
        
        private static void SetProcess(string command, bool redirect)
        {
            exiftool = new Process();
            string ExiftoolName = "";

			if (ProjectManager.RunningPlatform == Platform.Windows) { ExiftoolName = Path.Combine(Directory.GetCurrentDirectory(), "exiftool.exe"); }
            else if (ProjectManager.RunningPlatform == Platform.Unix || ProjectManager.RunningPlatform == Platform.MacOSX) { ExiftoolName = "/usr/bin/exiftool"; }
            else { throw new PlatformNotSupportedException(); }

            ProcessStartInfo exiftoolStartInfo = new ProcessStartInfo(ExiftoolName, command);
            exiftoolStartInfo.UseShellExecute = false;
            exiftoolStartInfo.CreateNoWindow = true;
            exiftoolStartInfo.RedirectStandardOutput = redirect;
            exiftool.StartInfo = exiftoolStartInfo;
        }
    }
}

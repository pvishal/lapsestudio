using System.IO;
using System.Threading;

namespace Timelapse_API
{
    internal static class FileHandle
    {
		//TODO: check for upper and lower case file extension

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="Path">Path to the file</param>
        public static void DeleteFile(string Path)
        {
            int c = 0;
            while (File.Exists(Path) && c < 5) { File.Delete(Path); Thread.Sleep(50); c++; }

            if (File.Exists(Path)) { throw new FileDeleteException(); }
        }

        /// <summary>
        /// Deletes a directory
        /// </summary>
        /// <param name="directory">Path to the directory</param>
        public static void DeleteDirectory(string directory)
        {
            int c = 0;
            while (Directory.Exists(directory) && c < 5) { Directory.Delete(directory); Thread.Sleep(50); c++; }

            if (Directory.Exists(directory)) { throw new FileDeleteException(); }
        }

        /// <summary>
        /// Deletes all files of a directory
        /// </summary>
        /// <param name="directory">The path to the directory</param>
        /// <param name="TopDirectoryOnly">Search only in top directory or in subdirectories too</param>
        public static void ClearDirectory(string directory, bool TopDirectoryOnly)
        {
            string[] files = Directory.GetFiles(directory, "*.*", TopDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
            foreach (string file in files) { DeleteFile(file); }
        }

        /// <summary>
        /// Deletes all files of a directory that meet the search pattern
        /// </summary>
        /// <param name="directory">The path to the directory</param>
        /// <param name="SearchPattern">Specifies which files should get deleted (e.g. *.txt)</param>
        /// <param name="TopDirectoryOnly">Search only in top directory or in subdirectories too</param>
        public static void ClearDirectory(string directory, string SearchPattern, bool TopDirectoryOnly)
        {
            string[] files = Directory.GetFiles(directory, SearchPattern, TopDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
            foreach (string file in files) { DeleteFile(file); }
        }

        /// <summary>
        /// Creates a new directory
        /// </summary>
        /// <param name="directory">The path to the directory</param>
        public static void CreateDirectory(string directory)
        {
            int c = 0;
            while (!Directory.Exists(directory) && c < 5) { Directory.CreateDirectory(directory); Thread.Sleep(50); c++; }

            if (!Directory.Exists(directory)) { throw new FileCreateException(); }
        }
    }
}

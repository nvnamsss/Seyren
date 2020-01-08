
using System.IO;

namespace Base2D.Utils
{
    public static class FileUtils
    {
        /// <summary>
        /// Check if a file is ready to use
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileReady(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    FileInfo file = new FileInfo(path);
                    using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }

                    return true;
                }
                catch (IOException)
                {
                    //the file is unavailable because it is:
                    //still being written to
                    //or being processed by another thread
                    //or does not exist (has already been processed)
                    return false;
                }

            }

            return false;
        }
    }
}

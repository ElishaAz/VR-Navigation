using System;
using System.IO;

namespace Log
{
    /// <summary>
    /// A class for writing log to file.
    /// </summary>
    public class LogWriter
    {
        /// <summary>
        /// The path of the log file.
        /// </summary>
        private readonly string path;

        /// <summary>
        /// Create a new log writer.
        /// </summary>
        /// <param name="path">The path of the log file.</param>
        public LogWriter(string path)
        {
            this.path = path;

            if (path == null)
                throw new NullReferenceException("Path cannot be null!");

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }

        /// <summary>
        /// Write a line to the log file.
        /// </summary>
        /// <param name="line">The line to write.</param>
        public void Log(string line)
        {
            using var w = File.AppendText(path);
            w.WriteLine(line);
            w.Flush();
        }
    }
}
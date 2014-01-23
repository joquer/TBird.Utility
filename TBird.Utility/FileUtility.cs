// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileUtility.cs" company="Mr Smarti Pantz">
//     Copyright © Mr. Smarti Pantz LLC 2011, All rights reserved.//   
// </copyright>
// <summary>
//   Defines the FileUtility type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading;

    /// <summary>
    /// A collection of utility routines for working with files and directories.
    /// </summary>
    public static class FileUtility
    {
        /// <summary>
        /// Given a path, checks that this path exists as a directory and not a file and if
        /// it doesn't exist then it creates the full path.  If the directory exists or can
        /// be created as a directory, true is returned otherwise returns false
        /// </summary>
        /// <param name="path">The full path of the directory to check or create</param>
        /// <returns>true if the path exists as a directory or can be created as a directory otherwise false</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "A boolean status is returned to raising the exception.")]
        public static bool CheckCreateDirectory(string path)
        {
            Contract.Requires(!string.IsNullOrEmpty(path));
            bool success = true;
            if (!Directory.Exists(path))
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                {
                    success = false;
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch (Exception)
                    {
                        success = false;
                    }
                }
            }
            return success;
        }

        /// <summary>
        /// This routine will open a file retry opening a file multiple times until maxWaitTime
        /// is reached.  This eliminates the close delay that some of the files seem to have when accessing
        /// files between different threads.  An attempt is made to open the file, if it fails,
        /// the routine waits <code>tryInterval</code> milliseconds to try again, and if it is not opened by
        /// the <code>maxWaitTime</code> seconds has been reached, null is returned.
        /// </summary>
        /// <param name="fileName">The path of the file to open</param>
        /// <param name="maxWaitTime">The maximum number of seconds to wait before giving up</param>
        /// <param name="tryInterval">The number of milliseoconds to wait between open attempts</param>
        /// <returns>A <code>FileStream</code> when the file is successfully opened, or <code>null</code>
        /// if an error occurs of the <code>maxWaitTime</code> is exceeded.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "We don't care about what type of exception is thrown at this point")]
        public static bool WaitForFile(string fileName, int maxWaitTime, int tryInterval)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));
            FileInfo fi = new FileInfo(fileName);
            if (!fi.Exists)
            {
                return false;
            }

            int elapsedTime = 0;
            while (true)
            {
                try
                {
                    using (FileStream stream = fi.Open(FileMode.Open))
                    {
                        return true;
                    }
                }
                catch
                {
                    Thread.Sleep(tryInterval);
                    elapsedTime += tryInterval;
                    if ((int)(elapsedTime / 1000) > maxWaitTime)
                    {
                        return false;
                    }
                }
            }
        }

    }
}

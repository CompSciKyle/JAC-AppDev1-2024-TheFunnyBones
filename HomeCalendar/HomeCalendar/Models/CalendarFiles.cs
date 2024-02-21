using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Calendar
{

    /// <summary>
    /// CalendarFiles class is used to manage the files used in the Calendar project.
    /// </summary>
    public class CalendarFiles
    {
        private static String DefaultSavePath = @"Calendar\";
        private static String DefaultAppData = @"%USERPROFILE%\AppData\Local\";

        // ====================================================================
        // verify that the name of the file, or set the default file, and 
        // is it readable?
        // throws System.IO.FileNotFoundException if file does not exist
        // ====================================================================

        /// <summary>
        /// Verifies the name of the file or sets a default file to read from.
        /// </summary>
        /// <param name="FilePath">The path of the file you want to verify.</param>
        /// <param name="DefaultFileName">The default file name to use if you don't know the file path.</param>
        /// <returns> The verfied file path</returns>
        /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
        /// <example>
        /// <code>
        /// string verifiedFilePath = CalendarFiles.VerifyReadFromFileName(null, "data.txt");
        /// Console.WriteLine($"Verified File Path for Reading: {verifiedFilePath}");
        /// </code>
        /// </example>
        public static String VerifyReadFromFileName(String? FilePath, String DefaultFileName)
        {

            // ---------------------------------------------------------------
            // if file path is not defined, use the default one in AppData
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does FilePath exist?
            // ---------------------------------------------------------------
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("ReadFromFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ----------------------------------------------------------------
            // valid path
            // ----------------------------------------------------------------
            return FilePath;

        }

        // ====================================================================
        // verify that the name of the file, or set the default file, and 
        // is it writable
        // ====================================================================

        /// <summary>
        /// Verifies the filepath or uses the default one from application data if path is not specified, and checks if its writable.
        /// </summary>
        /// <param name="FilePath">The path of the file you want to verify</param>
        /// <param name="DefaultFileName">The default filename to use if you don't know the file path.</param>
        /// <returns>A verfied file path</returns>
        /// <exception cref="Exception">Thrown if the folder doesn't exist where you want it to be saved or if you can write to it</exception>
        /// <example>
        /// <code>
        /// string verfiedFilepath = CalendarFiles.VerifyWriteToFileName(null, "data.txt")
        ///  Console.WriteLine($"Verified File Path for Reading: {verifiedFilePath}");
        /// </code>
        /// </example>
        public static String VerifyWriteToFileName(String? FilePath, String DefaultFileName)
        {
            // ---------------------------------------------------------------
            // if the directory for the path was not specified, then use standard application data
            // directory
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                // create the default appdata directory if it does not already exist
                String tmp = Environment.ExpandEnvironmentVariables(DefaultAppData);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                // create the default Calendar directory in the appdirectory if it does not already exist
                tmp = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does directory where you want to save the file exist?
            // ... this is possible if the user is specifying the file path
            // ---------------------------------------------------------------
            String? folder = Path.GetDirectoryName(FilePath);
            String delme = Path.GetFullPath(FilePath);
            if (!Directory.Exists(folder))
            {
                throw new Exception("SaveToFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ---------------------------------------------------------------
            // can we write to it?
            // ---------------------------------------------------------------
            if (File.Exists(FilePath))
            {
                FileAttributes fileAttr = File.GetAttributes(FilePath);
                if ((fileAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    throw new Exception("SaveToFileException:  FilePath(" + FilePath + ") is read only");
                }
            }

            // ---------------------------------------------------------------
            // valid file path
            // ---------------------------------------------------------------
            return FilePath;

        }



    }
}

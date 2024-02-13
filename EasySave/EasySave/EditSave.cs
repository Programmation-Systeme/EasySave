using System;
using System.IO;

namespace EasySave
{
    internal class EditSave
    {
        /// <summary>
        /// Creates a new save folder and returns routes when saved.
        /// Creates a dedicated folder for each saved folder, ensuring a structured organizational, preventing data overwrite, facilitating folder identification, 
        /// and enhancing data integrity and traceability. 
        /// </summary>
        /// <param name="sourceFolder">The folder chosen by the user to be saved.</param>
        /// <param name="destinationDirectory">The directory where the folder will be saved.</param>
        public static string Create(string sourceFolder, string destinationDirectory)
        {
            // Generate formatted date-time string for unique identifier
            string formattedDateTime = DateTime.Now.ToString("MM-dd-yyyy-h-mm-ss");

            // Form a dynamic path for the folder
            string pathWithId = Path.Combine(destinationDirectory, Path.GetFileName(sourceFolder) + "-" + formattedDateTime);

            // Check if the destination directory already exists
            bool destinationDirectoryExists = Directory.Exists(destinationDirectory);
            if (!destinationDirectoryExists)
                return null;

            try
            {
                // Create the new directory
                Directory.CreateDirectory(pathWithId);

                // Copy the entire source folder to the destination
                Update(sourceFolder, pathWithId);

                return pathWithId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Deletes a specified folder and returns true when deleted.
        /// </summary>
        /// <param name="destinationFolder">The folder to be deleted.</param>
        public static bool Delete(string destinationFolder)
        {
            // Check if the folder exists
            bool folderExists = Directory.Exists(destinationFolder);

            try
            {
                // Delete the folder if it exists
                if (folderExists)
                {
                    Directory.Delete(destinationFolder, true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Copies the entire contents of a directory to another directory.
        /// </summary>
        /// <param name="sourceDir">The source directory to copy from.</param>
        /// <param name="destDir">The destination directory to copy to.</param>
        public static void Update(string sourceDir, string destDir)
        {
            // Get the subdirectories for the specified directory
            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDir);

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            // Get the files in the directory and copy them to the new location
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDir, file.Name);
                file.CopyTo(tempPath, false);
            }

            // Copy subdirectories and their contents to the new location
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDir, subdir.Name);
                Update(subdir.FullName, tempPath);
            }
        }
    }
}

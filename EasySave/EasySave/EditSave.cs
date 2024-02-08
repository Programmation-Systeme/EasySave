using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave
{
    internal class EditSave
    {
        /// <summary>
        /// Creates a new save file and return true when saved.
        /// </summary>
        /// <param name="sourceFile">The file chosen by the user to be saved.</param>
        /// <param name="destinationDirectory">The directory where the data will be saved.</param>
        public static string Create(string sourceFile, string destinationDirectory)
        {
            // Generate formatted date-time string for unique identifier
            string formattedDateTime = DateTime.Now.ToString("MM-dd-yyyy-h-mm-ss");

            // TO KNOW FOR LATER, ID GENERATION: Guid.NewGuid().ToString("N")
            // Form a dynamic path for the file
            string pathWithId = Path.Combine(destinationDirectory, System.IO.Path.ChangeExtension(Path.GetFileName(sourceFile), null) + "-" + formattedDateTime);

            // Check if the directory already exists
            bool directoryExists = Directory.Exists(pathWithId);
            bool sourceFileExists = File.Exists(sourceFile);
            bool destinationDirectoryExists = Directory.Exists(destinationDirectory);

            // TODO: destinationFile HAVE TO BE SAVED IN THE DB
            // Form the destination file path
            string destinationFile = Path.Combine(pathWithId, Path.GetFileName(sourceFile));
            try
            {
                // We ensure that the future directory we're creating is feasible and verify that files and the destination directory are defined.
                if (directoryExists == false && sourceFileExists && destinationDirectoryExists)
                {
                    // Create a new directory and copy the file
                    System.IO.Directory.CreateDirectory(pathWithId);
                    System.IO.File.Copy(sourceFile, destinationFile);
                    return destinationFile;
                }
                else
                {
                    return null;
                }
            }
            catch (IOException iox)
            {
                return null;
            }
        }
        /// <summary>
        /// Deletes a specified file and return true when deleted.
        /// </summary>
        /// <param name="destinationFile">The file to be deleted.</param>
        public static bool Delete(string destinationFile)
        {
            // Check if the file exists
            bool fileExists = File.Exists(destinationFile);

            // Get the directory path of the file
            string directoryPath = Path.GetDirectoryName(destinationFile);

            try
            {
                // We check if file and directory exist
                if (fileExists == true && Path.Exists(directoryPath))
                {
                    // Delete the file and its containing directory
                    File.Delete(destinationFile);
                    Directory.Delete(directoryPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (IOException iox)
            {
                return false;
            }
        }

        /// <summary>
        /// Updates an existing file with a new one and return true when updated.
        /// </summary>
        /// <param name="sourceFile">The file chosen by the user.</param>
        /// <param name="destinationFile">The file to be updated.</param>
        public static bool Update(string sourceFile, string destinationFile)
        {
            // Check if the destination file exists
            bool fileExists = File.Exists(destinationFile);
            string directoryPath = Path.GetDirectoryName(destinationFile);
            string path = Path.Combine(directoryPath, System.IO.Path.ChangeExtension(Path.GetFileName(sourceFile), null));

            try
            {
                // We check if file and directory exist
                if (fileExists == true && Path.Exists(directoryPath))
                {
                    // Delete the old file and copy the new one
                    File.Delete(destinationFile);
                    System.IO.File.Copy(sourceFile, destinationFile);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (IOException iox)
            {
                return false;
            }
        }
    }
}

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
        public static bool Create(string sourceFile, string destinationDirectory)
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
                if (directoryExists == false && sourceFileExists && destinationDirectoryExists)
                {
                    // Create a new directory and copy the file
                    System.IO.Directory.CreateDirectory(pathWithId);
                    System.IO.File.Copy(sourceFile, destinationFile);
                    Console.WriteLine("File copied successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Already exist");
                    return false;
                }
            }
            catch (IOException iox)
            {
                Console.WriteLine("Error: " + iox.Message);
                return false;
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
            Console.WriteLine("directoryPath: " + directoryPath);

            try
            {
                if (fileExists == true && Path.Exists(directoryPath))
                {
                    // Delete the file and its containing directory
                    File.Delete(destinationFile);
                    Directory.Delete(directoryPath);
                    Console.WriteLine("File deleted successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine("File do not exist");
                    return false;
                }
            }
            catch (IOException iox)
            {
                Console.WriteLine("Error: " + iox.Message);
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
                if (fileExists == true && Path.Exists(directoryPath))
                {
                    // Delete the old file and copy the new one
                    File.Delete(destinationFile);
                    System.IO.File.Copy(sourceFile, destinationFile);
                    Console.WriteLine("File updated successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine("File do not exist");
                    return false;
                }
            }
            catch (IOException iox)
            {
                Console.WriteLine("Error: " + iox.Message);
                return false;
            }
        }
    }
}

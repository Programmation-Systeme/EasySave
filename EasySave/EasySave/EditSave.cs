using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave
{
    internal class EditSave
    {
        public static void Create(string sourceFile, string destinationDirectory)
        {
            // SourceFile is the file that choose the user to be saved
            // DestinationDirectory is the directory where we save our data
            
            string formattedDateTime = DateTime.Now.ToString("MM-dd-yyyy-h-mm-ss");
            Console.WriteLine("DATE: " + formattedDateTime);
            // Guid.NewGuid().ToString("N")
            
            // We create a dynamic name for our file
            string pathWithId = Path.Combine(destinationDirectory, System.IO.Path.ChangeExtension(Path.GetFileName(sourceFile), null) + "-" + formattedDateTime);
            Console.WriteLine("PATH WITH ID" + pathWithId);
            // We check if a file already exists
            bool directoryExists = Directory.Exists(pathWithId);

            Console.WriteLine("Already exist: " + directoryExists);

            // TODO: destinationFile HAVE TO BE SAVED IN THE DB
            string destinationFile = Path.Combine(pathWithId, Path.GetFileName(sourceFile));

            Console.WriteLine("FINAL DEST" + destinationFile);
            // We create firstly a new directory on the storage and then we save the file of the user on this directoy with copy
            try
            {
                if (directoryExists == false)
                {
                    System.IO.Directory.CreateDirectory(pathWithId);
                    System.IO.File.Copy(sourceFile, destinationFile);
                    Console.WriteLine("File copied successfully.");
                }
                else
                {
                    Console.WriteLine("Already exist");
                }
            }
            catch (IOException iox)
            {
                Console.WriteLine("Error: " + iox.Message);
            }
        }

        public static void Delete(string destinationFile)
        {
            // Destination file is the file that we want to delete
            bool fileExists = File.Exists(destinationFile);

            // We get the path of the current directory where the file is located
            string directoryPath = Path.GetDirectoryName(destinationFile);
            Console.WriteLine("directoryPath: " + directoryPath);

            try
            {
                // We delete the file file and then the directory
                if (fileExists == true && Path.Exists(directoryPath))
                {
                    File.Delete(destinationFile);
                    Directory.Delete(directoryPath);
                    Console.WriteLine("File deleted successfully.");
                }
                else
                {
                    Console.WriteLine("File do not exist");
                }
            }
            catch (IOException iox)
            {
                Console.WriteLine("Error: " + iox.Message);
            }
        }

        public static void Update(string sourceFile, string destinationFile)
        {
            // SourceFile is the file that choose the user
            // Destination file is the file that we are going to update with the new sourceFile

            bool fileExists = File.Exists(destinationFile);
            string directoryPath = Path.GetDirectoryName(destinationFile);
            Console.WriteLine("directoryPath: " + directoryPath);

            string path = Path.Combine(directoryPath, System.IO.Path.ChangeExtension(Path.GetFileName(sourceFile), null));

            try
            {
                if (fileExists == true && Path.Exists(directoryPath))
                {
                    File.Delete(destinationFile);
                    System.IO.File.Copy(sourceFile, destinationFile);

                    Console.WriteLine("File updated successfully.");
                }
                else
                {
                    Console.WriteLine("File do not exist");
                }
            }
            catch (IOException iox)
            {
                Console.WriteLine("Error: " + iox.Message);
            }
        }
    }
}

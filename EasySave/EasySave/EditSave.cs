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
            string formattedDateTime = DateTime.Now.ToString("MM-dd-yyyy-h-mm-ss");
            Console.WriteLine("DATE: " + formattedDateTime);
            // Guid.NewGuid().ToString("N")
            string pathWithId = Path.Combine(destinationDirectory, System.IO.Path.ChangeExtension(Path.GetFileName(sourceFile), null) + "-" + formattedDateTime);
            Console.WriteLine("PATH WITH ID" + pathWithId);
            bool directoryExists = Directory.Exists(pathWithId);

            Console.WriteLine("Already exist: " + directoryExists);

            // TODO: destinationFile HAVE TO BE SAVED IN THE DB
            string destinationFile = Path.Combine(pathWithId, Path.GetFileName(sourceFile));

            Console.WriteLine("FINAL DEST" + destinationFile);
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
            bool fileExists = File.Exists(destinationFile);
            string directoryPath = Path.GetDirectoryName(destinationFile);
            Console.WriteLine("directoryPath: " + directoryPath);

            try
            {
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
    }
}

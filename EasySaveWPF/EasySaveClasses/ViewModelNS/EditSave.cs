using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;

namespace EasySaveClasses.ViewModelNS
{
    public class EditSave
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
        /// And we check if files had been change to update them
        /// </summary>
        /// <param name="sourceDir">The source directory to copy from.</param>
        /// <param name="destDir">The destination directory to copy to.</param>
        public static bool Update(string sourceDir, string destDir)
        {
            List<String> listFiles = ["fichier1.txt", "fichier2.png", "fichier3.pdf"];
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../EasySaveClasses/ViewModelNS/Config.json");
            List<string> listExt = new List<string>();
            if (File.Exists(configPath))
            {
                string jsonContent = File.ReadAllText(configPath);

                dynamic jsonObject = JsonConvert.DeserializeObject(jsonContent);
                dynamic firstConfig = jsonObject[0];

             
                if (firstConfig.ExtensionCryptage != null)
                {
                    foreach (var extension in firstConfig.ExtensionCryptage)
                    {
                        listExt.Add(extension.ToString());
                    }
                }
                List<string> filteredFiles = listFiles.Where(file => listExt.Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase))).ToList();
                string CryptoSoftPath = "../../../../../CryptoSoft/CryptoSoft/bin/Debug/net8.0/CryptoSoft.exe";
                string FilteredFilesPath = "";
                foreach (string fileCrypt in filteredFiles)
                {
                    FilteredFilesPath += "\"" + fileCrypt + "\" ";
                }
                string command = CryptoSoftPath + " -e " + FilteredFilesPath;
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",         
                    RedirectStandardOutput = true,
                    UseShellExecute = false,      
                    CreateNoWindow = true         
                };

                // Cr�er et d�marrer le processus
                using (Process process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    process.StartInfo.Arguments = "/c " + command; // "/c" est utilis� pour ex�cuter la commande puis fermer le terminal
                    process.Start();

                    // Lire et afficher la sortie du processus
                    string output = process.StandardOutput.ReadToEnd();
                    
                }
            }

            try
            {
                // Get the subdirectories for the specified directory
                DirectoryInfo sourceDirInfo = new DirectoryInfo(sourceDir);

                if (!sourceDirInfo.Exists)
                    throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDir);

                DirectoryInfo[] sourceSubDirs = sourceDirInfo.GetDirectories();
                // If the destination directory doesn't exist, create it
                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);

                // Get the files in the source directory
                FileInfo[] sourceFiles = sourceDirInfo.GetFiles();
                foreach (FileInfo sourceFile in sourceFiles)
                {
                    string destFilePath = Path.Combine(destDir, sourceFile.Name);

                    // Check if the corresponding file exists in the destination directory
                    if (File.Exists(destFilePath))
                    {
                        // Check if the source file is deleted
                        if (!File.Exists(sourceFile.FullName))
                        {
                            // If the source file is deleted, delete the corresponding file in the destination directory
                            File.Delete(destFilePath);
                            continue; // Move to the next file
                        }

                        // Compare metadata (last write time) of source and destination files
                        DateTime sourceLastWriteTime = sourceFile.LastWriteTime;
                        DateTime destLastWriteTime = File.GetLastWriteTime(destFilePath);

                        // If the metadata differs, update the destination file with the source file
                        if (sourceLastWriteTime != destLastWriteTime)
                        {
                            sourceFile.CopyTo(destFilePath, true); // Overwrite existing file
                        }
                    }
                    else
                    {
                        // Destination file doesn't exist, just copy the source file
                        sourceFile.CopyTo(destFilePath);
                    }
                }

                // Update subdirectories and their contents recursively
                foreach (DirectoryInfo sourceSubDir in sourceSubDirs)
                {
                    string destSubDirPath = Path.Combine(destDir, sourceSubDir.Name);
                    Update(sourceSubDir.FullName, destSubDirPath);
                }

                return true;
            }    
            catch (Exception ex)
            {
                // Handle the exception (you might want to log it or perform other actions)
                Console.WriteLine("An error occurred: " + ex.Message);
                return false; // Return false indicating error
            }
        }
    }
}

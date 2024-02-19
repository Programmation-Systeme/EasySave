using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;

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
        public static string Create(string sourceFolder, string destinationDirectory, int saveType)
        {
            // Generate formatted date-time string for unique identifier
            string formattedDateTime = DateTime.Now.ToString("MM-dd-yyyy-h-mm-ss");
            if (sourceFolder == null || destinationDirectory == null)
            { return "source directory or target directory unselected"; }
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
                Update(sourceFolder, pathWithId, saveType);
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
        public static bool Update(string sourceDir, string destDir, int saveType)
        {
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

                DirectoryInfo destDirInfo = new DirectoryInfo(destDir);

                // Get the files in the source directory
                FileInfo[] sourceFiles = sourceDirInfo.GetFiles();
                FileInfo[] destFiles = destDirInfo.GetFiles();

                List<string> sourceFilesNames = [];

                foreach (FileInfo sourceFile in sourceFiles)
                {
                    sourceFilesNames.Add(sourceFile.Name);
                }

                foreach (FileInfo destFile in destFiles)
                {
                    if (!sourceFilesNames.Contains(destFile.Name))
                    {
                        File.Delete(destFile.FullName);
                    }
                }

                foreach (FileInfo sourceFile in sourceFiles)
                {
                    string destFilePath = Path.Combine(destDir, sourceFile.Name);

                    // Full save
                    if (saveType == 1)
                    {
                        sourceFile.CopyTo(destFilePath, true);
                    }
                    // Differential save
                    else if (saveType == 2)
                    {
                        // Compare metadata (last write time) of source and destination files
                        DateTime sourceLastWriteTime = sourceFile.LastWriteTime;
                        DateTime destLastWriteTime = File.GetLastWriteTime(destFilePath);

                        // If the metadata differs, update the destination file with the source file
                        if (sourceLastWriteTime != destLastWriteTime)
                        {
                            sourceFile.CopyTo(destFilePath, true); // Overwrite existing file
                        }
                    }
                    // If saveType is not full or differential, error
                    else
                    {
                        return false;
                    }
                }
                destDirInfo = new DirectoryInfo(destDir);
                DirectoryInfo[] destSubDirs = destDirInfo.GetDirectories();
                destFiles = destDirInfo.GetFiles();
                // encrypt
                List<string> listFilesPath = [];
                foreach (FileInfo file in destFiles)
                {
                    listFilesPath.Add(file.FullName);
                }
                EncryptFiles(listFilesPath);

                // Update subdirectories and their contents recursively
                foreach (DirectoryInfo sourceSubDir in sourceSubDirs)
                {
                    string destSubDirPath = Path.Combine(destDir, sourceSubDir.Name);
                    Update(sourceSubDir.FullName, destSubDirPath, saveType);
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

        private static void EncryptFiles(List<string> listFilesPath)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../EasySaveClasses/ViewModelNS/Config.json");
            List<string> listExt = [];
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
                List<string> filteredFiles = listFilesPath.Where(file => listExt.Any(ext => IsFileWithExtension(file, ext))).ToList();
                string CryptoSoftPath = "../../../../../CryptoSoft/CryptoSoft/bin/Debug/net8.0";
                string FilteredFilesPath = "";
                foreach (string fileCrypt in filteredFiles)
                {
                    FilteredFilesPath += fileCrypt + " ";
                }

                string command = "/c cd " + CryptoSoftPath + " && CryptoSoft.exe -e " + FilteredFilesPath;
                Process process2 = new Process();
                process2.StartInfo.FileName = "cmd.exe";
                process2.StartInfo.Arguments = command;
                process2.StartInfo.RedirectStandardOutput = true;
                process2.StartInfo.RedirectStandardError = true;
                process2.StartInfo.CreateNoWindow = true;
                process2.Start();
                // Lire la sortie standard
                string output2 = process2.StandardOutput.ReadToEnd();
                string error2 = process2.StandardError.ReadToEnd();
                process2.WaitForExit();
                // Afficher la sortie
                Console.WriteLine(output2);
            }
        }

        private static bool IsFileWithExtension(string filePath, string extension)
        {
            string fileExtension = Path.GetExtension(filePath);
            return string.Equals(fileExtension, extension, StringComparison.OrdinalIgnoreCase);
        }
    }
}

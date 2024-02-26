using EasySaveClasses.ModelNS;
using Newtonsoft.Json;
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
        public static string Create(string sourceFolder, string destinationDirectory, int saveType)
        {
            if (sourceFolder == null || destinationDirectory == null || saveType == 0 || !Directory.Exists(destinationDirectory))
            { return ""; }

            try
            {
                // Generate formatted date-time string for unique identifier
                string formattedDateTime = DateTime.Now.ToString("MM-dd-yyyy-h-mm-ss");

                // Form a dynamic path for the folder
                string pathWithId = Path.Combine(destinationDirectory, Path.GetFileName(sourceFolder) + "-" + formattedDateTime);

                // Create the new directory
                Directory.CreateDirectory(pathWithId);

                // Copy the entire source folder to the destination
                Update(sourceFolder, pathWithId, saveType);
                return pathWithId;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Deletes a specified folder and returns true when deleted.
        /// </summary>
        /// <param name="destinationFolder">The folder to be deleted.</param>
        public static bool Delete(string destinationFolder)
        {
            try
            {
                // Delete the folder if it exists
                if (Directory.Exists(destinationFolder))
                {
                    Directory.Delete(destinationFolder, true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private static bool wait_abortThread(ManualResetEvent manualEvent, CancellationTokenSource cancelEvent) {

            WaitHandle.WaitAny(new WaitHandle[] { manualEvent, cancelEvent.Token.WaitHandle });
            if (cancelEvent.Token.IsCancellationRequested)
            {
                return true;
            }
            return false;

        }

        public class ResultUpdate
        {
            public bool Success { get; set; }
            public int Progression { get; set; }
        }
        /// <summary>
        /// Copies the entire contents of a directory to another directory.
        /// And we check if files had been change to update them
        /// </summary>
        /// <param name="sourceDir">The source directory to copy from.</param>
        /// <param name="destDir">The destination directory to copy to.</param>
        public static ResultUpdate Update(string sourceDir, string destDir, int saveType, ManualResetEvent manualEvent, CancellationTokenSource cancelEvent)
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
                    if(wait_abortThread(manualEvent, cancelEvent)) return new ResultUpdate { Success = false, Progression = 2 }; ;
                    if (!sourceFilesNames.Contains(destFile.Name))
                    {
                        File.Delete(destFile.FullName);
                    }
                }
                int i = 0;
                foreach (FileInfo sourceFile in sourceFiles)
                {
                    i++;
                    if (wait_abortThread(manualEvent, cancelEvent)) return new ResultUpdate { Success = false, Progression = i * 30/ sourceFiles.Length };

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
                        return new ResultUpdate { Success = false, Progression = i * 30 / sourceFiles.Length };  
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
                int y = 0;
                // Update subdirectories and their contents recursively
                foreach (DirectoryInfo sourceSubDir in sourceSubDirs)
                {
                    y++;
                    if (wait_abortThread(manualEvent, cancelEvent)) return new ResultUpdate { Success = false, Progression = 30 + y * 70 / sourceFiles.Length };
                    string destSubDirPath = Path.Combine(destDir, sourceSubDir.Name);
                    Update(sourceSubDir.FullName, destSubDirPath, saveType, manualEvent, cancelEvent);
                }
                return new ResultUpdate { Success = true, Progression = 100 };
            }
            catch
            {
                // Handle the exception (you might want to log it or perform other actions)
                return new ResultUpdate { Success = false, Progression = 0 }; // Return false indicating error
            }
        }



        /// <summary>
        /// ///////////////////////
        /// </summary>
        /// <param name=""></param>
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

                DirectoryInfo destDirInfo = new(destDir);

                // Get the files in the source directory
                FileInfo[] sourceFiles = sourceDirInfo.GetFiles();
                FileInfo[] destFiles = destDirInfo.GetFiles();

                List<string> sourceFilesNames = [];

                // Adding to a list the name of each file
                foreach (FileInfo sourceFile in sourceFiles)
                {
                    sourceFilesNames.Add(sourceFile.Name);
                }



                // Deleting every file that is not in source directory
                foreach (FileInfo destFile in destFiles)
                {

                    if(destFile.Extension == ".hash" || destFile.Extension == ".encrypted")
                    {
                        FileInfo? correspondingSourceFile = sourceFiles.FirstOrDefault(sourceFile => sourceFile.Name == Path.GetFileNameWithoutExtension(destFile.FullName));
                        
                        if(correspondingSourceFile == null)
                        {
                            destFile.Delete();
                        }
                    }
                    else if(!sourceFilesNames.Contains(destFile.Name))
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
                        if(File.Exists(destFilePath + ".encrypted"))
                        {
                            if(File.Exists(destFilePath + ".hash"))
                            {
                                string sourceFileHash = File.ReadAllText(destFilePath + ".hash");
                                if (sourceFileHash != CalculateFileHash(sourceFile.FullName))
                                {
                                    File.Delete(destFilePath + ".hash");
                                    File.Delete(destFilePath + ".encrypted");
                                    sourceFile.CopyTo(destFilePath, true);
                                }
                            }
                            else
                            {
                                File.Delete(destFilePath + ".encrypted");
                            }
                        }
                        else
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

                // encryption
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
            catch
            {
                // Handle the exception (you might want to log it or perform other actions)
                return false; // Return false indicating error
            }
        }

        private static List<string> ReadExtensions()
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
            }
            return listExt;
        }

        private static void EncryptFiles(List<string> listFilesPath)
        {
            List<string> listExt = ReadExtensions();

            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../EasySaveClasses/ViewModelNS/Config.json");
            if (File.Exists(configPath))
            {
                
                List<string> filteredFiles = listFilesPath.Where(file => listExt.Any(ext => IsFileWithExtension(file, ext))).ToList();
                string CryptoSoftPath = "../../../../../CryptoSoft/CryptoSoft/bin/Debug/net8.0";
                string FilteredFilesPath = "";
                foreach (string fileCrypt in filteredFiles)
                {
                    FilteredFilesPath += fileCrypt + " ";
                    File.WriteAllText(fileCrypt + ".hash", CalculateFileHash(fileCrypt));
                    File.SetAttributes(fileCrypt + ".hash", File.GetAttributes(fileCrypt + ".hash") | FileAttributes.Hidden);
                }

                string command = "/c cd " + CryptoSoftPath + " && CryptoSoft.exe -e " + FilteredFilesPath;
                Process process = new();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = command;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                // Read response from process
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
        }

        private static bool IsFileWithExtension(string filePath, string extension)
        {
            return Path.GetExtension(filePath).Equals(extension, StringComparison.OrdinalIgnoreCase);
        }

        static string CalculateFileHash(string filePath)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            using var stream = File.OpenRead(filePath);
            byte[] hashBytes = sha256.ComputeHash(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}

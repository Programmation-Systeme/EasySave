using EasySaveClasses.ModelNS;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace EasySaveClasses.ViewModelNS
{
    public class ResultUpdate
    {
        /// <summary>
        /// True if update is successfull and false otherwise.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The actual progression of the update operation.
        /// </summary>
        public int Progression { get; set; }
    }

    public class EditSave
    {
        /// <summary>
        /// Deletes a specified folder and returns true when deleted.
        /// </summary>
        /// <param name="destinationFolder">The path of the folder to be deleted.</param>
        /// <returns>True if folder has been deleted, false if not.</returns>
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

        /// <summary>
        /// Checks if the pause or cancellation was requested by the user.
        /// </summary>
        /// <param name="manualEvent"></param>
        /// <param name="cancelEvent"></param>
        /// <returns></returns>
        private static bool Wait_AbortThread(ManualResetEvent manualEvent, CancellationTokenSource cancelEvent) {

            WaitHandle.WaitAny(new WaitHandle[] { manualEvent, cancelEvent.Token.WaitHandle });
            if (cancelEvent.Token.IsCancellationRequested)
            {
                return true;
            }
            return false;

        }

        
        /// <summary>
        /// Copies the entire contents of a directory to another directory.
        /// And we check if files had been change to update them
        /// </summary>
        /// <param name="sourceDir">The source directory to copy from.</param>
        /// <param name="destDir">The destination directory to copy to.</param>
        public static ResultUpdate Update(string sourceDir, string destDir, int saveType, ref int totalEncryptionTime, ManualResetEvent manualEvent, CancellationTokenSource cancelEvent)
        {
            try
            {
                // Get the subdirectories for the specified directory
                DirectoryInfo sourceDirInfo = new DirectoryInfo(sourceDir);

                DirectoryInfo[] sourceSubDirs = sourceDirInfo.GetDirectories();
                // If the destination directory doesn't exist, create it
                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);

                DirectoryInfo destDirInfo = new DirectoryInfo(destDir);

                // Get the files from the source directory
                FileInfo[] sourceFiles = sourceDirInfo.GetFiles();

                // Get the files from the destination directory
                FileInfo[] destFiles = destDirInfo.GetFiles();

                // Adding to a list the name of each file
                List<string> sourceFilesNames = [];
                foreach (FileInfo sourceFile in sourceFiles)
                {
                    sourceFilesNames.Add(sourceFile.Name);
                }

                // Deleting every file that is not in source directory or for encrypted files, those whose source file hash has changed (cleaning of the destination file)
                foreach (FileInfo destFile in destFiles)
                {
                    if(Wait_AbortThread(manualEvent, cancelEvent)) return new ResultUpdate { Success = false, Progression = 2 }; ;

                    //Operation for encrypted/hash files
                    if (destFile.Extension == ".hash" || destFile.Extension == ".encrypted")
                    {
                        //If the source file associated with the encrypted file/hash file does not exist, the encrypted file/hash file is deleted.
                        FileInfo? correspondingSourceFile = sourceFiles.FirstOrDefault(sourceFile => sourceFile.Name == Path.GetFileNameWithoutExtension(destFile.FullName));
                        if (correspondingSourceFile == null)
                        {
                            destFile.Delete();
                        }
                    }
                    // Otherwise we delete the file from the destination folder if its name is not the name of a file in the source folder.
                    else if (!sourceFilesNames.Contains(destFile.Name))
                    {
                        File.Delete(destFile.FullName);
                    }
                }

                //temp
                List<string> tempExtPriority = [".xlsx"];
                //

                // Get a list of all files from destination directory, filtered by the priorities of extensions defined in configuration.
                List<FileInfo> filteredSourceFiles = GetFilteredFilesByPriority(sourceFiles, tempExtPriority);

                // Copy files from source folder to destination folder
                int i = 0;
                foreach (FileInfo sourceFile in filteredSourceFiles)
                {
                    i++;
                    if (Wait_AbortThread(manualEvent, cancelEvent)) return new ResultUpdate { Success = false, Progression = i * 30/ sourceFiles.Length };

                    string destFilePath = Path.Combine(destDir, sourceFile.Name);

                    // Full save
                    if (saveType == 1)
                    {
                        sourceFile.CopyTo(destFilePath, true);
                    }
                    // Differential save
                    else if (saveType == 2)
                    {
                        DifferentialSave(destFilePath, sourceFile);
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
                
                // Encryption operation on all files
                List<string> listFilesPath = [];
                foreach (FileInfo file in destFiles)
                {
                    listFilesPath.Add(file.FullName);
                }
                EncryptFiles(listFilesPath, ref totalEncryptionTime);

                // Update subdirectories and their contents recursively
                int y = 0;
                foreach (DirectoryInfo sourceSubDir in sourceSubDirs)
                {
                    y++;
                    if (Wait_AbortThread(manualEvent, cancelEvent)) return new ResultUpdate { Success = false, Progression = 30 + y * 70 / sourceFiles.Length };
                    string destSubDirPath = Path.Combine(destDir, sourceSubDir.Name);
                    Update(sourceSubDir.FullName, destSubDirPath, saveType, ref totalEncryptionTime, manualEvent, cancelEvent); ;
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
        /// Returns files filtered by extension priorities.
        /// </summary>
        /// <param name="sourceFiles"></param>
        /// <param name="extensionsPriority"></param>
        /// <returns>Returns the list of filtered files.</returns>
        private static List<FileInfo> GetFilteredFilesByPriority(FileInfo[] sourceFiles, List<string> extensionsPriority)
        {
            List<FileInfo> filteredSourceFiles = [];
            List<FileInfo> othersSourceFiles = [];

            // Separation of all source files into 2 lists
            foreach (FileInfo sourceFile in sourceFiles)
            {
                // A list of priority files
                if (extensionsPriority.Contains(sourceFile.Extension))
                {
                    filteredSourceFiles.Add(sourceFile);
                }
                // A list of non-priority files
                else
                {
                    othersSourceFiles.Add(sourceFile);
                }
            }
            // Merging the 2 lists
            filteredSourceFiles.AddRange(othersSourceFiles);
            return filteredSourceFiles;
        }

        /// <summary>
        /// Make a differential backup taking into account encrypted files.
        /// </summary>
        /// <param name="destFilePath">The path to which the file should be copied.</param>
        /// <param name="sourceFile">The file to be copied.</param>
        private static void DifferentialSave(string destFilePath, FileInfo sourceFile)
        {
            // Management of encrypted files
            if (File.Exists(destFilePath + ".encrypted"))
            {
                if (File.Exists(destFilePath + ".hash"))
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
            // Management of others files
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

        /// <summary>
        /// Read the extensions to encrypt from the json configuration file and return the list of extensions.
        /// </summary>
        /// <returns>The list of extensions</returns>
        private static List<string> ReadExtensionsForEncryptionFromJson()
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

        private static void InsertExtensions(string newExt)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../EasySaveClasses/ViewModelNS/Config.json");
            // Load the JSON
            string json = File.ReadAllText(configPath);

            dynamic configuration = JsonConvert.DeserializeObject(json);

            // Verify if "ExtensionCryptage" exist in the Json
            if (configuration[0]["ExtensionCryptage"] == null)
            {
                //configuration[0]["ExtensionCryptage"] = new JArray();
            }

            // Add the new extension
            configuration[0]["ExtensionCryptage"].Add(newExt);

            string nouveauJson = JsonConvert.SerializeObject(configuration, Formatting.Indented);

            // Write the new Json file with the new changements
            File.WriteAllText(configPath, nouveauJson);
        }

	/// <summary>
        /// Filters by extensions the files to be encrypted and creates an instance of CryptoSoft to encrypt the files.
        /// </summary>
        /// <param name="listFilesPath">Files that need to be filtered by extension before encryption</param>
        private static void EncryptFiles(List<string> listFilesPath, ref int totalEncryptionTime)
        {
            List<string> listExt = ReadExtensionsForEncryptionFromJson();

            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../EasySaveClasses/ViewModelNS/Config.json");
            if (File.Exists(configPath))
            {
                List<string> filteredFilesToEncrypt = listFilesPath.Where(file => listExt.Any(ext => IsFileWithExtension(file, ext))).ToList();
                string CryptoSoftPath = "../../../../../CryptoSoft/CryptoSoft/bin/Debug/net8.0";
                string FilteredFilesPath = "";
                foreach (string fileCrypt in filteredFilesToEncrypt)
                {
                    // Build command arguments for CryptoSoft, consisting of file paths to encrypt.
                    FilteredFilesPath += fileCrypt + " ";

                    string hashFileName = fileCrypt + ".hash";
                    // If the hash file already exists, temporary removal of the "hidden" attribute to allow writing to the file.
                    if (File.Exists(hashFileName))
                    {
                        File.SetAttributes(hashFileName, File.GetAttributes(hashFileName) & ~FileAttributes.Hidden);
                    }
                    // Writing the hash from the source file to the hash file
                    File.WriteAllText(hashFileName, CalculateFileHash(fileCrypt));
                    // Added the "hidden" attribute to the file in order to hide it by default in the folder
                    File.SetAttributes(hashFileName, File.GetAttributes(fileCrypt + ".hash") | FileAttributes.Hidden);
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
                // The result returned by CryptoSoft is as follows: fileName:encryptionTime;fileName2:encryptionTime2;...
                // It is therefore necessary to split the result (by ';') and treat each of the value pairs.
                string[] everyFileEncryptionTime = output.Trim().Split(';');
                foreach(string fileEncryptionTime in everyFileEncryptionTime)
                {
                    if(fileEncryptionTime != "")
                    {
                        // This string contains file name and file encryption time
                        string[] splitedFileEncryptionTime = fileEncryptionTime.Split(':');
                        // Get the encrypted file name
                        string sourceFileName = splitedFileEncryptionTime[0];
                        // Get the file encryption time
                        int encryptionTime = int.Parse(splitedFileEncryptionTime[1]);
                        // Adding the file encryption time to the total encryption time
                        totalEncryptionTime += encryptionTime;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the file has the specified extension.
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <param name="extension">Extension to check</param>
        /// <returns>True if file has specified extension, false if not.</returns>
        private static bool IsFileWithExtension(string filePath, string extension)
        {
            return Path.GetExtension(filePath).Equals(extension, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Calculates the hash of the source file and writes it to a hidden file .hash, in order to read it and check during a differential backup if the source file has had changes since it was encrypted.
        /// </summary>
        /// <param name="filePath">Path to the file to calculate the hash.</param>
        /// <returns>The hash of the source file</returns>
        private static string CalculateFileHash(string filePath)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            using var stream = File.OpenRead(filePath);
            byte[] hashBytes = sha256.ComputeHash(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
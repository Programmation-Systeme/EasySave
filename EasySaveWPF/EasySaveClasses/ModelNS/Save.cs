using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Text.Json;

namespace EasySaveClasses.ModelNS
{
    /// <summary>
    /// Class allowing the management of saves, representing a save with its name, state, progress... More generally, its characteristics.
    /// </summary>
    internal class Save
    {
        // Private fields for the Save class
        private string _fileName;
        private string _state;
        private string _currentSourceFolder;
        private string _destinationFile;
        private int _totalFilesToCopy;
        private long _totalFilesSize;
        private int _nbFilesLeftToDo;
        private int _progression;
        private int _saveType;
        private int _encryptingTime;

        // Properties for accessing the private fields of Save class

        /// <summary>
        /// Current status of the backup.
        /// </summary>
        [JsonProperty(nameof(State))]
        public string State { get => _state; set => _state = value; }

        /// <summary>
        /// The total number of files to copy for this save.
        /// </summary>
        [JsonProperty(nameof(TotalFilesToCopy))]
        public int TotalFilesToCopy { get => _totalFilesToCopy; set => _totalFilesToCopy = value; }

        /// <summary>
        /// Total size of all files in the backup.
        /// </summary>
        [JsonProperty(nameof(TotalFilesSize))]
        public long TotalFilesSize { get => _totalFilesSize; set => _totalFilesSize = value; }

        /// <summary>
        /// Number of remaining files to process for this save.
        /// </summary>
        [JsonProperty(nameof(NbFilesLeftToDo))]
        public int NbFilesLeftToDo { get => _nbFilesLeftToDo; set => _nbFilesLeftToDo = value; }

        /// <summary>
        /// Progress of running this save.
        /// </summary>
        [JsonProperty(nameof(Progression))]
        public int Progression { get => _progression; set => _progression = value; }

        /// <summary>
        /// Name of this save.
        /// </summary>
        [JsonProperty(nameof(Name))]
        public string Name { get => _fileName; set => _fileName = value; }

        /// <summary>
        /// Path to the save source folder.
        /// </summary>
        [JsonProperty(nameof(SourceFolderPath))]
        public string SourceFolderPath { get => _currentSourceFolder; set => _currentSourceFolder = value; }

        /// <summary>
        /// Path to the target folder.
        /// </summary>
        [JsonProperty(nameof(TargetFolderPath))]
        public string TargetFolderPath { get => _destinationFile; set => _destinationFile = value; }

        /// <summary>
        /// Type of the save (1 for Full, 2 for Differential)
        /// </summary>
        [JsonProperty(nameof(SaveType))]
        public int SaveType { get => _saveType; set => _saveType = value; }

        /// <summary>
        /// Encrypting time of the save
        /// </summary>
        [JsonProperty(nameof(EncryptionTime))]
        public int EncryptionTime { get => _encryptingTime; set => _encryptingTime = value; }

        /// <summary>
        /// Constructor required and used for deserialization
        /// </summary>
        public Save() {}

        /// <summary>
        /// Initializes a new instance of a save, which allows the management of saves.
        /// </summary>
        /// <param name="fileName">Name of this save</param>
        /// <param name="state">Actual state of the save</param>
        /// <param name="sourceFolder">Source folder of the save</param>
        /// <param name="destinationFolder">Destination folder of the save</param>
        /// <param name="saveType">Type of the save</param>
        /// <param name="nbFilesLeftToDo">Number of files left to process</param>
        /// <param name="progression">Progression of the save</param>
        public Save(string fileName, string state, string sourceFolder, string destinationFolder, int saveType, int nbFilesLeftToDo =0, int progression =0)
        {
            string[] files = [];
            if (sourceFolder != null)
            {
                files = Directory.GetFiles(sourceFolder);
            }
            int numberOfFiles = files.Length;

            // Calculating the size of the folder
            long totalSize = 0;
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                totalSize += fileInfo.Length;
            }

            _fileName = fileName;
            _state = state;
            _currentSourceFolder = sourceFolder;
            _destinationFile = destinationFolder;
            _totalFilesToCopy = numberOfFiles;
            _totalFilesSize = Convert.ToInt64(totalSize);
            _nbFilesLeftToDo = nbFilesLeftToDo;
            _progression = progression;
            _saveType = saveType;
        }

        /// <summary>
        /// Method to serialize the Save array to JSON and save it to a file
        /// </summary>
        /// <param name="dataSaveList"></param>
        public static void Serialize(List<Save> dataSaveList)
        {
            string fileName = "SaveData.json";
            try
            {
                // Configure JSON serialization options
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                // Serialize the Save list to JSON string
                string jsonSave = System.Text.Json.JsonSerializer.Serialize(dataSaveList, options);
                // Write JSON Save to file
                File.WriteAllText(fileName, jsonSave);
            }
            catch
            {
                // Handle any errors during file creation
            }
        }

        /// <summary>
        /// Method to deserialize JSON Save from file back to Save array
        /// </summary>
        /// <returns>The list of Save objects</returns>
        public static List<Save> UnSerialize()
        {
            string fileName = "SaveData.json";
            // Check if the file exists
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                // Check if JSON Save is not empty
                if (jsonString.Length > 0)
                {
                    // Deserialize JSON Save to Save array
                    List<Save>? saveSaveArray = System.Text.Json.JsonSerializer.Deserialize<List<Save>>(jsonString);

                    if (saveSaveArray != null && saveSaveArray.Count > 0)
                    {
                        Serialize(saveSaveArray);
                        // Return the deserialized Save array
                        return saveSaveArray;
                    }
                }
            }
            // If file doesn't exist or JSON Save is empty, create a new Save array, serialize it and return it
            List<Save> saveTab = [];
            Serialize(saveTab);
            return saveTab;
        }
    }
}

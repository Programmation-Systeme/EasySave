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
    internal class Save
    {
        // Private fields for the Save class
        private string _fileName;
        private string _state;
        private string _currentSourceFile;
        private string _destinationFile;
        private int _totalFilesToCopy;
        private long _totalFilesSize;
        private int _nbFilesLeftToDo;
        private int _progression;
        private int _saveType;

        // Properties for accessing the private fields
        [JsonProperty(nameof(State))]
        public string State { get => _state; set => _state = value; }

        [JsonProperty(nameof(TotalFilesToCopy))]
        public int TotalFilesToCopy { get => _totalFilesToCopy; set => _totalFilesToCopy = value; }

        [JsonProperty(nameof(TotalFilesSize))]
        public long TotalFilesSize { get => _totalFilesSize; set => _totalFilesSize = value; }

        [JsonProperty(nameof(NbFilesLeftToDo))]
        public int NbFilesLeftToDo { get => _nbFilesLeftToDo; set => _nbFilesLeftToDo = value; }

        [JsonProperty(nameof(Progression))]
        public int Progression { get => _progression; set => _progression = value; }

        [JsonProperty(nameof(Name))]
        public string Name { get => _fileName; set => _fileName = value; }

        [JsonProperty(nameof(SourceFilePath))]
        public string SourceFilePath { get => _currentSourceFile; set => _currentSourceFile = value; }

        [JsonProperty(nameof(TargetFilePath))]
        public string TargetFilePath { get => _destinationFile; set => _destinationFile = value; }

        [JsonProperty(nameof(SaveType))]
        public int SaveType { get => _saveType; set => _saveType = value; }

        // Constructor required and used for deserialization
        public Save() {}

        public Save(string fileName, string state, string currentSourceFile, string destinationFile, int saveType, int nbFilesLeftToDo =0, int progression =0)
        {

            string[] files = Directory.GetFiles(currentSourceFile);
            int numberOfFiles = files.Length;

            // Calculer la taille du dossier
            long totalSize = 0;
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                totalSize += fileInfo.Length;
            }

            _fileName = fileName;
            _state = state;
            _currentSourceFile = currentSourceFile;
            _destinationFile = destinationFile;
            _totalFilesToCopy = numberOfFiles;
            _totalFilesSize = Convert.ToInt64(totalSize);
            _nbFilesLeftToDo = nbFilesLeftToDo;
            _progression = progression;
            _saveType = saveType;
        }
        // Method to serialize the Save array to JSON and save it to a file
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

        // Method to deserialize JSON Save from file back to Save array
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
                        // If the save destination directory doesn't exists anymore then it has been deleted outside the software, so we remove that save

                        // Copy of SaveArray
                        List<Save> saveSaveArrayCopy = [.. saveSaveArray];

                        foreach (Save save in saveSaveArrayCopy)
                        {
                            if (!Directory.Exists(save.TargetFilePath))
                            {
                                saveSaveArray.Remove(save);
                            }
                        }

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

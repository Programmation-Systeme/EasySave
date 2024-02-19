using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

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
        private int _totalFilesSize;
        private int _nbFilesLeftToDo;
        private int _progression;
        private int _saveType;

        // Properties for accessing the private fields
        public string State { get => _state; set => _state = value; }
        public int TotalFilesToCopy { get => _totalFilesToCopy; set => _totalFilesToCopy = 0; }
        public int TotalFilesSize { get => _totalFilesSize; set => _totalFilesSize = 0; }
        public int NbFilesLeftToDo { get => _nbFilesLeftToDo; set => _nbFilesLeftToDo = 0; }
        public int Progression { get => _progression; set => _progression = 0; }
        public string Name { get => _fileName; set => _fileName = value; }
        public string SourceFilePath { get => _currentSourceFile; set => _currentSourceFile = value; }
        public string TargetFilePath { get => _destinationFile; set => _destinationFile = value; }

        public int SaveType { get => _saveType; set => _saveType = value; }

        // Constructor
        public Save()
        {
        }
        public Save(string fileName, string state, string currentSourceFile, string destinationFile, int nbFilesLeftToDo =0, int progression =0)
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
            _totalFilesSize = Convert.ToInt32(totalSize);
            _nbFilesLeftToDo = nbFilesLeftToDo;
            _progression = progression;
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
                string jsonSave = JsonSerializer.Serialize(dataSaveList, options);
                // Write JSON Save to file
                File.WriteAllText(fileName, jsonSave);
            }
            catch (Exception ex)
            {
                // Handle any errors during file creation
                Console.WriteLine($" Error in file creation : {ex.Message}");
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
                if (jsonString != "")
                {
                    // Deserialize JSON Save to Save array
                    List<Save> saveSaveArray = JsonSerializer.Deserialize<List<Save>>(jsonString);

                    if (saveSaveArray.Any())
                    {
                        List<Save> savesToRemove = new List<Save>();
                        foreach (Save save in saveSaveArray)
                        {
                            if (!Directory.Exists(save.TargetFilePath))
                            {
                                savesToRemove.Add(save);
                            }
                        }

                        // Supprimer les éléments marqués
                        foreach (Save saveToRemove in savesToRemove)
                        {
                            saveSaveArray.Remove(saveToRemove);
                        }
                        Serialize(saveSaveArray);
                        // Return the deserialized Save array
                        return saveSaveArray;
                    }
                }
            }
            // If file doesn't exist or JSON Save is empty, create a new Save array, serialize it and return it
            List<Save> saveTab = new List<Save>();
            Serialize(saveTab);
            return saveTab;
        }
    }
}

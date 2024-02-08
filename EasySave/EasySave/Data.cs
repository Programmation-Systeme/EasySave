using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySave
{
    internal class Data
    {
        // Private fields for the data class
        private string _fileName;
        private string _state;
        private string _currentSourceFile;
        private string _destinationFile;
        private int _totalFilesToCopy;
        private int _totalFilesSize;
        private int _nbFilesLeftToDo;
        private int _progression;

        // Properties for accessing the private fields
        public string State { get => _state; set => _state = value; }
        public int TotalFilesToCopy { get => _totalFilesToCopy; set => _totalFilesToCopy = 0; }
        public int TotalFilesSize { get => _totalFilesSize; set => _totalFilesSize = 0; }
        public int NbFilesLeftToDo { get => _nbFilesLeftToDo; set => _nbFilesLeftToDo = 0; }
        public int Progression { get => _progression; set => _progression = 0; }
        public string Name { get => _fileName; set => _fileName = value; }
        public string SourceFilePath { get => _currentSourceFile; set => _currentSourceFile = value; }
        public string TargetFilePath { get => _destinationFile; set => _destinationFile = value; }

        // Constructor
        public Data()
        {
        }

        // Method to serialize the Data array to JSON and save it to a file
        public static void Serialize(Data[] saveDataList)
        {
            string fileName = "SaveData.json";
            try
            {
                // Configure JSON serialization options
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                // Serialize the data list to JSON string
                string jsonData = JsonSerializer.Serialize(saveDataList, options);
                // Write JSON data to file
                File.WriteAllText(fileName, jsonData);
            }
            catch (Exception ex)
            {
                // Handle any errors during file creation
                Console.WriteLine($" Error in file creation : {ex.Message}");
            }
        }

        // Method to deserialize JSON data from file back to Data array
        public static Data[] UnSerialize()
        {
            string fileName = "SaveData.json";
            // Check if the file exists
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                // Check if JSON data is not empty
                if (jsonString != "")
                {
                    // Deserialize JSON data to Data array
                    Data[] saveDataArray = JsonSerializer.Deserialize<Data[]>(jsonString);

                    if (saveDataArray != null)
                    {
                        // Return the deserialized data array
                        return saveDataArray;
                    }
                }
            }
            // If file doesn't exist or JSON data is empty, create a new Data array, serialize it and return it
            Data[] saveTab = new Data[5];
            Serialize(saveTab);
            return saveTab;
        }
    }
}

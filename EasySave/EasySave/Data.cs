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
        private string _fileName;
        private string _state;
        private string _currentSourceFile;
        private string _destinationFile;
        private int _totalFilesToCopy;
        private int _totalFilesSize;
        private int _nbFilesLeftToDo;
        private int _progression;
        public string State { get => _state; set => _state = value; }
        public int TotalFilesToCopy { get => _totalFilesToCopy; set => _totalFilesToCopy = 0; }
        public int TotalFilesSize { get => _totalFilesSize; set => _totalFilesSize = 0; }
        public int NbFilesLeftToDo { get => _nbFilesLeftToDo; set => _nbFilesLeftToDo = 0; }
        public int Progression { get => _progression; set => _progression = 0; }
        public string Name { get => _fileName; set => _fileName = value; }
        public string SourceFilePath { get => _currentSourceFile; set => _currentSourceFile = value; }
        public string TargetFilePath { get => _destinationFile; set => _destinationFile = value; }
        public Data()
        {
        }

        public static void Serialize(Data[] saveDataList)
        {
            string fileName = "SaveData.json";
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                string jsonData = JsonSerializer.Serialize(saveDataList, options);

                // Écrit la chaîne JSON dans le fichier
                File.WriteAllText(fileName, jsonData);

                //Console.WriteLine("JSON file created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error in file creation : {ex.Message}");
            }
        }

        public static Data[] UnSerialize()
        {
            string fileName = "SaveData.json";
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                if(jsonString != "") 
                {
                    Data[] saveDataArray = JsonSerializer.Deserialize<Data[]>(jsonString);

                    if (saveDataArray != null)
                    {
                        return saveDataArray;
                    }
                }      
            }
            else
            {
                //Console.WriteLine("This file does not exist!");
            }
            Data[] saveTab = new Data[5];
            Serialize(saveTab);
            return saveTab;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;


namespace EasySave
{
    internal class Log
    {
        private List<int> _indexes;
        private Model _model;
        private float _fileTransferTime;
        internal List<int> Indexes { get => _indexes; set => _indexes = value; }

        private string JsonPath = "D:\\CESI\\ProgSys\\Projet\\Code\\EasySave\\EasySave\\EasySave\\Json\\Logs.json";
        /// <summary>
        /// Entry point of the log class
        /// </summary>
        /// <param name="model"></param>
        internal Log(Model model) {
            _model = model;
        }

        /// <summary>
        /// Write in the log when save
        /// </summary>
        public void AddLog()
        {
            for (int i = 0; i < _indexes.Count; i++) 
            {
                string NameFile = _model.Datas[_indexes[i]-1].FileName;
                string SourcePath = _model.Datas[_indexes[i]-1].CurrentSourceFile;
                string DestinationPath = _model.Datas[_indexes[i] - 1].DestinationFile;
                FileInfo fiSource = new FileInfo(SourcePath);
                long fileSize = fiSource.Length;
                //long fileSize = 25;
                _fileTransferTime = 256;

                string date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                var newLogEntry = new
                {
                    Name = NameFile,
                    FileSource = SourcePath,
                    FileDestination = DestinationPath,
                    FileSize = fileSize,
                    FileTransferTime = _fileTransferTime,
                    Date = date
                };
                List<object> existingLogs = new List<object>();

                if (File.Exists(JsonPath))
                {
                    string jsonContent = File.ReadAllText(JsonPath);
                    existingLogs = JsonConvert.DeserializeObject<List<object>>(jsonContent);
                    existingLogs.Add(newLogEntry);
                    string updatedJson = JsonConvert.SerializeObject(existingLogs, Formatting.Indented);
                    File.WriteAllText(JsonPath, updatedJson);
                    Console.WriteLine("Ajout des Logs avec succès");
                }
                else
                {
                    Console.WriteLine("Pas trouvé le JSON");
                }
            }
        }
    }
}

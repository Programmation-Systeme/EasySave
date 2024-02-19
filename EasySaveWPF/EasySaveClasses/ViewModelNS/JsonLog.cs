using EasySaveClasses.ModelNS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveClasses.ViewModelNS
{
    internal class JsonLog : ILog
    {
        private readonly Model _model;

        private static readonly string JsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../LogsDirectory/Logs.json");

        public JsonLog(Model model)
        {
            _model = model;
        }

        public String AddLog()
        {
            var newLogEntry = new
            {
                //Timestamp = this.timestamp,
                //Name = this.saveName,
                //sourcePath = this.sourcePath,
                //targetPath = this.targetPath,
                //DirSize = this.directorySize,
                //DirTransferTime = this.transferTime
            };
            List<object> existingLogs = new List<object>();

            if (File.Exists(JsonPath))
            {
                string jsonContent = File.ReadAllText(JsonPath);
                existingLogs = JsonConvert.DeserializeObject<List<object>>(jsonContent);
                existingLogs.Add(newLogEntry);
                string updatedJson = JsonConvert.SerializeObject(existingLogs, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(JsonPath, updatedJson);
                return "Ajout des Logs avec succès";
            }
            else
            {
                return " Pas trouvé le JSON";
            }

        }

        public void AddLog(List<int> indexes)
        {
            foreach (int index in indexes)
            {
                string NameFile = _model.Datas[index - 1].Name;
                string SourcePath = _model.Datas[index - 1].SourceFilePath;
                string DestinationPath = _model.Datas[index - 1].TargetFilePath;

                DirectoryInfo diSource = new DirectoryInfo(SourcePath);
                long DirectorySize = CalculateDirectorySize(diSource);
                float fileTransferTime = 256;

                string date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                //LogEntry newLogEntry = new()
                //{
                //    Name = NameFile,
                //    FileSource = SourcePath,
                //    FileDestination = DestinationPath,
                //    FileSize = DirectorySize,
                //    FileTransferTime = fileTransferTime,
                //    Date = date
                //};

                List<LogEntry> existingLogs = ReadFromFile();
                //existingLogs.Add(newLogEntry);
                WriteToFile(existingLogs);

                Console.WriteLine("Logs ajoutés avec succès");
            }
        }

        private static long CalculateDirectorySize(DirectoryInfo directory)
        {
            long size = 0;
            FileInfo[] files = directory.GetFiles();

            foreach (FileInfo file in files)
            {
                size += file.Length;
            }

            DirectoryInfo[] subDirectories = directory.GetDirectories();

            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                size += CalculateDirectorySize(subDirectory);
            }

            return size;
        }

        private static void WriteToFile(List<LogEntry> data)
        {
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(JsonPath, jsonData);
        }

        private static List<LogEntry> ReadFromFile()
        {
            if (File.Exists(JsonPath))
            {
                string jsonData = File.ReadAllText(JsonPath);
                return JsonConvert.DeserializeObject<List<LogEntry>>(jsonData);
            }
            else
            {
                return new List<LogEntry>();
            }
        }
    }
}

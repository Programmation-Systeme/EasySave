using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace EasySave
{
    public class LogEntry
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileDestination { get; set; }
        public long FileSize { get; set; }
        public float FileTransferTime { get; set; }
        public string Date { get; set; }
    }
    public interface ILog
    {
        void AddLog(List<int> indexes);
    }

    internal class XmlLog : ILog
    {
        private readonly Model _model;

        private static readonly string XmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../LogsDirectory/Logs.xml");

        public XmlLog(Model model)
        {
            _model = model;
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

                LogEntry newLogEntry = new LogEntry
                {
                    Name = NameFile,
                    FileSource = SourcePath,
                    FileDestination = DestinationPath,
                    FileSize = DirectorySize,
                    FileTransferTime = fileTransferTime,
                    Date = date
                };

                List<LogEntry> existingLogs = ReadFromFile();
                existingLogs.Add(newLogEntry);
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<LogEntry>));
            using (StreamWriter writer = new StreamWriter(XmlPath))
            {
                xmlSerializer.Serialize(writer, data);
            }
        }

        private static List<LogEntry> ReadFromFile()
        {
            if (File.Exists(XmlPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<LogEntry>));
                using (StreamReader reader = new StreamReader(XmlPath))
                {
                    return (List<LogEntry>)xmlSerializer.Deserialize(reader);
                }
            }
            else
            {
                return new List<LogEntry>();
            }
        }
    }

    internal class JsonLog : ILog
    {
        private readonly Model _model;

        private static readonly string JsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../LogsDirectory/Logs.json");

        public JsonLog(Model model)
        {
            _model = model;
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

                LogEntry newLogEntry = new LogEntry
                {
                    Name = NameFile,
                    FileSource = SourcePath,
                    FileDestination = DestinationPath,
                    FileSize = DirectorySize,
                    FileTransferTime = fileTransferTime,
                    Date = date
                };

                List<LogEntry> existingLogs = ReadFromFile();
                existingLogs.Add(newLogEntry);
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

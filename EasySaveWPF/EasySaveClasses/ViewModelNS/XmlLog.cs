using EasySaveClasses.ModelNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EasySaveClasses.ViewModelNS
{
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

                //LogEntry newLogEntry = new LogEntry()
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<LogEntry>));
            using (StreamWriter writer = new(XmlPath))
            {
                xmlSerializer.Serialize(writer, data);
            }
        }

        private static List<LogEntry> ReadFromFile()
        {
            if (File.Exists(XmlPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<LogEntry>));
                using StreamReader reader = new(XmlPath);
                return (List<LogEntry>)xmlSerializer.Deserialize(reader);
            }
            else
            {
                return new List<LogEntry>();
            }
        }
    }

}

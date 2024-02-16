using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using EasySaveClasses.ModelNS;
using System.Windows;

namespace EasySaveClasses.ViewModelNS
{
    internal class Log
    {
        private string timestamp, saveName, sourcePath, targetPath;
        private float directorySize, transferTime;

        private string JsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../ViewModelNS/Logs.json");
        /// <summary>
        /// Entry point of the log class
        /// </summary>
        /// <param name="model"></param>
        internal Log(string sourcePath, string targetPath, float transferTime)
        {
            this.timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            this.saveName = Path.GetFileName(sourcePath);
            this.sourcePath = sourcePath;
            this.targetPath = targetPath;
            this.directorySize = 0;
            this.transferTime = transferTime;
            //this.directorySize = CalculateDirectorySize(new DirectoryInfo(sourcePath));
        }

        /// <summary>
        /// Write in the log when save
        /// </summary>
        public String AddLog()
        {
                var newLogEntry = new
                {
                    Timestamp = this.timestamp,
                    Name = this.saveName,
                    sourcePath = this.sourcePath,
                    targetPath = this.targetPath,
                    DirSize = this.directorySize,
                    DirTransferTime = this.transferTime
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

        static long CalculateDirectorySize(DirectoryInfo directory)
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
    }
}

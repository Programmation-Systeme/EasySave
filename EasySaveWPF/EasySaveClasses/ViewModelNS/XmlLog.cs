using EasySaveClasses.ViewModelNS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class XmlLog : ILog
{
    private static readonly string XmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../LogDirectory/Logs.xml");

    public string AddLog(string sourcePath, string targetPath, float transferTime)
    {
        var logEntry = new LogEntry
        {
            Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
            Name = Path.GetFileName(sourcePath),
            SourcePath = sourcePath,
            TargetPath = targetPath,
            CryptingTime = new Random().Next(1, 9999),
            DirSize = EasySaveClasses.ViewModelNS.ILog.CalculateDirectorySize(new DirectoryInfo(sourcePath)),
            DirTransferTime = transferTime
        };

        var existingLogs = new List<LogEntry>();
        if (File.Exists(XmlPath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<LogEntry>));
            using (FileStream fs = new FileStream(XmlPath, FileMode.Open))
            {
                existingLogs = (List<LogEntry>)serializer.Deserialize(fs);
            }
        }

        existingLogs.Add(logEntry);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<LogEntry>));
        using (FileStream fs = new FileStream(XmlPath, FileMode.Create))
        {
            xmlSerializer.Serialize(fs, existingLogs);
        }

        return "Log added successfully in XML format";
    }
}

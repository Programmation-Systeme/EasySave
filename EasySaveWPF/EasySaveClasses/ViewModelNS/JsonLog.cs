using EasySaveClasses.ViewModelNS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class JsonLog : ILog
{
    private static readonly string JsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../LogDirectory/Logs.json");

    public string AddLog(string sourcePath, string targetPath, float transferTime)
    {
        var logEntry = new LogEntry
        {
            Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
            Name = Path.GetFileName(sourcePath),
            SourcePath = sourcePath,
            TargetPath = targetPath,
            DirSize = EasySaveClasses.ViewModelNS.ILog.CalculateDirectorySize(new DirectoryInfo(sourcePath)),
            CryptingTime = new Random().Next(1, 9999),
            DirTransferTime = transferTime
        };

        var existingLogs = new List<LogEntry>();
        if (File.Exists(JsonPath))
        {
            string jsonContent = File.ReadAllText(JsonPath);
            existingLogs = JsonConvert.DeserializeObject<List<LogEntry>>(jsonContent) ?? new List<LogEntry>();
        }

        existingLogs.Add(logEntry);
        string updatedJson = JsonConvert.SerializeObject(existingLogs, Formatting.Indented);
        File.WriteAllText(JsonPath, updatedJson);
        return "Log added successfully in JSON format";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveClasses.ViewModelNS
{
    public class LogEntry
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileDestination { get; set; }
        public long FileSize { get; set; }
        public float FileTransferTime { get; set; }
        public string Date { get; set; }

        protected string timestamp, saveName, sourcePath, targetPath;
        protected float directorySize, transferTime;

    internal LogEntry(string sourcePath, string targetPath, float transferTime)
    {
        this.timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        this.saveName = Path.GetFileName(sourcePath);
        this.sourcePath = sourcePath;
        this.targetPath = targetPath;
        this.directorySize = 0;
        this.transferTime = transferTime;
    }
    }


    public interface ILog
    {
        void AddLog(List<int> indexes);
    }
}

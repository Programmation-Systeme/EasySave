namespace EasySaveClasses.ViewModelNS
{
    public interface ILog
    {
        string AddLog(string sourcePath, string targetPath, float transferTime);

        public static long CalculateDirectorySize(DirectoryInfo directory)
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
    public class LogEntry
    {
        public string Timestamp { get; set; }
        public string Name { get; set; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public float DirSize { get; set; }
        public float DirTransferTime { get; set; }
        public int CryptingTime { get; set; }

    }
}
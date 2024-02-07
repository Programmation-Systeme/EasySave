using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave
{
    internal class Data
    {
        private string _fileName;
        private string _currentSourceFile;
        private string _destinationFile;

        public string FileName { get => _fileName; set => _fileName = value; }
        public string CurrentSourceFile { get => _currentSourceFile; set => _currentSourceFile = value; }
        public string DestinationFile { get => _destinationFile; set => _destinationFile = value; }
        public Data(string fileName)
        {
            _fileName = fileName;
        }
        public Data(string fileName, string currentSourceFile, string destinationFile)
        {
            _fileName = fileName;
            _currentSourceFile = currentSourceFile;
            _destinationFile = destinationFile;
        }
    }

}

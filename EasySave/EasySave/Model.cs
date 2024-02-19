using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace EasySave
{
    internal class Model
    {
        private Data[] _datas;
        /// <summary>
        /// Entry point of the model
        /// </summary>
        public Model()
        {
            _datas = Data.UnSerialize();
        }

        internal Data[] Datas { get => _datas; set => _datas = value; }

        public void AddDataOnSlot(string sourceDirectory, string destinationFile, int saveType, int slot)
        {
            Data saveData = new()
            {
                Name = Path.GetFileName(sourceDirectory),
                SourceFilePath = sourceDirectory,
                TargetFilePath = destinationFile,
                State = "ACTIVE",
                TotalFilesToCopy = 3300,
                TotalFilesSize = 1240312777,
                NbFilesLeftToDo = 3274,
                Progression = 0,
                SaveType = saveType,
            };
            _datas[slot - 1] = saveData;
            Data.Serialize(_datas);
        }

        public void SerializeDatas()
        {
            try {  Data.Serialize(_datas); } catch { throw; }
        }

    }
}

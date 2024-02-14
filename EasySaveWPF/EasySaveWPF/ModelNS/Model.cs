using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveWPF.ModelNS
{
    public class Model
    {

        private Data[] _datas;
        /// <summary>
        /// Entry point of the model
        /// </summary>
        public Model()
        {
            _datas = new Data[5];
            // Initialize data slots (for the moment)
            var saveData1 = new Data
            {
                Name = "Save9",
                SourceFilePath = "C:\\progSys\\Projet\\gr6_save\\EasySave_Group6_1.1\\api - ms - win - core - errorhandling - l1 - 1 - 0.dll",
                TargetFilePath = "D:\\save\\Projet\\gr6_save\\EasySave_Group6_1.1\\api-ms-win-core-errorhandling-l1-1- 0.dll",
                State = "ACTIVE",
                TotalFilesToCopy = 3300,
                TotalFilesSize = 1240312777,
                NbFilesLeftToDo = 3274,
                Progression = 0
            };
            var saveData2 = new Data
            {
                Name = "Save2",
                SourceFilePath = "TEST",
                TargetFilePath = "TEST",
                State = "DESACTIVE",
                TotalFilesToCopy = 0,
                TotalFilesSize = 0,
                NbFilesLeftToDo = 0,
                Progression = 0
            };
            var saveData3 = new Data
            {
                Name = "Save3",
                SourceFilePath = "1",
                TargetFilePath = "1",
                State = "ACTIVE",
                TotalFilesToCopy = 1,
                TotalFilesSize = 1,
                NbFilesLeftToDo = 1,
                Progression = 0
            };
            //var saveData4 = new Data
            //{
            //    Name = "Empty",
            //    SourceFilePath = "",
            //    TargetFilePath = "",
            //    State = "",
            //    TotalFilesToCopy = 0,
            //    TotalFilesSize = 0,
            //    NbFilesLeftToDo = 0,
            //    Progression = 0
            //};
            //var saveData5 = new Data
            //{
            //    Name = "Empty",
            //    SourceFilePath = "",
            //    TargetFilePath = "",
            //    State = "",
            //    TotalFilesToCopy = 0,
            //    TotalFilesSize = 0,
            //    NbFilesLeftToDo = 0,
            //    Progression = 0
            //};
            //_datas[0] = saveData1;
            //_datas[4] = saveData2;
            //_datas[2] = saveData3;
            //_datas[3] = saveData4;
            //_datas[4] = saveData5;

            //Data.Serialize(_datas);
            _datas = Data.UnSerialize();
        }

        internal Data[] Datas { get => _datas; set => _datas = value; }
    }
}

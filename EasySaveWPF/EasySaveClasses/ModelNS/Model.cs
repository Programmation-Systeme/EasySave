using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveClasses.ModelNS
{
    public class Model
    {
        private List<Save> _datas = [];
        internal List<Save> Datas { get => _datas; set => _datas = value; }

        /// <summary>
        /// Entry point of the model
        /// </summary>
        public Model()
        {
            _datas = Save.UnSerialize();
        }

        public List<string> GetSaveList()
        {
            List<string> saveList = [];
            foreach (var save in _datas) 
            {
                saveList.Add(save.Name);
            }
            return saveList;
        }
    }
}

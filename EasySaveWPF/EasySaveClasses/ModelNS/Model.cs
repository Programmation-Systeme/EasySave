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

        /// <summary>
        /// Get a saves names list from the saves.
        /// </summary>
        /// <returns>A list containing saves' names</returns>
        public List<string> GetSavesNamesList()
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveClasses.ModelNS
{
    /// <summary>
    /// Model component of the MVVM allowing the centralization of application data.
    /// </summary>
    public class Model
    {
        private List<Save> _datas = [];
        internal List<Save> Datas { get => _datas; set => _datas = value; }

        /// <summary>
        /// Initializes a new instance of the model, allowing centralization of application data.
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

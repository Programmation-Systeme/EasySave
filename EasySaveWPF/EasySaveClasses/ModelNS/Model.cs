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
        private List<Save> _datas = new List<Save>();
        /// <summary>
        /// Entry point of the model
        /// </summary>
        public Model()
        {
            _datas = Save.UnSerialize();
        }

        public List<string> getSaveList()
        {
            List<string> list = new List<string>();
            foreach (var save in _datas) 
            {
                list.Add(save.Name);
            }
            return list;
        }

        internal List<Save> Datas { get => _datas; set => _datas = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

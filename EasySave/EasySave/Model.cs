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

        public Model()
        {
            _datas = new Data[5];
            // Initialize data slots (for the moment)
            _datas[0] = new Data("Item1");
            _datas[1] = new Data("Item2");
            _datas[3] = new Data("Item4");
        }

        internal Data[] Datas { get => _datas; set => _datas = value; }
    }
}

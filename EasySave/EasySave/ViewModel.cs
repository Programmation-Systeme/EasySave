using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave
{
    internal class ViewModel
    {
        private Model _model;
         
        public ViewModel()
        {
            _model = new Model();
        }

        internal Model Model { get => _model; set => _model = value; }
    }
}

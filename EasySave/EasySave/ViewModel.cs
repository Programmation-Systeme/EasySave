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

        private Log _log;
         
        public ViewModel()
        {
            _model = new Model();
            _log = new Log(_model);
        }

        internal Model Model { get => _model; set => _model = value; }

        internal Log Log { get => _log; set => _log = value; }
    }
}

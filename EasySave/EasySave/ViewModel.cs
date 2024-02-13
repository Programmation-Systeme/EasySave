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

        private ILog _log;
         /// <summary>
         /// Entry point of the log
         /// </summary>
        public ViewModel()
        {
            _model = new Model();
            _log = new JsonLog(_model);
        }

        internal Model Model { get => _model; set => _model = value; }

        internal ILog Log { get => _log; set => _log = value; }
    }
}

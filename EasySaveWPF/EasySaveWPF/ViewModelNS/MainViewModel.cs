using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveWPF.ModelNS;

namespace EasySaveWPF.ViewModelNS
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Model _model;

        private Log _log;
        /// <summary>
        /// Entry point of the log
        /// </summary>
        public MainViewModel()
        {
            _model = new Model();
            _log = new Log(_model);
        }

        internal Model Model { get => _model; set => _model = value; }

        internal Log Log { get => _log; set => _log = value; }
    }
}

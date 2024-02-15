using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveWPF.ModelNS;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;

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

        private ObservableCollection<string> _items;
        public ObservableCollection<string> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ICommand ClickCommand { get; private set; }


        /// <summary>
        /// Entry point of the log
        /// </summary>
        /// 
        public MainViewModel()
        {
            Items = new ObservableCollection<string>
            {
                "Item1",
                "Item2",
                "Item3",
            };
            ClickCommand = new RelayCommand(ExecuteClickCommand);
        }

        private void ExecuteClickCommand()
        {
            IsMetierSoftwareRunning();
            _log = new Log("D:\\CESI\\Anglais\\Presentation.txt", "feur", 25);
            _log.AddLog();
        }

        private void IsMetierSoftwareRunning()
        {
            // Nom du processus du logiciel métier (à adapter en fonction de votre cas)
            string metierSoftwareProcessName = "CalculatorApp";

            // Vérifie si le processus est en cours d'exécution
            Process[] processes = Process.GetProcessesByName(metierSoftwareProcessName);
            //Process[] processesExec = Process.GetProcesses();
            if (processes.Length>0)
            {
                MessageBox.Show("Le logiciel métier est en cours d'exécution. Veuillez le fermer avant de lancer la sauvegarde.", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Le travail de sauvegarde a été lancé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

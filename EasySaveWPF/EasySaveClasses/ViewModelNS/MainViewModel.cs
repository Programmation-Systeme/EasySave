using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveClasses.ModelNS;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using Microsoft.Win32;

namespace EasySaveClasses.ViewModelNS
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

        private string _openFileSrc;
        public string OpenFileSrc
        {
            get { return _openFileSrc; }
            set
            {
                _openFileSrc = value;
                OnPropertyChanged(nameof(OpenFileSrc));
            }
        }

        private string _openFileDest;

        public string OpenFileDest
        {
            get { return _openFileDest; }
            set
            {
                _openFileDest = value;
                OnPropertyChanged(nameof(OpenFileDest));
            }
        }

        private string _errorText;

        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                _errorText = value;
                OnPropertyChanged(nameof(ErrorText));
            }
        }

        public ICommand ClickCommand { get; private set; }
        public ICommand btnOpenFilesSrc { get; private set; }
        public ICommand btnOpenFilesDest { get; private set; }


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
            btnOpenFilesSrc = new RelayCommand(OpenFilesSrc_Click);
            btnOpenFilesDest = new RelayCommand(OpenFilesDest_Click);
        }



        private void OpenFilesSrc_Click()
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Multiselect = false;
            openFolderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFolderDialog.ShowDialog() == true)
            {
                OpenFileSrc = openFolderDialog.FolderName;
            }
        }


        private void OpenFilesDest_Click()
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Multiselect = false;
            openFolderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFolderDialog.ShowDialog() == true)
            {
                OpenFileDest = openFolderDialog.FolderName;
            }
        }

        private void ExecuteClickCommand()
        {
            IsMetierSoftwareRunning();
            _log = new Log("D:\\CESI\\Anglais\\Presentation.txt", "feur", 25);
            ErrorText = _log.AddLog();
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
                ErrorText = "Le logiciel métier est en cours d'exécution. Veuillez le fermer avant de lancer la sauvegarde.";
            }
            else
            {
                ErrorText = "Le travail de sauvegarde a été lancé avec succès.";
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

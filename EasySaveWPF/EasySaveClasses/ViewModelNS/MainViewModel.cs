using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveClasses.ModelNS;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace EasySaveClasses.ViewModelNS
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        List<Thread> workersList = new List<Thread>();
        static object lockObject = new object();

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

        private ObservableCollection<string> _selectedItemss;
        public ObservableCollection<string> SelectedItemss
        {
            get { return _selectedItemss; }
            set
            {
                _selectedItemss = value;
                OnPropertyChanged(nameof(SelectedItemss));
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

        static void ExecuteWork(string source, string directory)
        {
            lock (lockObject)
            {

            }
            bool res = EditSave.Update(source, directory);
            if (res) 
            {
                lock (lockObject)
                {

                }
            }
            else
            {
                lock (lockObject)
                {

                }
            }
        }

        public ICommand ClickCommand { get; private set; }

        public ICommand btnAddSave { get; private set; }

        public ICommand btnExecutSaves { get; private set; }
        public ICommand btnDeletSaves { get; private set; }
        /// <summary>
        /// Entry point of the log
        /// </summary>
        /// 
        public MainViewModel()
        {
            Items = new ObservableCollection<string>
            {
            };
            _model = new Model();
            List<string> saveList = _model.getSaveList();
            foreach (string save in saveList) { Items.Add(save); }

            ClickCommand = new RelayCommand(ExecuteClickCommand);
            btnOpenFilesSrc = new RelayCommand(OpenFilesSrc_Click);
            btnOpenFilesDest = new RelayCommand(OpenFilesDest_Click);
            btnAddSave = new RelayCommand(AddSave_Click);
            btnExecutSaves = new RelayCommand(ExecuteSave_Click);
            btnDeletSaves = new RelayCommand(DeleteSave_Click);
        }
        //private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    var item = sender as ListBoxItem;
        //    if (item != null && item.IsSelected)
        //    {
        //        // Assuming your DataContext is MainViewModel
        //        var viewModel = (MainViewModel)this.DataContext;
        //        viewModel.SelectedItems = new ObservableCollection<string>(ItemSelected.SelectedItems.Cast<string>());
        //    }
        //}

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

        private void AddSave_Click()
        {
            EditSave.Create(OpenFileSrc, OpenFileDest);
            _model.Datas.Add(new ModelNS.Save(Path.GetFileName(OpenFileSrc), OpenFileSrc, OpenFileDest, "ACTIVE", 3300, 1240312777, 3274, 0));
            Save.Serialize(_model.Datas);
            foreach (Save save in _model.Datas)
            {
                Thread newWork = new Thread(() => ExecuteWork(save.SourceFilePath, save.TargetFilePath));
                workersList.Add(newWork);
                newWork.Start();
            }

        }

        private void ExecuteSave_Click()
        {
            List<ModelNS.Save> selectedSaves = new List<ModelNS.Save>();

            // Itérer à travers les éléments sélectionnés
            foreach (string selectedItemName in SelectedItemss)
            {
                // Utiliser LINQ pour trouver l'élément correspondant dans votre modèle de données
                ModelNS.Save selectedSave = _model.Datas.FirstOrDefault(item => item.Name == selectedItemName);

                // Vérifier si l'élément est trouvé (il pourrait être null si aucun élément ne correspond)
                if (selectedSave != null)
                {
                    // Ajouter l'élément sélectionné à la liste
                    selectedSaves.Add(selectedSave);
                }
            }

            // Vous avez maintenant la liste des éléments sélectionnés dans la liste selectedSaves
            // Vous pouvez les utiliser selon vos besoins ici.
            foreach (ModelNS.Save save in selectedSaves)
            {
                // Faites ce que vous devez faire avec chaque élément sélectionné ici
                // Par exemple, exécutez le travail sur cet élément, supprimez-le, etc.
            }
        }

        private void DeleteSave_Click()
        {
            
            _model.Datas.Add(new ModelNS.Save(Path.GetFileName(OpenFileSrc), OpenFileSrc, OpenFileDest, "ACTIVE", 3300, 1240312777, 3274, 0));
            Save.Serialize(_model.Datas);


            List<ModelNS.Save> filteredItems = new List<ModelNS.Save>();

            // Filtrage des éléments en fonction des noms sélectionnés
            foreach (string selectedItemName in SelectedItemss)
            {
                // Utilisation de LINQ pour filtrer les éléments dont le nom correspond à un élément de SelectedItems
                var itemsWithName = _model.Datas.Where(item => item.Name == selectedItemName);

                // Ajout des éléments filtrés à la liste de résultats
                filteredItems.AddRange(itemsWithName);
            }

            foreach (Save save in filteredItems)
            {
                EditSave.Delete(save.TargetFilePath);
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

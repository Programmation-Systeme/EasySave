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
using System.Collections;
using static System.Net.Mime.MediaTypeNames;

namespace EasySaveClasses.ViewModelNS
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        SynchronizationContext _syncContext = SynchronizationContext.Current;
        private Dictionary<string, Thread> saveThreads = new Dictionary<string, Thread>();
        private Dictionary<string, bool> threadPausedStates = new Dictionary<string, bool>();
        private object pauseLock = new object();
        static Mutex mutex = new Mutex();
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


        private ObservableCollection<string> _currentSave;
        private string _currentSaveSelected;

        public ObservableCollection<string> CurrentSave
        {
            get { return _currentSave; }
            set
            {
                _currentSave = value;
                OnPropertyChanged(nameof(CurrentSave));
            }
        }

        public string CurrentSaveSelected
        {
            get { return _currentSaveSelected; }
            set
            {
                _currentSaveSelected = value;
                OnPropertyChanged(nameof(CurrentSaveSelected));
            }
        }

        public ICommand ClickCommand { get; private set; }

        public ICommand btnAddSave { get; private set; }

        /// <summary>
        /// Entry point of the log
        /// </summary>
        /// 
        public MainViewModel()
        {
            CurrentSave = new ObservableCollection<string>
            {
            };
            Items = new ObservableCollection<string>
            {
            };
            _model = new Model();
            List<string> saveList = _model.getSaveList();
            foreach (string save in saveList) { Items.Add(save); }

            ClickCommand = new RelayCommand(ExecuteClickCommand);

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
        public void PauseSave(string saveName)
        {
            lock (pauseLock)
            {
                if (threadPausedStates.ContainsKey(saveName))
                {
                    threadPausedStates[saveName] = true;
                    Monitor.Enter(pauseLock);
                }
            }
        }


        public void ResumeSave(string saveName)
        {
            lock (pauseLock)
            {
                if (threadPausedStates.ContainsKey(saveName))
                {
                    threadPausedStates[saveName] = false;
                    Monitor.PulseAll(pauseLock);
                }
            }
        }

        public void AbortSave(string saveName)
        {
            if (saveThreads.ContainsKey(saveName))
            {
                Thread thread = saveThreads[saveName];
                thread.Abort();
            }
        }

        private void ExecuteWork(Save save, SynchronizationContext syncContext,int index)
        {

           
                lock (pauseLock)
                {
                    if (threadPausedStates.ContainsKey(save.Name) && threadPausedStates[save.Name])
                    {
                        Monitor.Wait(pauseLock);
                    }
                }

                Thread.Sleep(4000);

                bool res = EditSave.Update(save.SourceFilePath, save.TargetFilePath);

                lock (pauseLock)
                {
                    if (threadPausedStates.ContainsKey(save.Name) && threadPausedStates[save.Name])
                    {
                        Monitor.Wait(pauseLock);
                    }
                }
                syncContext.Post(state =>
                    {
                        mutex.WaitOne();
                        CurrentSave.Remove(save.Name);
                        mutex.ReleaseMutex();
                    }, null);

      
                threadPausedStates.Remove(save.Name);
                saveThreads.Remove(save.Name);
                   
        }


        public void AddSave()
        {
            string formattedDateTime = DateTime.Now.ToString("MM-dd-yyyy-h-mm-ss");
            string targetPath = OpenFileDest + "\\" + Path.GetFileName(OpenFileSrc) + "-" + formattedDateTime;
            Save save = new ModelNS.Save(Path.GetFileName(targetPath), "ACTIVE", OpenFileSrc, targetPath,1);
            _model.Datas.Add(save);
            CurrentSave.Add(save.Name);
            EditSave.Create(OpenFileSrc, OpenFileDest);
            CurrentSave.Remove(save.Name);
            Save.Serialize(_model.Datas);
            Items.Add(save.Name);
            CurrentSave.Add(save.Name);
            Thread newWork = new Thread(() => ExecuteWork(save, _syncContext, 4));
            saveThreads.Add(save.Name, newWork);
            threadPausedStates.Add(save.Name, false);
            newWork.Start();

        }

        public void ExecuteSave_Click(List<string> list)
        {
            List<ModelNS.Save> selectedSaves = new List<ModelNS.Save>();

            // Itérer à travers les éléments sélectionnés
            int i = 0;

            foreach (string selectedItemName in list)
            {
                CurrentSave.Add(selectedItemName);
                ModelNS.Save selectedSave = _model.Datas.FirstOrDefault(item => item.Name == selectedItemName);
                if (selectedSave != null)
                {
                    i++;

                    Thread newWork = new Thread(() => ExecuteWork(selectedSave,_syncContext,i));
                    saveThreads.Add(selectedSave.Name, newWork);
                    threadPausedStates.Add(selectedSave.Name,false);
                    newWork.Start();
                }

            }
        }

        public void DeleteSave_Click(List<string> list)
        {

            List<ModelNS.Save> selectedSaves = new List<ModelNS.Save>();

            // Itérer à travers les éléments sélectionnés
            foreach (string selectedItemName in list)
            {
                // Utiliser LINQ pour trouver l'élément correspondant dans votre modèle de données
                ModelNS.Save selectedSave = _model.Datas.FirstOrDefault(item => item.Name == selectedItemName);

                // Vérifier si l'élément est trouvé (il pourrait être null si aucun élément ne correspond)
                if (selectedSave != null)
                {
                    EditSave.Delete(selectedSave.TargetFilePath);
                    _model.Datas.Remove(selectedSave);
                    Save.Serialize(_model.Datas);
                    Items.Remove(selectedSave.Name);
                }
            }

            //_model.Datas.Add(new ModelNS.Save(Path.GetFileName(OpenFileSrc), OpenFileSrc, OpenFileDest, "ACTIVE", 3300, 1240312777, 3274, 0));
            //Save.Serialize(_model.Datas);


            //List<ModelNS.Save> filteredItems = new List<ModelNS.Save>();

            //// Filtrage des éléments en fonction des noms sélectionnés
            //foreach (string selectedItemName in SelectedItemss)
            //{
            //    // Utilisation de LINQ pour filtrer les éléments dont le nom correspond à un élément de SelectedItems
            //    var itemsWithName = _model.Datas.Where(item => item.Name == selectedItemName);

            //    // Ajout des éléments filtrés à la liste de résultats
            //    filteredItems.AddRange(itemsWithName);
            //}

            //foreach (Save save in filteredItems)
            //{
            //    EditSave.Delete(save.TargetFilePath);
            //}
        }

        
        private void ExecuteClickCommand()
        {
            IsMetierSoftwareRunning();
            _log = new Log("D:\\CESI\\Anglais\\Presentation.txt", "feur", 25);
            ErrorText = _log.AddLog();
            bool save = EditSave.Update("D:\\CESI\\Anglais\\Presentation.txt", "D:\\CESI\\Anglais");
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

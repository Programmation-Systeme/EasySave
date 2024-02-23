using System.ComponentModel;
using EasySaveClasses.ModelNS;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using System.Security.AccessControl;

namespace EasySaveClasses.ViewModelNS
{
    /// <summary>
    /// ViewModel for the main functionality of the application.
    /// </summary>
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

        private readonly Model _model;
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

        private int _saveType;
        public int SaveType
        {
            get { return _saveType; }
            set
            {
                _saveType = value;
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


        /// <summary>
        /// Constructor initializes necessary properties and loads data.
        /// </summary>
        /// 
        public MainViewModel()
        {
            SaveType = 1;
            CurrentSave = [];
            Items = [];
            _model = new Model();
            List<string> saveList = _model.getSaveList();
            foreach (string save in saveList) { Items.Add(save); }

        }

        public void waiting(string saveName)
        { 
            while(true)
            {
                lock(pauseLock)
                {
                    if (threadPausedStates.ContainsKey(saveName) && !threadPausedStates[saveName])
                    {
                        break;
                    }
                }
            }
        }

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

                bool res = EditSave.Update(save.SourceFilePath, save.TargetFilePath, save.SaveType);

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

        /// <summary>
        /// Adds a new save operation.
        /// </summary>
        public void AddSave()
        {
            string formattedDateTime = DateTime.Now.ToString("MM-dd-yyyy-h-mm-ss");
            string targetPath = OpenFileDest + "\\" + Path.GetFileName(OpenFileSrc) + "-" + formattedDateTime;
            Save save = new(Path.GetFileName(targetPath), "ACTIVE", OpenFileSrc, targetPath,1);
            _model.Datas.Add(save);
            CurrentSave.Add(save.Name);
            EditSave.Create(OpenFileSrc, OpenFileDest, SaveType);
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

            // // Execute the selected save
            List<ModelNS.Save> selectedSaves = new List<ModelNS.Save>();

            // Iterate through selected items
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

            // // Verify if the calculator is open
            IsMetierSoftwareRunning();
            _log = new Log(OpenFileSrc, OpenFileDest, 25);
            ErrorText = _log.AddLog();
            EditSave.Update(OpenFileSrc, OpenFileDest, SaveType);
        }

        /// <summary>
        /// Deletes selected save operations.
        /// </summary>
        /// <param name="list">List of selected items to delete.</param>
        public void DeleteSave_Click(List<string> list)
        {
            List<ModelNS.Save> selectedSaves = new List<ModelNS.Save>();

            // Iterate through selected items
            foreach (string selectedItemName in list)
            {
                // Use LINQ to find the corresponding item in your data model
                ModelNS.Save selectedSave = _model.Datas.FirstOrDefault(item => item.Name == selectedItemName);

                // Check if the item is found (it might be null if no match is found)
                if (selectedSave != null)
                {
                    EditSave.Delete(selectedSave.TargetFilePath);
                    _model.Datas.Remove(selectedSave);
                    Save.Serialize(_model.Datas);
                    Items.Remove(selectedSave.Name);
                }
            }
        }

        /// <summary>
        /// Checks if the business software is running.
        /// </summary>
        private void IsMetierSoftwareRunning()
        {
            // Name of the business software process (adjust according to your case)
            string metierSoftwareProcessName = "CalculatorApp";

            // Check if the process is running
            Process[] processes = Process.GetProcessesByName(metierSoftwareProcessName);
            if (processes.Length > 0)
            {
                ErrorText = "The business software is currently running. Please close it before launching the backup.";
            }
            else
            {
                ErrorText = "Backup job launched successfully.";
            }
        }

    }
}
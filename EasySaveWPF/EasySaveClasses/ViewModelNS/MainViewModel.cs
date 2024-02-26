using System.ComponentModel;
using EasySaveClasses.ModelNS;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using System.Security.AccessControl;
using static System.Net.Mime.MediaTypeNames;
using static EasySaveClasses.ViewModelNS.EditSave;

namespace EasySaveClasses.ViewModelNS
{
    /// <summary>
    /// ViewModel for the main functionality of the application.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        readonly SynchronizationContext? _syncContext = SynchronizationContext.Current;
        private readonly Dictionary<string, Thread> threadsDictionary = new Dictionary<string, Thread>();
        private readonly Dictionary<string, ManualResetEvent> threadsManualResetEvent = new Dictionary<string, ManualResetEvent>();
        private readonly Dictionary<string, CancellationTokenSource> threadsCancelEvent = new Dictionary<string, CancellationTokenSource>();


        static readonly Mutex mutex = new();

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Model _model;
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

        /// <summary>
        /// Getter/Setter of SaveType (1 for full save, 2 for differential)
        /// </summary>
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
            LogManager.Instance.LogStrategyType = "Json";
            List<string> saveList = _model.GetSaveList();
            foreach (string save in saveList) { Items.Add(save); }

        }

        /// <summary>
        /// Executes the work of saving a file.
        /// </summary>
        /// <param name="save">The save object containing details of the save operation.</param>
        /// <param name="syncContext">The synchronization context for UI updates.</param>
        /// <param name="time">The time for which to sleep the thread.</param>
        private void ExecuteWork(Save save, SynchronizationContext syncContext, ManualResetEvent manualEvent, CancellationTokenSource cancelEvent)
        {

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            ResultUpdate res = EditSave.Update(save.SourceFilePath, save.TargetFilePath, save.SaveType, manualEvent, cancelEvent);
            stopwatch.Stop();
            syncContext.Post(state =>
            {
                if (res.Success)
                {
                    save.State = "END";
                    ErrorText = LogManager.Instance.AddLog(save.SourceFilePath, save.TargetFilePath, stopwatch.ElapsedMilliseconds);
                    CurrentSave.Remove(save.Name);
                    save.Progression = res.Progression;
                    Save.Serialize(_model.Datas);
                }
                else
                {
                    save.State = "ABORTED";
                    ErrorText = "ABORT SAVE";
                    ErrorText = LogManager.Instance.AddLog(save.SourceFilePath, save.TargetFilePath, stopwatch.ElapsedMilliseconds);
                    CurrentSave.Remove(save.Name);
                    save.Progression = res.Progression;
                    Save.Serialize(_model.Datas);
                }
            }, null);
            threadsManualResetEvent.Remove(save.Name);
            threadsDictionary.Remove(save.Name);
            threadsCancelEvent.Remove(save.Name);
        }

        public void AbortSave(string saveName) 
        {
            CancellationTokenSource cancelEvent;
            Thread work;
            threadsCancelEvent.TryGetValue(saveName, out cancelEvent);
            cancelEvent.Cancel();      
        }
        public void PauseSave(string saveName) 
        {
            ManualResetEvent manualReset;
            threadsManualResetEvent.TryGetValue(saveName, out manualReset);
            manualReset.Reset();
        }
        public void ResumeSave(string saveName) 
        {
            ManualResetEvent manualReset;
            threadsManualResetEvent.TryGetValue(saveName, out manualReset);
            manualReset.Set();
        }
        /// <summary>
        /// Adds a new save operation.
        /// </summary>
        public void AddSave_Click()
        {
            string formattedDateTime = DateTime.Now.ToString("MM-dd-yyyy-h-mm-ss");
            string targetPath = OpenFileDest + "\\" + Path.GetFileName(OpenFileSrc) + "-" + formattedDateTime;

            Save save = new(Path.GetFileName(targetPath), "NEW", OpenFileSrc, targetPath, SaveType);
            _model.Datas.Add(save);
            Save.Serialize(_model.Datas);
            Items.Add(save.Name);
        }

        /// <summary>
        /// Executes save operation for selected items.
        /// </summary>
        /// <param name="list">List of selected items.</param>
        public void ExecuteSave_Click(List<string> list)
        {

            // // Verify if the calculator is open
            if (!IsMetierSoftwareRunning())
            {
                return;
            }

            List<ModelNS.Save> selectedSaves = [];

            // Iterate through selected items
            foreach (string selectedItemName in list)
            {
                CurrentSave.Add(selectedItemName);
                // Use LINQ to find the corresponding item in your data model
                Save? selectedSave = _model.Datas.FirstOrDefault(item => item.Name == selectedItemName);

                // Check if the item is found (it might be null if no match is found)
                if (selectedSave != null)
                {
                    selectedSave.State = "ACTIVATE";
                    Save.Serialize(_model.Datas);
                    ManualResetEvent manualEvent = new ManualResetEvent(true);
                    CancellationTokenSource cancelEvent = new CancellationTokenSource();
                    threadsManualResetEvent.Add(selectedSave.Name, manualEvent);
                    threadsCancelEvent.Add(selectedSave.Name, cancelEvent);

                    Thread newWork = new(() => ExecuteWork(selectedSave, _syncContext, manualEvent, cancelEvent));

                    string str = selectedSave.Name;
                    if (!IsMetierSoftwareRunning())
                    {
                        PauseSave(str);
                    }

                    threadsDictionary.Add(selectedSave.Name,newWork);


                    newWork.Start();
                }
            }
        }

        /// <summary>
        /// Deletes selected save operations.
        /// </summary>
        /// <param name="listSaves">List of selected saves to delete.</param>
        public void DeleteSave_Click(List<string> listSaves)
        {
            List<Save> selectedSaves = [];

            // Iterate through selected items
            foreach (string selectedSaveName in listSaves)
            {
                // Use LINQ to find the corresponding item in your data model
                Save? selectedSave = _model.Datas.FirstOrDefault(save => save.Name == selectedSaveName);

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
        private bool IsMetierSoftwareRunning()
        {
            // Name of the business software process
            string metierSoftwareProcessName = "CalculatorApp";

            // Check if the process is running
            Process[] processes = Process.GetProcessesByName(metierSoftwareProcessName);
            if (processes.Length > 0)
            {
                ErrorText = "The business software is currently running. Please close it before launching the backup.";
                return false;
            }
            else
            {
                ErrorText = "Backup job launched successfully.";
                return true;
            }
        }

    }
}
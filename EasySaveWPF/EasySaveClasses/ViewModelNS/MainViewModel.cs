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
    /// Component ViewModel of the MVVM, allows to manage the entire application and to make the link between back and front (binding with the UI for example).
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        readonly SynchronizationContext? _syncContext = SynchronizationContext.Current;
        private readonly Dictionary<string, Thread> threadsDictionary = new Dictionary<string, Thread>();
        private readonly Dictionary<string, ManualResetEvent> threadsManualResetEvent = new Dictionary<string, ManualResetEvent>();
        private readonly Dictionary<string, CancellationTokenSource> threadsCancelEvent = new Dictionary<string, CancellationTokenSource>();

        /// <summary>
        /// Model object allowing the link with the application data.
        /// </summary>
        private readonly Model _model;

        static readonly Mutex mutex = new();

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<string> _allSavesNames;
        public ObservableCollection<string> AllSavesNames
        {
            get { return _allSavesNames; }
            set
            {
                _allSavesNames = value;
                OnPropertyChanged(nameof(AllSavesNames));
            }
        }

        private ObservableCollection<string> _priorityExtension;
        public ObservableCollection<string> PriorityExtension
        {
            get { return _priorityExtension; }
            set
            {
                _priorityExtension = value;
                OnPropertyChanged(nameof(PriorityExtension));
                if (_priorityExtension != null)
                    _priorityExtension.CollectionChanged += PriorityExtension_CollectionChanged;
            }
        }

        private ObservableCollection<string> _extensionCrypt;
        public ObservableCollection<string> ExtensionCrypt
        {
            get { return _extensionCrypt; }
            set
            {
                _extensionCrypt = value;
                OnPropertyChanged(nameof(ExtensionCrypt));
                if (_extensionCrypt != null)
                    _extensionCrypt.CollectionChanged += ExtensionCrypt_CollectionChanged;
            }
        }

        private string _errorText;
        /// <summary>
        /// Getter/Setter of the error handling message displayed in the UI
        /// </summary>
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
        /// <summary>
        /// Getter/Setter of the backup selected in the UI
        /// </summary>
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private string _openFolderSrc;
        /// <summary>
        /// 
        /// </summary>
        public string OpenFolderSrc
        {
            get { return _openFolderSrc; }
            set
            {
                _openFolderSrc = value;
                OnPropertyChanged(nameof(OpenFolderSrc));
            }
        }

        private string _openFolderDest;
        public string OpenFolderDest
        {
            get { return _openFolderDest; }
            set
            {
                _openFolderDest = value;
                OnPropertyChanged(nameof(OpenFolderDest));
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

        private ObservableCollection<string> _currentRunningSaves;
        /// <summary>
        /// Collection of saves being run now.
        /// </summary>
        public ObservableCollection<string> CurrentRunningSaves
        {
            get { return _currentRunningSaves; }
            set
            {
                _currentRunningSaves = value;
                OnPropertyChanged(nameof(CurrentRunningSaves));
            }
        }

        private string _currentRunningSaveSelected;
        public string CurrentRunningSaveSelected
        {
            get { return _currentRunningSaveSelected; }
            set
            {
                _currentRunningSaveSelected = value;
                OnPropertyChanged(nameof(CurrentRunningSaveSelected));
            }
        }

        /// <summary>
        /// Constructor initializes necessary properties and loads data.
        /// </summary>
        /// 
        public MainViewModel()
        {
            SaveType = 1;
            CurrentRunningSaves = [];
            AllSavesNames = [];
            _model = new Model();
            string cheminDossier = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../LogDirectory/");
            if (!Directory.Exists(cheminDossier))
            {
                Directory.CreateDirectory(cheminDossier);
            }
            LogManager.Instance.LogStrategyType = "Json";
            List<string> saveList = _model.GetSaveList();
            foreach (string save in saveList) { Items.Add(save); }

            _extensionCrypt = EditSave.ReadExtensionsForEncryptionFromJson();
            _extensionCrypt.CollectionChanged += ExtensionCrypt_CollectionChanged; // Abonnement initial
            _priorityExtension = EditSave.ReadExtensionsForEncryptionFromJson();
            _priorityExtension.CollectionChanged += PriorityExtension_CollectionChanged; // Abonnement initial
            
            
            List<string> saveList = _model.GetSavesNamesList();
            foreach (string save in saveList) { AllSavesNames.Add(save); }
        }
        /// <summary>
        /// Handles changes to the ExtensionCrypt collection.
        /// This method is automatically called when the ExtensionCrypt collection changes.
        /// </summary>
        /// <param name="sender">The object that triggered the event (in this case, the ExtensionCrypt collection).</param>
        /// <param name="e">Details about the modification to the collection.</param>
        private void ExtensionCrypt_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Check if an item was added to the collection
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                // Perform an action for each new item added
                foreach (var newItem in e.NewItems)
                {
                    // Add the extension (you may need to adjust this line to suit your needs)
                    EditSave.InsertExtensions("." + newItem.ToString());
                }
            }
            // Check if an item was removed from the collection
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                // Perform an action for each removed item
                foreach (var oldItem in e.OldItems)
                {
                    // Remove the extension or perform another action
                    EditSave.RemoveExtension("." + oldItem.ToString());
                }
            }
            // You can also handle other action types here (Replace, Move, Reset) if necessary
        }

        private void PriorityExtension_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Check if an item was added to the collection
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                // Perform an action for each new item added
                foreach (var newItem in e.NewItems)
                {
                    // Add the extension (you may need to adjust this line to suit your needs)
                    EditSave.InsertExtensions("." + newItem.ToString());
                }
            }
            // Check if an item was removed from the collection
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                // Perform an action for each removed item
                foreach (var oldItem in e.OldItems)
                {
                    // Remove the extension or perform another action
                    EditSave.RemoveExtension("." + oldItem.ToString());
                }
            }
        }

        /// <summary>
        /// Executes the work of saving a file.
        /// </summary>
        /// <param name="save">The save object containing details of the save operation.</param>
        /// <param name="syncContext">The synchronization context for UI updates.</param>
        /// <param name="time">The time for which to sleep the thread.</param>
        private void ExecuteWork(Save save, SynchronizationContext syncContext, ManualResetEvent manualEvent, CancellationTokenSource cancelEvent)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            ResultUpdate res = EditSave.Update(save.SourceFolderPath, save.TargetFolderPath, save.SaveType, manualEvent, cancelEvent);
            stopwatch.Stop();
            syncContext.Post(state =>
            {
                if (res.Success)
                {
                    save.State = "END";
                    ErrorText = LogManager.Instance.AddLog(save.SourceFolderPath, save.TargetFolderPath, stopwatch.ElapsedMilliseconds);
                    CurrentRunningSaves.Remove(save.Name);
                    save.Progression = res.Progression;
                    Save.Serialize(_model.Datas);
                }
                else
                {
                    save.State = "ABORTED";
                    ErrorText = "ABORT SAVE";
                    ErrorText = LogManager.Instance.AddLog(save.SourceFolderPath, save.TargetFolderPath, stopwatch.ElapsedMilliseconds);
                    CurrentRunningSaves.Remove(save.Name);
                    save.Progression = res.Progression;
                    Save.Serialize(_model.Datas);
                }
            }, null);

            threadsManualResetEvent.Remove(save.Name);
            threadsDictionary.Remove(save.Name);
            threadsCancelEvent.Remove(save.Name);
        }

        /// <summary>
        /// Cancel the specified save.
        /// </summary>
        /// <param name="saveName">The save to cancel</param>
        public void AbortSave(string saveName) 
        {
            threadsCancelEvent.TryGetValue(saveName, out CancellationTokenSource cancelEvent);
            cancelEvent.Cancel();      
        }

        /// <summary>
        /// Pause the specified save.
        /// </summary>
        /// <param name="saveName">The save to pause</param>
        public void PauseSave(string saveName) 
        {
            ManualResetEvent manualReset;
            threadsManualResetEvent.TryGetValue(saveName, out manualReset);
            manualReset.Reset();
        }

        /// <summary>
        /// Resume the specified save.
        /// </summary>
        /// <param name="saveName">The save to resume</param>
        public void ResumeSave(string saveName) 
        {
            threadsManualResetEvent.TryGetValue(saveName, out ManualResetEvent manualReset);
            manualReset.Set();
        }
        /// <summary>
        /// Adds a new save operation.
        /// </summary>
        public void AddSave_Click()
        {
            string formattedDateTime = DateTime.Now.ToString("MM-dd-yyyy-h-mm-ss");
            string targetPath = OpenFolderDest + "\\" + Path.GetFileName(OpenFolderSrc) + "-" + formattedDateTime;

            Save save = new(Path.GetFileName(targetPath), "NEW", OpenFolderSrc, targetPath, SaveType);
            _model.Datas.Add(save);
            Save.Serialize(_model.Datas);
            AllSavesNames.Add(save.Name);
        }


        /// <summary>
        /// Executes save operation for selected items.
        /// </summary>
        /// <param name="list">List of selected items.</param>
        public void ExecuteSave_Click(List<string> list)
        {
            // // Verify if the calculator is open
            if (IsBusinessSoftwareRunning())
            {
                return;
            }

            // Iteration through all selected backups
            foreach (string selectedItemName in list)
            {
                // Use LINQ to find the corresponding item in the data model
                Save? selectedSave = _model.Datas.FirstOrDefault(item => item.Name == selectedItemName);

                // Checks if the element is found (it can be null if no match is found)
                if (selectedSave != null)
                {
                    // If the source folder still exists, launch the backup
                    if (Directory.Exists(selectedSave.SourceFolderPath))
                    {
                        CurrentRunningSaves.Add(selectedItemName);
                        selectedSave.State = "ACTIVATE";
                        Save.Serialize(_model.Datas);
                        ManualResetEvent manualEvent = new(true);
                        CancellationTokenSource cancelEvent = new CancellationTokenSource();
                        threadsManualResetEvent.Add(selectedSave.Name, manualEvent);
                        threadsCancelEvent.Add(selectedSave.Name, cancelEvent);

                        // Creation of a new thread for the current save
                        Thread newWork = new(() => ExecuteWork(selectedSave, _syncContext, manualEvent, cancelEvent));

                        string str = selectedSave.Name;
                        if (IsBusinessSoftwareRunning())
                        {
                            PauseSave(str);
                        }

                        threadsDictionary.Add(selectedSave.Name, newWork);
                        newWork.Start();
                    }
                    // If the source folder no longer exists, delete the backup
                    else
                    {
                        EditSave.Delete(selectedSave.TargetFolderPath);
                        _model.Datas.Remove(selectedSave);
                        Save.Serialize(_model.Datas);
                        AllSavesNames.Remove(selectedSave.Name);
                        // Display error on UI
                        ErrorText = selectedSave.Name + " : Source path doesn't exist anymore (" + selectedSave.SourceFolderPath + ")";
                    }
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
                    EditSave.Delete(selectedSave.TargetFolderPath);
                    _model.Datas.Remove(selectedSave);
                    Save.Serialize(_model.Datas);
                    AllSavesNames.Remove(selectedSave.Name);
                }
            }
        }

        /// <summary>
        /// Checks if the business software is running.
        /// </summary>
        private bool IsBusinessSoftwareRunning()
        {
            // Name of the business process
            string businessSoftwareProcessName = "Notepad.exe";

            // Check if the process is running
            Process[] processes = Process.GetProcessesByName(businessSoftwareProcessName);
            if (processes.Length > 0)
            {
                ErrorText = "The business software is currently running. Please close it before launching the backup.";
                return true;
            }
            else
            {
                ErrorText = "Backup job launched successfully.";
                return false;
            }
        }

    }
}
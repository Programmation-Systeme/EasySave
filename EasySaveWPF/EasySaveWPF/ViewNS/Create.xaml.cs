using EasySaveClasses.ViewModelNS;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;

namespace EasySaveWPF.ViewNS
{
    /// <summary>
    /// Interaction logic for Create.xaml
    /// </summary>
    public partial class Create : Page
    {
        MainWindow _mainWindow;
        MainViewModel _mainViewModel;
        public Create(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _mainViewModel = _mainWindow.mainViewModel;
            DataContext = _mainViewModel;
            InitializeComponent();
        }
        private void OpenFolderSource_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Multiselect = false;
            openFolderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFolderDialog.ShowDialog() == true)
            {
                _mainViewModel.OpenFileSrc = openFolderDialog.FolderName;
            }
        }
        private void OpenFolderDest_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Multiselect = false;
            openFolderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFolderDialog.ShowDialog() == true)
            {
                _mainViewModel.OpenFileDest = openFolderDialog.FolderName;
            }
        }
        private void SaveTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (saveTypeComboBox.SelectedIndex == 0)
                {
                _mainViewModel.SaveType = 1;
            }
            else if (saveTypeComboBox.SelectedIndex == 1)
            {
                _mainViewModel.SaveType = 2;
            }
            else
            {
                return;
            }
        }

        private void btnAddSave_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModel.AddSave_Click();
            _mainViewModel.OpenFileSrc = null;
            _mainViewModel.OpenFileDest = null;
            saveTypeComboBox.SelectedIndex = 0;
        }
    }
}

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
            InitializeComponent();
            _mainWindow = mainWindow;
            _mainViewModel = _mainWindow.mainViewModel;
            DataContext = _mainViewModel;
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
            if (saveTypeComboBox.SelectedItem == null)
                return;

            ComboBoxItem selectedItem = (ComboBoxItem)saveTypeComboBox.SelectedItem;
            if (selectedItem.Content.ToString() == "Différentiel")
            {
                _mainViewModel.SaveType = 1;
            }
            else if (selectedItem.Content.ToString() == "Complète")
            {
                _mainViewModel.SaveType = 2;
            }
        }

        private void btnAddSave_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModel.AddSave();
        }
    }
}

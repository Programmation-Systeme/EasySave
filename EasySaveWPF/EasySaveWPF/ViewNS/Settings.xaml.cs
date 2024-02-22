using EasySaveClasses.ViewModelNS;
using System.Windows.Controls;

namespace EasySaveWPF.ViewNS
{
    /// <summary>
    /// Logique d'interaction pour Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        MainWindow _mainWindow;
        MainViewModel _mainViewModel;
        public Settings(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _mainViewModel = _mainWindow.mainViewModel;
            DataContext = _mainViewModel;
        }

        private void ComboBox_LanguageChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null && _mainWindow != null)
            {
                if (selectedItem.Content.ToString() == "Francais")
                {
                    _mainWindow.LoadLanguage_Fr();
                }
                if (selectedItem.Content.ToString() == "English")
                {
                    _mainWindow.LoadLanguage_En();
                }
            }
        }

    }
}

using EasySaveClasses.ViewModelNS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

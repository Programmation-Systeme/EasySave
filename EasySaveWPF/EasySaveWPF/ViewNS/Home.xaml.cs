using System;
using System.Collections;
using System.Collections.Generic;
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
using EasySaveClasses.ViewModelNS;

namespace EasySaveWPF.ViewNS
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        MainWindow _mainWindow;
        MainViewModel _mainViewModel;
        PlayBreak _playBreak;
        public Home(MainWindow mainWindow, PlayBreak playBreak)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _mainViewModel = _mainWindow.mainViewModel;
            DataContext = _mainViewModel;
            _playBreak = playBreak;
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            foreach(string element in ItemSelecteds.SelectedItems)
            { list.Add(element); }
            _mainViewModel.DeleteSave_Click(list);
        }

        private void ExecutExecute_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            foreach (string element in ItemSelecteds.SelectedItems)
            { list.Add(element); }
            _mainViewModel.ExecuteSave_Click(list);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

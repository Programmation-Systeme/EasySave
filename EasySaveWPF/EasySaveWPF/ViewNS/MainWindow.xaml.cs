using System.Text;
using System.Windows;
using System;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel mainViewModel;
        public MainWindow()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel();
            MainFrame.Navigate(new Home(this));
        }
        private void MenuItem_Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Home(this));
        }
        private void MenuItem_Create_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Create(this));
        }
    }
}
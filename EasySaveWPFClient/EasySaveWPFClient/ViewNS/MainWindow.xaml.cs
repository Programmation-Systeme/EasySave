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
using System.Globalization;
using System.Windows.Resources;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Net.Sockets;

namespace EasySaveWPF.ViewNS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel _mainViewModel;
        public string _socketDataList;

        public MainWindow()
        {
            InitializeComponent();
            LoadLanguage_En();
            _mainViewModel = new MainViewModel();
            DataContext = _mainViewModel;
            Dispatcher mainDispatcher = Dispatcher.CurrentDispatcher;
            Play.Content = "▶";
            _mainViewModel.LaunchSocketClient("1");
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (ItemSelecteds.SelectedItem != null)
            {
                // Cast de l'élément sélectionné en ListBoxItem
                ListBoxItem selectedItem = ItemSelecteds.ItemContainerGenerator.ContainerFromItem(ItemSelecteds.SelectedItem) as ListBoxItem;
                if (selectedItem != null)
                {
                    _mainViewModel.LaunchSocketClient(selectedItem.ToString());
                }

                if (selectedItem.Foreground == Brushes.Red)
                {
                    selectedItem.Foreground = Brushes.Black;
                    Play.Content = "▶";
                }
                else if (selectedItem.Foreground == Brushes.Black)
                {
                    selectedItem.Foreground = Brushes.Red;
                    Play.Content = "⏸";
                }
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ItemSelecteds.SelectedItem != null)
            {
                _mainViewModel.AllSocketSavesNames.Clear();
            }
        }

        private void LoadLanguage(string relativePath)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Uri uri = new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath), UriKind.Absolute);
            using (FileStream fs = new FileStream(uri.LocalPath, FileMode.Open))
            {
                System.Windows.Markup.XamlReader reader = new System.Windows.Markup.XamlReader();
                ResourceDictionary myResourceDictionary = (ResourceDictionary)reader.LoadAsync(fs);
                Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary);
            }
        }

        public void LoadLanguage_Fr()
        {
            LoadLanguage("../../../Properties/French.xaml");
        }

        public void LoadLanguage_En()
        {
            LoadLanguage("../../../Properties/English.xaml");
        }

        private void MenuItem_Language_En(object sender, RoutedEventArgs e)
        {
            LoadLanguage_En();
        }

        private void MenuItem_Language_Fr(object sender, RoutedEventArgs e)
        {
            LoadLanguage_Fr();
        }
    }
}
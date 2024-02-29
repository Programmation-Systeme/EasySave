using EasySaveClasses.ViewModelNS;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EasySaveWPF.ViewNS
{
    /// <summary>
    /// Interaction logic for Execution.xaml
    /// </summary>
    public partial class Execution : Page
    {
        private ObservableCollection<string> items = new ObservableCollection<string>();

        MainWindow _mainWindow;
        MainViewModel _mainViewModel;
        public Execution(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _mainViewModel = _mainWindow._mainViewModel;
            DataContext = _mainViewModel;
            Play.Content = "▶";
        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSave.SelectedItem != null)
            {
                // Cast de l'élément sélectionné en ListBoxItem
                ListBoxItem selectedItem = CurrentSave.ItemContainerGenerator.ContainerFromItem(CurrentSave.SelectedItem) as ListBoxItem;
                if(selectedItem.Foreground == Brushes.Red)
                {
                    selectedItem.Foreground = Brushes.Black;
                    Play.Content = "▶";
                }
                else if(selectedItem.Foreground == Brushes.Black)
                {
                    selectedItem.Foreground = Brushes.Red;
                    Play.Content = "⏸";
                }
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSave.SelectedItem != null)
            {
                _mainViewModel.CurrentRunningSaves.Remove((string)CurrentSave.SelectedItem);
            }
        }

        //private void SocketClient_DataReceived(object sender, string[] outputs)
        //{
        //    // Update CurrentSave in MainViewModel with received data
        //    Application.Current.Dispatcher.Invoke(() =>
        //    {
        //        foreach (var output in outputs)
        //        {
        //            _mainViewModel.CurrentRunningSaves.Add(output);
        //        }
        //    });
        //}

    }
}

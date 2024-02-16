using EasySaveClasses.ViewModelNS;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasySaveWPF.ViewNS
{
    public partial class PlayBreak : Window
    {
        private ObservableCollection<string> items = new ObservableCollection<string>();

        MainWindow _mainWindow;
        MainViewModel _mainViewModel;
        public PlayBreak(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _mainViewModel = _mainWindow.mainViewModel;
            DataContext = _mainViewModel;
            Play.Content = "▶";

        }


        private void Play_Click(object sender, RoutedEventArgs e)
        {
            switch (Play.Content)
            {
                case "▶":
                    Play.Content = "⏸";
                    break;

                case "⏸":
                    Play.Content = "▶";
                    break;

                default:
                    break;
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSave.SelectedItem != null)
            {
                _mainViewModel.CurrentSave.Remove((string)CurrentSave.SelectedItem);
            }
        }

    }
}

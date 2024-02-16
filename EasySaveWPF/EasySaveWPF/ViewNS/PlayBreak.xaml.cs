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
            var itemsToRemove = new List<CurrentSave>();

            foreach (var selectedItem in listBox.SelectedItems)
            {
                if (selectedItem is CurrentSave currentSave)
                {
                    itemsToRemove.Add(currentSave);
                }
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                _mainViewModel.CurrentSave.Remove(itemToRemove);
            }
        }

        private void CanDelete_Click(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = listBox.SelectedItems.Count > 0;
        }
    }
}

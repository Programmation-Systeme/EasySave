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
            _mainViewModel = _mainWindow.mainViewModel;
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
                    _mainViewModel.ResumeSave(saveName: CurrentSave.SelectedItem.ToString());

                    Play.Content = "▶";
                }
                else if(selectedItem.Foreground == Brushes.Black)
                {
                    selectedItem.Foreground = Brushes.Red;
                    _mainViewModel.PauseSave(saveName: CurrentSave.SelectedItem.ToString());

                    Play.Content = "⏸";
                }
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSave.SelectedItem != null)
            {
                _mainViewModel.AbortSave(saveName: CurrentSave.SelectedItem.ToString());
                _mainViewModel.CurrentRunningSaves.Remove((string)CurrentSave.SelectedItem);
            }
        }

    }
}

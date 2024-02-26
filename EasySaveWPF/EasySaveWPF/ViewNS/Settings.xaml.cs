using EasySaveClasses.ViewModelNS;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasySaveWPF.ViewNS
{
    /// <summary>
    /// Logique d'interaction pour Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        MainWindow _mainWindow;
        MainViewModel _mainViewModel;
        ObservableCollection<string> items = new ObservableCollection<string>();

        public Settings(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _mainViewModel = _mainWindow.mainViewModel;
            DataContext = _mainViewModel;

            ItemsControl.ItemsSource = items;
            // Ajouter quelques éléments initiaux, si nécessaire
            items.Add("Élément 1");
            items.Add("Élément 2");
        }
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string item = button.DataContext as string;
            items.Remove(item);
        }

        private void ItemsListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Ici, vous pouvez ajouter la logique pour déterminer et ajouter un nouvel élément
            // Par exemple, vous pouvez ouvrir une boîte de dialogue pour saisir le nom de l'élément
            string newItem = $"Nouvel élément {items.Count + 1}";
            items.Add(newItem);
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
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            string newItem = NewItemTextBox.Text;
            if (!string.IsNullOrWhiteSpace(newItem))
            {
                items.Add(newItem);
                NewItemTextBox.Text = ""; // Efface le TextBox après l'ajout
            }
        }

        private void NewItemTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddNewItem();
            }
        }

        private void NewItemTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NewItemTextBox.Text == "Écrivez l'extension à rentrer ici")
            {
                NewItemTextBox.Text = "";
            }
        }

        private void NewItemTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewItemTextBox.Text))
            {
                NewItemTextBox.Text = "Écrivez l'extension à rentrer ici";
            }
        }

        private void AddItem2_Click(object sender, RoutedEventArgs e)
        {
            AddNewItem();
        }

        private void AddNewItem()
        {
            string newItem = NewItemTextBox.Text.Trim();

            // Vérification que l'entrée ne contient que des chiffres et des lettres
            if (!string.IsNullOrWhiteSpace(newItem) && Regex.IsMatch(newItem, @"^[a-zA-Z0-9]+$"))
            {
                items.Add(newItem);
                NewItemTextBox.Text = ""; // Efface le TextBox après l'ajout
            }
            else
            {
                MessageBox.Show("L'entrée doit contenir uniquement des lettres et des chiffres.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}

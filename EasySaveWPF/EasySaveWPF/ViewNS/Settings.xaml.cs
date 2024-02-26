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
                AddNewItem();
                NewItemTextBox.Text = ""; // Efface le TextBox après l'ajout
            }
        }

        private void AddNewItem()
        {
            string newItem = NewItemTextBox.Text.Trim();
            // Vérifie si 'newItem' est non vide, non nul et alphanumérique
            if (!string.IsNullOrWhiteSpace(newItem) && Regex.IsMatch(newItem, @"^[a-zA-Z0-9]+$"))
            {
                switch (newItem.ToLower())
                {
                    case "hash":
                    case "encrypted":
                        // Affiche un message d'erreur si 'newItem' est "hash" ou "encrypted"
                        MessageBox.Show("L'utilisation de '.hash' ou '.encrypted' est interdite.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    default:
                        // Exécute le code suivant si 'newItem' n'est ni "hash" ni "encrypted"
                        // Vérifie si l'élément existe déjà dans la liste
                        if (!items.Contains(newItem))
                        {
                            // Ajoute 'newItem' à la liste si ce n'est pas un doublon
                            items.Add(newItem);
                            NewItemTextBox.Text = ""; // Efface le contenu de 'NewItemTextBox' après l'ajout
                        }
                        else
                        {
                            // Affiche un message d'erreur si l'élément existe déjà dans la liste
                            MessageBox.Show("Cet élément existe déjà.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                }
            }
            else
            {
                // Affiche un message d'erreur si 'newItem' ne respecte pas le format alphanumérique
                MessageBox.Show("L'entrée doit contenir uniquement des lettres et des chiffres.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
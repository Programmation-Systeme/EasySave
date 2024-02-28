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

        public Settings(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _mainViewModel = _mainWindow.mainViewModel;
            DataContext = _mainViewModel;
        }
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string item = button.DataContext as string;
            _mainViewModel.ExtensionCrypt.Remove(item);
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
        private void AddExtensionCrypt_Click(object sender, RoutedEventArgs e)
        {
            string newItem = ExtensionCryptTextBox.Text;
            if (!string.IsNullOrWhiteSpace(newItem))
            {
                AddNewItem(ExtensionCryptTextBox.Text.Trim());
                ExtensionCryptTextBox.Text = ""; // Efface le TextBox après l'ajout
            }
        }
        private void PriorityExtension_Click(object sender, RoutedEventArgs e)
        {
            string newItem = PriorityExtensionTextBox.Text;
            if (!string.IsNullOrWhiteSpace(newItem))
            {
                AddNewItem(PriorityExtensionTextBox.Text.Trim());
                PriorityExtensionTextBox.Text = ""; // Efface le TextBox après l'ajout
            }
        }

        private void AddNewItem(string newItem)
        {
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
                        if (!_mainViewModel.ExtensionCrypt.Contains(newItem))
                        {
                            // Ajoute 'newItem' à la liste si ce n'est pas un doublon
                            _mainViewModel.ExtensionCrypt.Add(newItem);
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

        private void ComboBox_LogFormatChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null && _mainWindow != null)
            {
                LogManager.Instance.LogStrategyType = selectedItem.Content.ToString();
            }
        }
        
    }
}
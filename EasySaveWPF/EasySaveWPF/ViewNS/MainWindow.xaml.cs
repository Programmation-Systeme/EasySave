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

namespace EasySaveWPF.ViewNS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel mainViewModel;
        PlayBreak _playBreak;
        public MainWindow()
        {
            InitializeComponent();
            LoadLanguage_En();

            mainViewModel = new MainViewModel();
            PlayBreak playBreak = new PlayBreak(this);
            Home.Navigate(new Home(this, playBreak));
            Create.Navigate(new Create(this));
            PlayBreak.Navigate(new Execution(this));
        }
        private void ComboBox_LanguageChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                if (selectedItem.Content.ToString() == "Francais")
                {
                    LoadLanguage_Fr();
                }
                if (selectedItem.Content.ToString() == "English")
                {
                    LoadLanguage_En();
                }
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

        private void LoadLanguage_Fr()
        {
            LoadLanguage("../../../Properties/French.xaml");
        }

        private void LoadLanguage_En()
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
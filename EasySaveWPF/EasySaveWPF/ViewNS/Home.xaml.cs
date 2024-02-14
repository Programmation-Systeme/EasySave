using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EasySaveWPF.ViewModelNS;

namespace EasySaveWPF.ViewNS
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {

        public Home()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private List<string> GetSelectedItems()
        {
            List<string> selectedItems = new List<string>();

            // Parcourir les éléments sélectionnés dans le ListBox
            foreach (var selectedItem in ItemSelected.SelectedItems)
            {
                // Ajouter les éléments sélectionnés à la liste
                selectedItems.Add(selectedItem.ToString());
            }

            return selectedItems;
        }

        private void OnClick_Exe(object sender, RoutedEventArgs e)
        {
            List<string> itemsSelected = GetSelectedItems();
            string result = "";

            foreach (string element in itemsSelected)
            {
                result += element + " ";
            }
            
        }
    }
}

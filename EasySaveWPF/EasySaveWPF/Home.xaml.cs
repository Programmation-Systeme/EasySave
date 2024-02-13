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

namespace EasySaveWPF
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            PopulateListBox();
        }
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            //NavigateServices?.Navigate(new Ouvrir());
        }
        private void PopulateListBox()
        {
            ItemSelected.Items.Add("Item 1");
            ItemSelected.Items.Add("Item 2");
            ItemSelected.Items.Add("Item 3");
            ItemSelected.Items.Add("Item 1");
            ItemSelected.Items.Add("Item 2");
            ItemSelected.Items.Add("Item 3");
            ItemSelected.Items.Add("Item 1");
            ItemSelected.Items.Add("Item 2");
            ItemSelected.Items.Add("Item 3");
            ItemSelected.Items.Add("Item 1");
            ItemSelected.Items.Add("Item 2");
            ItemSelected.Items.Add("Item 3");
            ItemSelected.Items.Add("Item 1");
            ItemSelected.Items.Add("Item 2");
            ItemSelected.Items.Add("Item 3");
            ItemSelected.Items.Add("Item 1");
            ItemSelected.Items.Add("Item 2");
            ItemSelected.Items.Add("Item 3");
            ItemSelected.Items.Add("Item 1");
            ItemSelected.Items.Add("Item 2");
            ItemSelected.Items.Add("Item 3");
            ItemSelected.Items.Add("Item 1");
            ItemSelected.Items.Add("Item 2");
            ItemSelected.Items.Add("Item 3");
            ItemSelected.Items.Add("Item 1");
            ItemSelected.Items.Add("Item 2");
            ItemSelected.Items.Add("Item 3");
            ItemSelected.Items.Add("Item 1");
            ItemSelected.Items.Add("Item 2");
            ItemSelected.Items.Add("Item 3");
            ItemSelected.Items.Add("Item 1");
            ItemSelected.Items.Add("Item 2");
            ItemSelected.Items.Add("Item 3");
            ItemSelected.Items.Add("Item 1");
            ItemSelected.Items.Add("Item 2");
            ItemSelected.Items.Add("Item 3");
        }
    }
}

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
using System.Windows.Shapes;

namespace SpionshopWPFApp.View
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void btnCategorieen_Click(object sender, RoutedEventArgs e)
        {
            var win = new CategorieWindow();
            win.Show();
            this.Close();
        }

        private void btnArtikels_Click(object sender, RoutedEventArgs e)
        {
            var win = new ArtikelWindow();
            win.Show();
            this.Close();
        }

        private void btnBestellingen_Click(object sender, RoutedEventArgs e)
        {
            var win = new BestellingWindow();
            win.Show();
            this.Close();
        }
    }
}

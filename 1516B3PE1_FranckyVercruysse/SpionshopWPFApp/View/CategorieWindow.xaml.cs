using SpionshopWPFApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SpionshopWPFApp.View
{
    /// <summary>
    /// Interaction logic for CategorieWindow.xaml
    /// </summary>
    public partial class CategorieWindow : Window
    {
        HttpClient httpClient = new HttpClient();
        string categoriesUri = "/api/Categorie/";

        public CategorieWindow()
        {
            InitializeComponent();

            httpClient.BaseAddress = new Uri("http://spionshopapi2.azurewebsites.net/");    //server : omecc7czw4.database.windows.net,1433 database : database : Spionshop
            //httpClient.BaseAddress = new Uri("http://spionshop.azurewebsites.net/");      //SQL Server 2012 : SQL3.hostedSQL.be database : sofraverbe
            //httpClient.BaseAddress = new Uri("http://localhost:56575/");                        // dit is een vast adres -- zie uitleg in Solution Items 
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.Loaded += CategorieWindow_Loaded;
        }

        async void CategorieWindow_Loaded(object sender, RoutedEventArgs e)
        {
            categorieenListView.ItemsSource = await GetAlleCategorieen();
        }

        private void btnVoegToe_Click(object sender, RoutedEventArgs e)
        {
            stkAanpassen.Visibility = Visibility.Hidden;
            stkVoegToe.Visibility = Visibility.Visible;
            txtCategorie.Focus();
        }

        private async void btnPost_Click(object sender, RoutedEventArgs e)
        {
            CategoriePOCO categoriePOCO = new CategoriePOCO();
            categoriePOCO.Categorie1 = txtCategorie.Text;

            try
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<CategoriePOCO>(categoriesUri, categoriePOCO);
                response.EnsureSuccessStatusCode();     // Throw on error code.
                MessageBox.Show("Categorie succesvol in database weggeschreven", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                categorieenListView.ItemsSource = await GetAlleCategorieen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            stkVoegToe.Visibility = Visibility.Hidden;

        }

        private void categorieenListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            stkVoegToe.Visibility = Visibility.Hidden;
            stkAanpassen.Visibility = Visibility.Visible;
            var item = (sender as ListView).SelectedItem;

            var categoriePOCO = (CategoriePOCO) item;
            if (item != null)
            {
                txtCategorieId_A.Text = categoriePOCO.Cat_id.ToString();
                txtCategorie_A.Text = categoriePOCO.Categorie1;
            }
        }

        private async void btnCategoriePut_Click(object sender, RoutedEventArgs e)
        {
            CategoriePOCO categoriePOCO = new CategoriePOCO();
            categoriePOCO.Cat_id = short.Parse(txtCategorieId_A.Text);
            categoriePOCO.Categorie1 = txtCategorie_A.Text;
            try
            {
                var response = await httpClient.PutAsJsonAsync<CategoriePOCO>(categoriesUri + "?id=" + categoriePOCO.Cat_id, categoriePOCO);
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Categorie met succes aangepast in database", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                categorieenListView.ItemsSource = await GetAlleCategorieen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                stkAanpassen.Visibility = Visibility.Hidden;
            }
        }

        private async void btnCategorieDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await httpClient.DeleteAsync(categoriesUri + "?id=" + txtCategorieId_A.Text);
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Categorie met succes gewist in database", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                categorieenListView.ItemsSource = await GetAlleCategorieen();
                stkAanpassen.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task<IEnumerable> GetAlleCategorieen()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(categoriesUri);
                response.EnsureSuccessStatusCode(); // Throw on error code.
                var categorieen = await response.Content.ReadAsAsync<IEnumerable<CategoriePOCO>>();
                return categorieen;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return Enumerable.Empty<CategoriePOCO>();
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            var win = new MenuWindow();
            win.Show();
            this.Close();
        }
    }
}

using SpionshopWPFApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
    /// Interaction logic for BestellingWindow.xaml
    /// </summary>
    public partial class BestellingWindow : Window
    {
        HttpClient httpClient = new HttpClient();
        String bestellingUri = "/api/bestelling";

        public BestellingWindow()
        {
            InitializeComponent();

            httpClient.BaseAddress = new Uri("http://spionshopapi2.azurewebsites.net/");    //server : omecc7czw4.database.windows.net,1433 database : database : Spionshop
            //httpClient.BaseAddress = new Uri("http://spionshop.azurewebsites.net/");      //SQL Server 2012 : SQL3.hostedSQL.be database : sofraverbe (delete !!!)
            //httpClient.BaseAddress = new Uri("http://localhost:56575/");        // dit is een vast adres -- zie uitleg vast poort nummer in Solution Items
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.Loaded += BestellingWindow_Loaded;
        }

        async void BestellingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            bestellingDataGrid.ItemsSource = await GetAlleBestellingen();
        }

        private async Task<IEnumerable> GetAlleBestellingen()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(bestellingUri);
                response.EnsureSuccessStatusCode(); // Throw on error code.
                var bestellingen = await response.Content.ReadAsAsync<IEnumerable<BestellingKlantPOCO>>();
                return bestellingen;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return Enumerable.Empty<BestellingPOCO>();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            stkBestellingDetail.Visibility = Visibility.Visible;
            try
            {
                DataGrid datagrid = (DataGrid)sender;
                var bestellingDetailPOCO = (BestellingDetailPOCO)datagrid.CurrentItem;

                txtAantal.Text = bestellingDetailPOCO.Aantal.ToString();
                txtArtikel_id.Text = bestellingDetailPOCO.Artikel_id.ToString();
                txtArtikel1.Text = bestellingDetailPOCO.Artikel1;
                txtB_id.Text = bestellingDetailPOCO.B_id.ToString();
                txtBD_id.Text = bestellingDetailPOCO.BD_id.ToString();
            }
            catch
            {
                MessageBox.Show("Fout opgetreden : waarschijnlijk werden er artikels geschrapt uit spionshop");
            }
        }

        private async void btnArtikelDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await httpClient.DeleteAsync(bestellingUri + "?id=" + txtBD_id.Text);
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Artikel met succes gewist in database", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                bestellingDataGrid.ItemsSource = await GetAlleBestellingen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            stkBestellingDetail.Visibility = Visibility.Hidden;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            stkBestellingDetail.Visibility = Visibility.Hidden;
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            var win = new MenuWindow();
            win.Show();
            this.Close();
        }
    }
}

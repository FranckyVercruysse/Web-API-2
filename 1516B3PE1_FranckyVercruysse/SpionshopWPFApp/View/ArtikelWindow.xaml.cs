using Newtonsoft.Json;
using SpionshopWPFApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ArtikelWindow.xaml
    /// </summary>
    public partial class ArtikelWindow : Window
    {
        HttpClient httpClient = new HttpClient();
        string artikelsUri = "/api/Artikels/";
        string categoriesUri = "/api/Categorie/";
        string imageUri = "/bytesArray/";
        IEnumerable<ArtikelPOCO> artikels;

        public ArtikelWindow()
        {
            InitializeComponent();

            httpClient.BaseAddress = new Uri("http://spionshopapi2.azurewebsites.net/");    //server : omecc7czw4.database.windows.net,1433 database : database : Spionshop
            //httpClient.BaseAddress = new Uri("http://spionshop.azurewebsites.net/");      //SQL Server 2012 : SQL3.hostedSQL.be database : sofraverbe
            //httpClient.BaseAddress = new Uri("http://localhost:56575/");        // dit is een vast adres -- zie uitleg vast poort nummer in Solution Items
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.Loaded += ArtikelWindow_Loaded;
        }

        async void ArtikelWindow_Loaded(object sender, RoutedEventArgs e)
        {
            artikelsListView.ItemsSource = await GetAllArtikels();

        }

        private async void artikelsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            stkVoegToe.Visibility = Visibility.Hidden;
            stkAanpassen.Visibility = Visibility.Visible;
            var item = (sender as ListView).SelectedItem;

            var artikelPOCO = (ArtikelPOCO)item;
            if (item != null)
            {
                txtArtikel_id_A.Text = artikelPOCO.Artikel_id.ToString();
                cbxCat_id_A.SelectedValue = artikelPOCO.Cat_id.ToString();
                txtArtikel_A.Text = artikelPOCO.Artikel1;
                txtOmschrijving_A.Text = artikelPOCO.Omschrijving;
                txtVerkoopprijs_A.Text = artikelPOCO.Verkoopprijs.ToString();
                txtInstock_A.Text = artikelPOCO.Instock.ToString();
                kiesAfbeelding_A_ListView.ItemsSource = await GetAllAfbeeldingen();

                Image_A_Oud.Source = await GetImage(artikelPOCO.Artikel_id.ToString(), "/Images");   // uit map waar de afbeeldingen staan die gekoppeld zijn aan het artikel
                Image_A_Nieuw.Source = null;
            }
        }

        async private void btnArtikelToevoegen_Click(object sender, RoutedEventArgs e)
        {
            stkAanpassen.Visibility = Visibility.Hidden;
            stkVoegToe.Visibility = Visibility.Visible;

            kiesAfbeelding_T_ListView.ItemsSource = await GetAllAfbeeldingen();
        }

        async private void btnArtikelPut_Click(object sender, RoutedEventArgs e)
        {                                       // na het aanpassen van het artikel, wegschrijven naar databate met http method put
            ArtikelPOCO artikelPOCO = new ArtikelPOCO();
            artikelPOCO.Artikel_id = short.Parse(txtArtikel_id_A.Text);

            ComboBoxPairs cbp = (ComboBoxPairs)cbxCat_id_A.SelectedItem;
            string _key = cbp._Key;                 //      string _value = cbp._Value;
            artikelPOCO.Cat_id = short.Parse(_key);

            artikelPOCO.Artikel1 = txtArtikel_A.Text;
            artikelPOCO.Omschrijving = txtOmschrijving_A.Text;
            artikelPOCO.Verkoopprijs = decimal.Parse(txtVerkoopprijs_A.Text);
            artikelPOCO.Instock = short.Parse(txtInstock_A.Text);

            try
            {
                var response = await httpClient.PutAsJsonAsync<ArtikelPOCO>(artikelsUri + "?id=" + artikelPOCO.Artikel_id, artikelPOCO);
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Artikel met succes aangepast in database", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                artikelsListView.ItemsSource = await GetAllArtikels();
                if (Image_A_Nieuw.Source != null)           // Er werd een nieuwe afbeelding gekozen voor dit artikel
                {
                    try
                    {
                        string uri = "CopyAfbeelding/" + tbGekozenAfbeeldingA.Text + "/" + artikelPOCO.Artikel_id;
                        response = await httpClient.GetAsync(uri);
                        response.EnsureSuccessStatusCode();     // Throw on error code.
                        MessageBox.Show("Nieuwe afbeelding gekozen");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                txtArtikel_A.Text = "";
                txtOmschrijving_A.Text = "";
                txtVerkoopprijs_A.Text = "";
                txtInstock_A.Text = "";
                cbxCat_id_A.Text = "--Select a Categorie--";
                Image_A_Oud.Source = null;
                Image_A_Nieuw.Source = null;
                tbGekozenAfbeeldingA.Text = String.Empty;
                stkAanpassen.Visibility = Visibility.Hidden;
            }
        }

        async private void btnArtikelPost_Click(object sender, RoutedEventArgs e)
        {
            ArtikelPOCO artikelPOCO = new ArtikelPOCO();

            artikelPOCO.Artikel1 = txtArtikelT.Text;
            artikelPOCO.Omschrijving = txtOmschrijvingT.Text;
            try
            {       //  Indien geen selectie van categorie gemaakt, dan krijgen we een foutmelding.
                ComboBoxPairs cbp = (ComboBoxPairs)cbxCat_id_T.SelectedItem;
                string _key = cbp._Key;         //      string _value = cbp._Value;
                artikelPOCO.Cat_id = short.Parse(_key);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            decimal prijs;
            decimal.TryParse(txtVerkoopprijsT.Text, out prijs);
            artikelPOCO.Verkoopprijs = prijs;

            short stock;
            short.TryParse(txtInstockT.Text, out stock);
            artikelPOCO.Instock = stock;

            try
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<ArtikelPOCO>(artikelsUri, artikelPOCO);
                response.EnsureSuccessStatusCode();     // Throw on error code.
                MessageBox.Show("Artikel succesvol in database weggeschreven", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                artikelsListView.ItemsSource = await GetAllArtikels();      //      toon de aangepaste artikellijst
                if (response.IsSuccessStatusCode)
                {
                    artikelPOCO = await response.Content.ReadAsAsync<ArtikelPOCO>();    // om Artikel_id te verkrijgen
                }
                try
                {
                    string uri = "CopyAfbeelding/" + tbGekozenAfbeeldingT.Text + "/" + artikelPOCO.Artikel_id;
                    response = await httpClient.GetAsync(uri);
                    response.EnsureSuccessStatusCode();     // Throw on error code.
                    MessageBox.Show("Afbeelding goed weggeschreven");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                txtArtikelT.Text = String.Empty;
                txtOmschrijvingT.Text = String.Empty;
                txtVerkoopprijsT.Text = String.Empty;
                txtInstockT.Text = String.Empty;
                cbxCat_id_T.Text = "--Select a Categorie--";
                tbGekozenAfbeeldingT.Text = "kies een afbeelding";
                Image_T.Source = null;

                stkVoegToe.Visibility = Visibility.Hidden;
            }
        }

        private async Task<IEnumerable> GetAllAfbeeldingen()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("/LijstAfbeeldingen/");
                response.EnsureSuccessStatusCode(); // Throw on error code.
                return await response.Content.ReadAsAsync<List<FilesInfo>>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return Enumerable.Empty<String>();
        }

        private async Task<IEnumerable> GetAllArtikels()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(artikelsUri);
                response.EnsureSuccessStatusCode(); // Throw on error code.
                artikels = await response.Content.ReadAsAsync<IEnumerable<ArtikelPOCO>>();
                return artikels;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return Enumerable.Empty<ArtikelPOCO>();
        }

        private async Task<BitmapImage> GetImage(string filename, string pad)
        {                                           //  http://stackoverflow.com/questions/5346727/convert-memory-stream-to-bitmapimage
            byte[] bytes = { };
            try
            {
                bytes = await httpClient.GetByteArrayAsync(imageUri + filename + "/gif/" + pad);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (bytes == null || bytes.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(bytes))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
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

        public class ComboBoxPairs
        {
            public string _Key { get; set; }
            public string _Value { get; set; }

            public ComboBoxPairs(string _key, string _value)
            {
                _Key = _key;
                _Value = _value;
            }
        }

        private async void cbxCat_id_A_Loaded(object sender, RoutedEventArgs e)
        {
            List<ComboBoxPairs> cbp = new List<ComboBoxPairs>();
            var categorieen = await GetAlleCategorieen();

            foreach (CategoriePOCO item in categorieen)
            {
                cbp.Add(new ComboBoxPairs(item.Cat_id.ToString(), item.Categorie1));
            }

            cbxCat_id_A.DisplayMemberPath = "_Value";
            cbxCat_id_A.SelectedValuePath = "_Key";
            cbxCat_id_A.ItemsSource = cbp;
        }

        private async void btnArtikelDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("DeleteAsync :" + artikelsUri + "?id=" + txtArtikel_id_A.Text);
            try
            {
                var response = await httpClient.DeleteAsync(artikelsUri + "?id=" + txtArtikel_id_A.Text);
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Artikel met succes gewist in database", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                artikelsListView.ItemsSource = await GetAllArtikels();
                stkAanpassen.Visibility = Visibility.Hidden;
                try
                {
                    response = await httpClient.GetAsync("/DeleteAfbeelding/" + txtArtikel_id_A.Text);
                    response.EnsureSuccessStatusCode(); // Throw on error code.
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Waarschijnlijk zijn er bestellingen van dit artikel ! \n\n" + ex.Message);
            }
        }

        private async void cbxCat_id_T_Loaded(object sender, RoutedEventArgs e)
        {
            List<ComboBoxPairs> cbp = new List<ComboBoxPairs>();
            var categorieen = await GetAlleCategorieen();

            foreach (CategoriePOCO item in categorieen)
            {
                cbp.Add(new ComboBoxPairs(item.Cat_id.ToString(), item.Categorie1));
            }

            cbxCat_id_T.DisplayMemberPath = "_Value";
            cbxCat_id_T.SelectedValuePath = "_Key";
            cbxCat_id_T.ItemsSource = cbp;
        }

        private async void kiesAfbeelding_T_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                var filesInfo = (FilesInfo)item;
                try
                {
                    tbGekozenAfbeeldingT.Text = filesInfo.FileName;

                    var names = filesInfo.FileName.Split('.');

                    Image_T.Source = await GetImage(names[0], "Resources");
                    btnArtikelPost.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void kiesAfbeelding_A_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                var filesInfo = (FilesInfo)item;
                try
                {
                    MessageBox.Show("nieuwe keuze : " + filesInfo.FileName);

                    var names = filesInfo.FileName.Split('.');

                    tbGekozenAfbeeldingA.Text = filesInfo.FileName;
                    Image_A_Nieuw.Source = await GetImage(names[0], "Resources");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

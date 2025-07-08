using GorselOdev3.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Globalization;
using static GorselOdev3.Models.Haberlers;
using GorselOdev3;
namespace GorselOdev3
{
    public partial class Haberler : ContentPage
    {
        public ObservableCollection<Haberlers.Kategori> Kategoriler { get; set; } = new ObservableCollection<Haberlers.Kategori>();
        public ObservableCollection<Haber> Haberlers { get; set; } = new ObservableCollection<Haber>();
        private bool isLoading = false;


        public Haberler()
        {
            InitializeComponent();
            foreach (var kategori in Kategori.liste)
            {
                Kategoriler.Add(kategori);
            }
            BindingContext = this;
        }

        private async void OnKategoriClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            string selectedCategoryLink = button.CommandParameter.ToString();

            await LoadHaberler(selectedCategoryLink);
        }

        private async Task LoadHaberler(string rssUrl)
        {
            var kategori = new Haberlers.Kategori() { Link = rssUrl };

            var json = await Servisler.HaberleriGetir(kategori);

            if (json == null)
            {
                await DisplayAlert("Hata", "Haberler y?klenirken bir hata olu?tu.", "Tamam");
                return;
            }

            var haberlerData = Newtonsoft.Json.JsonConvert.DeserializeObject<RssResponse>(json);

            Haberlers.Clear();

            foreach (var item in haberlerData.Items)
            {
                DateTime pubDate = DateTime.Parse(item.PubDate, CultureInfo.InvariantCulture);

                Haberlers.Add(new Haber
                {
                    Baslik = item.Title,
                    ResimUrl = item.Enclosure?.Link,
                    Tarih = pubDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Author = item.Author,
                    Link = item.Link,
                });
            }
        }
        private async void OnHaberClicked(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var haber = (Haber)frame.BindingContext;

            await Navigation.PushAsync(new HaberDetay(haber.Baslik, haber.Link));
        }

    }

    public class Haber
    {
        public string Baslik { get; set; }
        public string ResimUrl { get; set; }
        public string Tarih { get; set; }
        public string Author { get; set; }
        public string Link { get; set; }
    }


    public class RssResponse
    {
        public List<RssItem> Items { get; set; }
    }


    public class RssItem
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string PubDate { get; set; }

        public RssEnclosure Enclosure { get; set; }

        public string Author { get; set; }
    }

    public class RssEnclosure
    {
        public string Link { get; set; }
    }
}
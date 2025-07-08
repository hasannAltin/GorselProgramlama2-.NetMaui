using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using Newtonsoft.Json;

namespace GorselOdev3.Models
{
    public class HaberlerViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Haberlers.Kategori> Kategoriler { get; set; }
        public ObservableCollection<Haberlers.Haber> HaberListesi { get; set; }

        public ICommand KategoriSecCommand { get; }

        public HaberlerViewModel()
        {
            Kategoriler = new ObservableCollection<Haberlers.Kategori>(Haberlers.Kategori.liste);
            HaberListesi = new ObservableCollection<Haberlers.Haber>();

            KategoriSecCommand = new Command<string>(OnKategoriSec);
        }

        private async void OnKategoriSec(string link)
        {
            try
            {
                var json = await Servisler.HaberleriGetir(new Haberlers.Kategori { Link = link });
                var haberlerData = Newtonsoft.Json.JsonConvert.DeserializeObject<RssResponse>(json);

                HaberListesi.Clear();

                foreach (var item in haberlerData.Items)
                {
                    DateTime pubDate;
                    var baslik = item.Title ?? "Başlık Yok";
                    var resimUrl = item.Enclosure?.Link ?? "https://via.placeholder.com/300";
                    var yazar = item.Author ?? "Bilinmeyen Yazar";
                    var tarih = DateTime.TryParse(item.PubDate, out pubDate)
                        ? pubDate.ToString("yyyy-MM-dd HH:mm:ss")
                        : "Tarih Yok";

                    HaberListesi.Add(new Haberlers.Haber
                    {
                        Baslik = baslik,
                        ResimUrl = resimUrl,
                        Tarih = tarih,
                        Author = yazar
                    });
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Hata", $"Haberler yüklenirken bir hata oluştu: {ex.Message}", "Tamam");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class RssResponse
    {
        public List<RssItem> Items { get; set; }
    }


    public class RssItem
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("pubDate")]
        public string PubDate { get; set; }

        [JsonProperty("enclosure")]
        public RssEnclosure Enclosure { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }
    }

    public class RssEnclosure
    {
        public string Link { get; set; }
    }
}
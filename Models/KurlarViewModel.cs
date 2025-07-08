using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

namespace GorselOdev3.Models
{
    public class KurlarViewModel : BaseViewModel
    {

        public ObservableCollection<KurModel> KurlarListesi { get; set; } = new();
        public ICommand YenileCommand { get; }
        
        public KurlarViewModel()
        {
            YenileCommand = new Command(async () => await GetKurlarAsync());
            _ = GetKurlarAsync();
        }

        private async Task GetKurlarAsync()
        {
            try
            {
                IsBusy = true;
                KurlarListesi.Clear();

                var jsondata = await Servisler.GetAltinDovizGuncelKurlar();

                if (!string.IsNullOrEmpty(jsondata))
                {
                    var data = JsonSerializer.Deserialize<Dictionary<string, object>>(jsondata);

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            if (item.Key == "Update_Date") continue;

                            var kurDetayJson = JsonSerializer.Serialize(item.Value);
                            var kurDetay = JsonSerializer.Deserialize<KurDetayi>(kurDetayJson);

                            double.TryParse(
                                kurDetay?.Değişim?.Replace("%", "").Replace(",", "."),
                                out double farkValue);

                            KurlarListesi.Add(new KurModel
                            {
                                Tur = item.Key,
                                Alis = kurDetay?.Alış,
                                Satis = kurDetay?.Satış,
                                Fark = kurDetay?.Değişim,
                                YonIkon = farkValue < 0 ? "down_arrow.png" : "up_arrow.png"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Hata", $"Veriler al�n�rken bir hata olu�tu: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }

    public class KurModel
    {
        public string Tur { get; set; }
        public string Alis { get; set; }
        public string Satis { get; set; }
        public string Fark { get; set; }
        public string YonIkon { get; set; }
        public string FarkRenk
        {
            get
            {
                if (double.TryParse(Fark?.Replace("%", "").Replace(",", "."), out double farkValue))
                {
                    return farkValue < 0 ? "Red" : "Green";
                }
                return "Black";
            }
        }

    }
}
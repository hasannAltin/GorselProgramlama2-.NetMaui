using GorselOdev3.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GorselOdev3;

public partial class HavaDurumu : ContentPage
{
    public ObservableCollection<SehirHavaDurumu> Sehirler { get; set; }
    public ICommand RemoveCityCommand { get; }

    public HavaDurumu()
    {
        InitializeComponent();

        Sehirler = new ObservableCollection<SehirHavaDurumu>();

        RemoveCityCommand = new Command<SehirHavaDurumu>(RemoveCity);

        BindingContext = this;
    }

    private async void OnAddCityClicked(object sender, EventArgs e)
    {
        string sehir = await DisplayPromptAsync("Þehir:", "Þehir ismi", "OK", "Cancel");

        if (!string.IsNullOrWhiteSpace(sehir))
        {
            sehir = sehir.ToUpper(System.Globalization.CultureInfo.CurrentCulture)
                         .Replace('Ç', 'C')
                         .Replace('Ð', 'G')
                         .Replace('Ý', 'I')
                         .Replace('Ö', 'O')
                         .Replace('Ü', 'U')
                         .Replace('Þ', 'S');

            Sehirler.Add(new SehirHavaDurumu { Name = sehir });
        }
    }

    private void OnRefreshClicked(object sender, EventArgs e)
    {
        DisplayAlert("Yenile", "Hava durumu verileri yenilendi.", "OK");
    }

    private void RemoveCity(SehirHavaDurumu city)
    {
        if (city != null && Sehirler.Contains(city))
        {
            DisplayAlert("Kaldýrýlýyor", $"þehir: {city.Name}", "Tamam");
            Sehirler.Remove(city);
        }
    }

}
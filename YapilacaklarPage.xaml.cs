using System.Collections.ObjectModel;
using GorselOdev3.Models;
using GorselOdev3.Services;
using System.Text;
using System.ComponentModel;

namespace GorselOdev3;

public partial class YapilacaklarPage : ContentPage
{
    private FirebaseService _firebaseService;
    public ObservableCollection<Yapilacak> Yapilacaklar { get; set; } = new ObservableCollection<Yapilacak>();
    public class YapilacakViewModel : INotifyPropertyChanged
    {
        private string _baslik;
        private string _aciklama;

        public string Baslik
        {
            get => _baslik;
            set
            {
                if (_baslik != value)
                {
                    _baslik = value;
                    OnPropertyChanged(nameof(Baslik));
                }
            }
        }

        public string Aciklama
        {
            get => _aciklama;
            set
            {
                if (_aciklama != value)
                {
                    _aciklama = value;
                    OnPropertyChanged(nameof(Aciklama));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public YapilacaklarPage()
    {
        InitializeComponent();
        _firebaseService = new FirebaseService();
        BindingContext = this;
        LoadYapilacaklar();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadYapilacaklar();
    }

    private async void LoadYapilacaklar()
    {
        try
        {
            var yapilacaklar = await _firebaseService.GetYapilacaklarAsync();
            Console.WriteLine($"Yapilacaklar Loaded: {yapilacaklar.Count} items.");

            Yapilacaklar.Clear();
            foreach (var item in yapilacaklar)
            {
                Yapilacaklar.Add(item);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        LoadYapilacaklar();
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        var viewModel = new YapilacakViewModel();
        AddYapilacakLayout.BindingContext = viewModel;

        AddYapilacakLayout.IsVisible = true;
    }
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var viewModel = AddYapilacakLayout.BindingContext as YapilacakViewModel;
        string baslik = viewModel?.Baslik;
        string aciklama = viewModel?.Aciklama;

        if (string.IsNullOrEmpty(baslik)) return;

        var yapilacak = new Yapilacak { Baslik = baslik, Aciklama = aciklama ?? "Açýklama eklenmedi" };
        await _firebaseService.AddYapilacakAsync(yapilacak);

        AddYapilacakLayout.IsVisible = false;
        LoadYapilacaklar();
    }

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var yapilacak = button?.CommandParameter as Yapilacak;
        if (yapilacak != null)
        {
            await _firebaseService.DeleteYapilacakAsync(yapilacak.Id);
            Yapilacaklar.Remove(yapilacak);
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var yapilacak = button?.CommandParameter as Yapilacak;
        if (yapilacak != null)
        {
            string yeniBaslik = await DisplayPromptAsync("Düzenle", "Yeni baþlýk:", initialValue: yapilacak.Baslik);
            if (string.IsNullOrEmpty(yeniBaslik)) return;

            yapilacak.Baslik = yeniBaslik;
            await _firebaseService.UpdateYapilacakAsync(yapilacak);
            LoadYapilacaklar();
        }
    }

    private async void OnTamamlandiCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = sender as CheckBox;
        var yapilacak = checkbox?.BindingContext as Yapilacak;
        if (yapilacak != null)
        {
            yapilacak.Tamamlandi = checkbox.IsChecked;
            await _firebaseService.UpdateYapilacakAsync(yapilacak);
        }
    }
}
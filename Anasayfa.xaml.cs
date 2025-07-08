using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using GorselOdev3;

namespace GorselOdev3;

public partial class Anasayfa : ContentPage
{
    private const string FirebaseWebApiKey = "AIzaSyCb9RURTzB6FRo2KqDW2Hxy6Om3KyGnLus\r\n";

    public Anasayfa()
    {
        InitializeComponent();

    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Hata", "Lütfen geçerli bir e-posta ve þifre girin.", "Tamam");
            return;
        }

        try
        {
            var response = await SignInUser(email, password);
            Application.Current.MainPage = new AppShell();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Giriþ baþarýsýz: {ex.Message}", "Tamam");
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new KayitOl());
    }

    private static async Task<string> SignUpUser(string email, string password)
    {
        var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={FirebaseWebApiKey}";
        var data = new
        {
            email = email,
            password = password,
            returnSecureToken = true
        };

        return await HttpPost(url, data);
    }

    private static async Task<string> SignInUser(string email, string password)
    {
        var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={FirebaseWebApiKey}";
        var data = new
        {
            email = email,
            password = password,
            returnSecureToken = true
        };

        return await HttpPost(url, data);
    }

    private static async Task<string> HttpPost(string url, object data)
    {
        using (var client = new HttpClient())
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, jsonContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}


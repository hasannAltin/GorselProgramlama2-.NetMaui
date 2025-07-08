using Microsoft.Maui.Controls;

namespace GorselOdev3;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new Anasayfa());

    }



}
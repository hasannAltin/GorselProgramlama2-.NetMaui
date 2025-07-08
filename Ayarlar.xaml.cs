
using Syncfusion.Maui.Themes;

namespace GorselOdev3;

public partial class Ayarlar : ContentPage
{
    public Ayarlar()
    {
        InitializeComponent();
    }



    private void OnThemeToggled(object sender, ToggledEventArgs e)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries != null)
        {
            var theme = mergedDictionaries.OfType<SyncfusionThemeResourceDictionary>().FirstOrDefault();
            if (theme != null)
            {
                if (e.Value)
                {
                    if (theme.VisualTheme is SfVisuals.MaterialLight)
                    {
                        theme.VisualTheme = SfVisuals.MaterialDark;
                        Application.Current.UserAppTheme = AppTheme.Dark;
                        themeLabel.Text = "Karanlýk Mod";
                    }
                }
                else
                {
                    if (theme.VisualTheme is SfVisuals.MaterialDark)
                    {
                        theme.VisualTheme = SfVisuals.MaterialLight;
                        Application.Current.UserAppTheme = AppTheme.Light;
                        themeLabel.Text = "Aydýnlýk Mod";
                    }
                }
            }
        }
    }

}

using Microsoft.Maui.Controls;
using System;

namespace GorselOdev3
{
    public partial class HaberDetay : ContentPage
    {
        private Haber haber;

        public HaberDetay(string title, string url)
        {
            InitializeComponent();

            var webView = new WebView
            {
                Source = new UrlWebViewSource
                {
                    Url = url
                }
            };

            Content = webView;

            haber = new Haber
            {
                Title = title,
                Link = url
            };
        }

        private async void ShareClicked(object sender, EventArgs e)
        {
            if (haber != null)
            {
                await ShareUri(haber.Link, Share.Default);
            }
        }

        public async Task ShareUri(string uri, IShare share)
        {
            await share.RequestAsync(new ShareTextRequest
            {
                Uri = uri,
                Title = haber.Title
            });
        }

        public class Haber
        {
            public string Title { get; set; }
            public string Link { get; set; }
        }
    }
}
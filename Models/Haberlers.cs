using System;
using System.Collections.Generic;

namespace GorselOdev3.Models
{
    public class Haberlers
    {
        public static List<Haber> HaberListesi { get; set; } = new List<Haber>();

        public class Kategori
        {
            public string Baslik { get; set; }
            public string Link { get; set; }

            public static List<Kategori> liste = new List<Kategori>()
            {
                new Kategori() { Baslik = "Son Dakika", Link = "https://www.trthaber.com/sondakika_articles.rss"},
                new Kategori() { Baslik = "Gündem", Link = "https://www.trthaber.com/gundem_articles.rss"},
                new Kategori() { Baslik = "Ekonomi", Link = "https://www.trthaber.com/ekonomi_articles.rss"},
                new Kategori() { Baslik = "Bilim Teknoloji", Link = "https://www.trthaber.com/bilim_teknoloji_articles.rss"},
                new Kategori() { Baslik = "Eğitim", Link = "https://www.trthaber.com/egitim_articles.rss"},
            };
        }
        public class Haber
        {
            public string Baslik { get; set; }
            public string ResimUrl { get; set; }
            public string Tarih { get; set; }
            public string Author { get; set; }
        }
    }
}
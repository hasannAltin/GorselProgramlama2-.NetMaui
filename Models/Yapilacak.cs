using System;

namespace GorselOdev3.Models
{
    public class Yapilacak
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public bool Tamamlandi { get; set; }
    }
}
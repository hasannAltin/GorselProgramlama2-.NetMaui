using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorselOdev3.Models
{
    public class DovizVerisi
    {
        public string Update_Date { get; set; }
        public Dictionary<string, KurDetayi> KurBilgileri { get; set; }
    }

    public class KurDetayi
    {
        public string Alış { get; set; }
        public string Satış { get; set; }
        public string Tür { get; set; }
        public string Değişim { get; set; }
    }
}
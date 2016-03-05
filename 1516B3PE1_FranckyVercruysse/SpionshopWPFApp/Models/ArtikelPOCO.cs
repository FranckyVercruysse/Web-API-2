using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpionshopWPFApp.Models
{
    public class ArtikelPOCO
    {
        public short Artikel_id { get; set; }
        public short Cat_id { get; set; }
        public string Artikel1 { get; set; }
        public string Omschrijving { get; set; }
        public Nullable<decimal> Verkoopprijs { get; set; }
        public Nullable<short> Instock { get; set; }
    }
}

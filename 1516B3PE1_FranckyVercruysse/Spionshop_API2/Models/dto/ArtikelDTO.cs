using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Spionshop_API2.Models.dto
{
    public class ArtikelDTO
    {
        [Key]
        public short Artikel_id { get; set; }
        public short Cat_id { get; set; }
        public string Artikel1 { get; set; }
        public string Omschrijving { get; set; }
        public Nullable<decimal> Verkoopprijs { get; set; }
        public Nullable<short> Instock { get; set; }
    }
}
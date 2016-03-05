using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpionshopASPNETWebApp.Models.dto
{
    public class BestellingDetailDTO
    {
        public int BD_id { get; set; }
        public int B_id { get; set; }
        public short Artikel_id { get; set; }
        public string Artikel1 { get; set; }
        public short Aantal { get; set; }
    }
}
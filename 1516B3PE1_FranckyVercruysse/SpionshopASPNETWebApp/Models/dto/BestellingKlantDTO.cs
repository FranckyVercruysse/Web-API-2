using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpionshopASPNETWebApp.Models.dto
{
    public class BestellingKlantDTO
    {
        public int B_id { get; set; }
        public DateTime Datum { get; set; }
        public short Klant_id { get; set; }
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public ICollection<BestellingDetailDTO> Bestellingen { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpionshopASPNETWebApp.Models.dto
{
    public class KlantDTO
    {
        public short Klant_id { get; set; }
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public string Woonplaats { get; set; }
        public Nullable<System.DateTime> Geboortedatum { get; set; }
        public string Gebruikersnaam { get; set; }
        public string Pwd { get; set; }
    }
}
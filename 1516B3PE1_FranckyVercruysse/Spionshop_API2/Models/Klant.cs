using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spionshop_API2.Models
{
    [Table("Klant")]
    public class Klant
    {
        public Klant()
        {
            this.Bestellings = new HashSet<Bestelling>();
        }

        [Key]
        public short Klant_id { get; set; }
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public string Woonplaats { get; set; }
        public Nullable<System.DateTime> Geboortedatum { get; set; }
        public string Gebruikersnaam { get; set; }
        public string Pwd { get; set; }

        public virtual ICollection<Bestelling> Bestellings { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Spionshop_API2.Models
{
    [Table("Bestelling")]
    public class Bestelling
    {
        public Bestelling()
        {
            this.BestellingDetails = new HashSet<BestellingDetail>();
        }

        [Key]
        public int B_id { get; set; }
        public short Klant_id { get; set; }
        public System.DateTime Datum { get; set; }

        public virtual Klant Klant { get; set; }
        public virtual ICollection<BestellingDetail> BestellingDetails { get; set; }
    }
}
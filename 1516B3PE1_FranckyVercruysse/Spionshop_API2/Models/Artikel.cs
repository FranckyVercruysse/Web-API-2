using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Spionshop_API2.Models
{
    [Table("Artikel")]
    public class Artikel
    {

        public Artikel()
        {
            this.BestellingDetails = new HashSet<BestellingDetail>();
        }
        [Key]
        public short Artikel_id { get; set; }
        public short Cat_id { get; set; }
        public string Artikel1 { get; set; }
        public string Omschrijving { get; set; }
        public Nullable<decimal> Verkoopprijs { get; set; }
        public Nullable<short> Instock { get; set; }

        public virtual Categorie Categorie { get; set; }
        public virtual ICollection<BestellingDetail> BestellingDetails { get; set; }
    }
}
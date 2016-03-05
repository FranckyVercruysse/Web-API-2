using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Spionshop_API2.Models
{
    [Table("Categorie")]
    public class Categorie
    {
        public Categorie()
        {
            this.Artikels = new HashSet<Artikel>();
        }

        [Key]
        public short Cat_id { get; set; }
        public string Categorie1 { get; set; }

        public virtual ICollection<Artikel> Artikels { get; set; }
    }
}
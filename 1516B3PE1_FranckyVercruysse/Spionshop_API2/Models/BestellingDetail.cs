using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Spionshop_API2.Models
{
    [Table("BestellingDetail")]
    public class BestellingDetail
    {
        [Key]
        public int BD_id { get; set; }
        public int B_id { get; set; }
        public short Artikel_id { get; set; }
        public short Aantal { get; set; }

        public virtual Artikel Artikel { get; set; }
        public virtual Bestelling Bestelling { get; set; }
    }
}
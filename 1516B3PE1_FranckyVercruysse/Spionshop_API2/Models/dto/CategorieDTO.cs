using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Spionshop_API2.Models.dto
{
    public class CategorieDTO
    {
        [Key]
        public short Cat_id { get; set; }
        public string Categorie1 { get; set; }
    }
}
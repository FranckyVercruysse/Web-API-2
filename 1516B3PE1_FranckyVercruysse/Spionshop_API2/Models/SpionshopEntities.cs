using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Spionshop_API2.Models
{
    public class SpionshopEntities : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public SpionshopEntities() : base("name=SpionshopEntities")
        {
        }

        public System.Data.Entity.DbSet<Spionshop_API2.Models.Categorie> Categories { get; set; }

        public System.Data.Entity.DbSet<Spionshop_API2.Models.Artikel> Artikels { get; set; }

        public System.Data.Entity.DbSet<Spionshop_API2.Models.Bestelling> Bestellings { get; set; }

        public System.Data.Entity.DbSet<Spionshop_API2.Models.Klant> Klants { get; set; }

        public System.Data.Entity.DbSet<Spionshop_API2.Models.BestellingDetail> BestellingDetails { get; set; }
    
    }
}

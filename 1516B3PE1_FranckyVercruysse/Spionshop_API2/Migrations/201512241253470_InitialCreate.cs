namespace Spionshop_API2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorie",
                c => new
                    {
                        Cat_id = c.Short(nullable: false, identity: true),
                        Categorie1 = c.String(),
                    })
                .PrimaryKey(t => t.Cat_id);
            
            CreateTable(
                "dbo.Artikel",
                c => new
                    {
                        Artikel_id = c.Short(nullable: false, identity: true),
                        Cat_id = c.Short(nullable: false),
                        Artikel1 = c.String(),
                        Omschrijving = c.String(),
                        Verkoopprijs = c.Decimal(precision: 18, scale: 2),
                        Instock = c.Short(),
                    })
                .PrimaryKey(t => t.Artikel_id)
                .ForeignKey("dbo.Categorie", t => t.Cat_id, cascadeDelete: true)
                .Index(t => t.Cat_id);
            
            CreateTable(
                "dbo.BestellingDetail",
                c => new
                    {
                        BD_id = c.Int(nullable: false, identity: true),
                        B_id = c.Int(nullable: false),
                        Artikel_id = c.Short(nullable: false),
                        Aantal = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.BD_id)
                .ForeignKey("dbo.Artikel", t => t.Artikel_id, cascadeDelete: true)
                .ForeignKey("dbo.Bestelling", t => t.B_id, cascadeDelete: true)
                .Index(t => t.B_id)
                .Index(t => t.Artikel_id);
            
            CreateTable(
                "dbo.Bestelling",
                c => new
                    {
                        B_id = c.Int(nullable: false, identity: true),
                        Klant_id = c.Short(nullable: false),
                        Datum = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.B_id)
                .ForeignKey("dbo.Klant", t => t.Klant_id, cascadeDelete: true)
                .Index(t => t.Klant_id);
            
            CreateTable(
                "dbo.Klant",
                c => new
                    {
                        Klant_id = c.Short(nullable: false, identity: true),
                        Naam = c.String(),
                        Voornaam = c.String(),
                        Woonplaats = c.String(),
                        Geboortedatum = c.DateTime(),
                        Gebruikersnaam = c.String(),
                        Pwd = c.String(),
                    })
                .PrimaryKey(t => t.Klant_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Artikel", "Cat_id", "dbo.Categorie");
            DropForeignKey("dbo.Bestelling", "Klant_id", "dbo.Klant");
            DropForeignKey("dbo.BestellingDetail", "B_id", "dbo.Bestelling");
            DropForeignKey("dbo.BestellingDetail", "Artikel_id", "dbo.Artikel");
            DropIndex("dbo.Bestelling", new[] { "Klant_id" });
            DropIndex("dbo.BestellingDetail", new[] { "Artikel_id" });
            DropIndex("dbo.BestellingDetail", new[] { "B_id" });
            DropIndex("dbo.Artikel", new[] { "Cat_id" });
            DropTable("dbo.Klant");
            DropTable("dbo.Bestelling");
            DropTable("dbo.BestellingDetail");
            DropTable("dbo.Artikel");
            DropTable("dbo.Categorie");
        }
    }
}

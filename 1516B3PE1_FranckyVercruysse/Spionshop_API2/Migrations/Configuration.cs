namespace Spionshop_API2.Migrations
{
    using Spionshop_API2.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Spionshop_API2.Models.SpionshopEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Spionshop_API2.Models.SpionshopEntities";
        }

        protected override void Seed(Spionshop_API2.Models.SpionshopEntities context)
        {
            context.Klants.AddOrUpdate(
    k => k.Gebruikersnaam,
    new Klant { Klant_id = 1, Naam = "Vercruysse", Voornaam = "Francky", Woonplaats = "Roeselare", Geboortedatum = new DateTime(1964, 6, 21), Gebruikersnaam = "Francky", Pwd = "XXXXXXXX" },
    new Klant { Klant_id = 2, Naam = "VanCoillie", Voornaam = "Fanny", Woonplaats = "Brugge", Geboortedatum = new DateTime(1954, 8, 10), Gebruikersnaam = "Fanny", Pwd = "XXXXXXXX" },
    new Klant { Klant_id = 3, Naam = "Dendooven", Voornaam = "Hannes", Woonplaats = "Brugge", Geboortedatum = new DateTime(1988, 5, 3), Gebruikersnaam = "Hannes", Pwd = "XXXXXXXX" }
    );
            context.Categories.AddOrUpdate(
                c => c.Categorie1,
                new Categorie { Cat_id = 1, Categorie1 = "communicatie" },
                new Categorie { Cat_id = 2, Categorie1 = "afleiding" },
                new Categorie { Cat_id = 3, Categorie1 = "algemeen" },
                new Categorie { Cat_id = 4, Categorie1= "Transport"}
                );
            context.Artikels.AddOrUpdate(
                a => a.Artikel1,
                new Artikel { Artikel_id = 1, Cat_id = 1, Artikel1 = "Communicatie apparaat", Omschrijving = "Klein communicatieappparaat in de vorm van een potlood, spreek in de punt en luister aan het andere uiteinde.", Verkoopprijs = 49.99M, Instock = 150 },
                new Artikel { Artikel_id = 2, Cat_id = 1, Artikel1 = "Valse snor vertaler", Omschrijving = "Deze valse snor zorgt ervoor dat door gebruik te maken van geavanceerde voice-technologie uw spraak vertaald wordt in een door u ingestelde taal.", Verkoopprijs = 599.99M, Instock = 150 },
                new Artikel { Artikel_id = 3, Cat_id = 1, Artikel1 = "Vertaler oorringen", Omschrijving = "Deze oorringen vertalen gesproken woorden in een door u verlangde taal, zodanig dat u vreemde talen kan begrijpen.", Verkoopprijs = 459.9900M, Instock = 150 },
                new Artikel { Artikel_id = 4, Cat_id = 1, Artikel1 = "Sigaar laser", Omschrijving = "Laser pointer in de vorm van een sigaar, ontploft niet !", Verkoopprijs = 29.9900M, Instock = 150 },
                new Artikel { Artikel_id = 5, Cat_id = 1, Artikel1 = "Overtuigingspotlood", Omschrijving = "Richt dit potlood op uw opponent in een discussie en overtuig hem van uw gelijk.", Verkoopprijs = 1.9900M, Instock = 150 },
                new Artikel { Artikel_id = 6, Cat_id = 2, Artikel1 = "Correctie vloeistof", Omschrijving = "Een druppel van deze vloestof op de neus van het slachtoffer en hij vertelt je geen leugens meer.", Verkoopprijs = 3.9900M, Instock = 150 },
                new Artikel { Artikel_id = 7, Cat_id = 2, Artikel1 = "Munteenheden portefeuille", Omschrijving = "Plaats stenen en water in deze portefeuille en sluit ze. Wanneer ze opnieuw wordt geopend heb je geld in de lokale munteenheid.", Verkoopprijs = 12.5000M, Instock = 150 },
                new Artikel { Artikel_id = 8, Cat_id = 2, Artikel1 = "Identiteitsverwarringsapparaat", Omschrijving = "Wanneer een vijandig persoon u nadert legt u dit toestelletje aan, en hij herkent u niet meer en loopt  u zonder meer voorbij.", Verkoopprijs = 17.8000M, Instock = 150 },
                new Artikel { Artikel_id = 9, Cat_id = 3, Artikel1 = "Contactlenzen", Omschrijving = "Deze lenzen vervangen de traditionele verrekijker en geven perfect zicht tot 15km.", Verkoopprijs = 13.7500M, Instock = 150 },
                new Artikel { Artikel_id = 10, Cat_id = 3, Artikel1 = "Snelpleisters", Omschrijving = "Doe een snelpleister op een wonde en na 10-30 min is de wonde volledig genezen", Verkoopprijs = 3.9900M, Instock = 150 }
                );
            context.Bestellings.AddOrUpdate(
                b => b.B_id,
                new Bestelling { B_id = 1, Klant_id = 1, Datum = new DateTime(2015, 11, 25) },
                new Bestelling { B_id = 2, Klant_id = 2, Datum = new DateTime(2015, 11, 25) },
                new Bestelling { B_id = 3, Klant_id = 2, Datum = new DateTime(2015, 11, 25) }
                );
            context.BestellingDetails.AddOrUpdate(
                bd => bd.BD_id,
                new BestellingDetail { BD_id = 1, B_id = 1, Artikel_id = 1, Aantal = 5 },
                new BestellingDetail { BD_id = 2, B_id = 1, Artikel_id = 10, Aantal = 15 },
                new BestellingDetail { BD_id = 3, B_id = 1, Artikel_id = 5, Aantal = 55 },
                new BestellingDetail { BD_id = 4, B_id = 2, Artikel_id = 7, Aantal = 53 },
                new BestellingDetail { BD_id = 5, B_id = 2, Artikel_id = 4, Aantal = 6 },
                new BestellingDetail { BD_id = 6, B_id = 3, Artikel_id = 9, Aantal = 16 },
                new BestellingDetail { BD_id = 7, B_id = 3, Artikel_id = 2, Aantal = 335 },
                new BestellingDetail { BD_id = 8, B_id = 3, Artikel_id = 3, Aantal = 155 }
                );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Spionshop_API2.Models;
using Spionshop_API2.Models.dto;
using System.Web.Http.Cors;

namespace Spionshop_API2.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/Bestelling")]
    public class BestellingController : ApiController
    {
        private SpionshopEntities db = new SpionshopEntities();

        [Route("BestellingKlant/{id}")]
        public IQueryable<BestellingKlantDTO> GetBestellingKlant(short id)
        {
            var bestellingen = from best in db.Bestellings
                               .Where(best=>best.Klant_id==id)
                               select new BestellingKlantDTO()
                               {
                                   Klant_id = best.Klant_id,
                                   Naam = best.Klant.Naam,
                                   Voornaam = best.Klant.Voornaam,
                                   B_id = best.B_id,
                                   Datum = best.Datum,
                                   Bestellingen = (from bd in db.BestellingDetails
                                                            .Where(bd => bd.B_id == best.B_id)
                                                   select new BestellingDetailDTO()
                                                   {
                                                       B_id = bd.B_id,
                                                       BD_id = bd.BD_id,
                                                       Aantal = bd.Aantal,
                                                       Artikel1 = db.Artikels.Where(a => a.Artikel_id == bd.Artikel_id).Select(a => a.Artikel1).FirstOrDefault(),
                                                       Artikel_id = bd.Artikel_id
                                                   }
                                                   ).ToList()
                               };

            return bestellingen;
        }


        [Route("")]
        public IQueryable<BestellingKlantDTO> GetBestellingen()
        {
            var bestellingen = from best in db.Bestellings
                               select new BestellingKlantDTO()
                               {
                                   Klant_id = best.Klant_id,
                                   Naam = best.Klant.Naam,
                                   Voornaam = best.Klant.Voornaam,
                                   B_id = best.B_id,
                                   Datum = best.Datum,
                                   Bestellingen = (from bd in db.BestellingDetails
                                                            .Where(bd => bd.B_id == best.B_id)
                                                   select new BestellingDetailDTO()
                                                   {
                                                       B_id = bd.B_id,
                                                       BD_id = bd.BD_id,
                                                       Aantal = bd.Aantal,
                                                       Artikel1 = db.Artikels.Where(a => a.Artikel_id == bd.Artikel_id).Select(a => a.Artikel1).FirstOrDefault(),
                                                       Artikel_id = bd.Artikel_id
                                                   }
                                                   ).ToList()
                               };

            return bestellingen;
        }

        // POST: api/Bestelling
        [Route("")]
        [ResponseType(typeof(BestellingDTO))]
        public async Task<IHttpActionResult> PostBestelling(BestellingDTO bestellingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bestelling = new Bestelling();
            bestelling.Datum = DateTime.Today;
            bestelling.Klant_id = bestellingDto.Klant_id;

            db.Entry(bestelling).State = System.Data.Entity.EntityState.Added;
            db.Bestellings.Add(bestelling);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            var B_id = bestelling.B_id;

            foreach (var item in bestellingDto.Bestellingen)
            {
                var bestellingDetail = new BestellingDetail();

                bestellingDetail.B_id = B_id;
                bestellingDetail.Artikel_id = item.Artikel_id;
                bestellingDetail.Aantal = item.Aantal;
                db.BestellingDetails.Add(bestellingDetail);
                await db.SaveChangesAsync();
            }
            await db.SaveChangesAsync();

            return Ok(bestellingDto);
        }

        // DELETE: api/Bestelling/5
        [Route("")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteBestelling(short id)
        {
            BestellingDetail bestellingdetail = await db.BestellingDetails.FindAsync(id);

            if (bestellingdetail == null)
            {
                return NotFound();
            }

            var b_id = bestellingdetail.B_id;

            db.BestellingDetails.Remove(bestellingdetail);
            await db.SaveChangesAsync();

            if (!BestellingDetailExists(b_id))      // als er geen detail meer is verwijder dan de bestelling
            {
                var bestelling = await db.Bestellings.FindAsync(b_id);
                db.Bestellings.Remove(bestelling);
                await db.SaveChangesAsync();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool BestellingExists(int id)
        {
            return db.Bestellings.Count(e => e.B_id == id) > 0;
        }

        private bool KlantExists(int id)
        {
            return db.Klants.Count(e => e.Klant_id == id) > 0;
        }

        private bool BestellingDetailExists(int b_id)
        {
            return db.BestellingDetails.Count(e => e.B_id == b_id) > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
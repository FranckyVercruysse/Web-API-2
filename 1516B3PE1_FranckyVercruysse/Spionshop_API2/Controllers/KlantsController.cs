using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Spionshop_API2.Models;
using Spionshop_API2.Models.dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Data.Entity.Validation;
using System.Web.Http.Cors;

namespace Spionshop_API2.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/Klants")]
    public class KlantsController : ApiController
    {
        private SpionshopEntities db = new SpionshopEntities();

        //  api/klants/lijstklanten
        [Route("LijstKlanten")]
        public IEnumerable<KlantDTO> GetKlant()
        {
            Mapper.CreateMap<Klant, KlantDTO>();
            var klanten = db.Klants.ProjectTo<KlantDTO>();

            return klanten;
        }

        [Route("")]
        [ResponseType(typeof(KlantDTO))]
        public async Task<IHttpActionResult> GetKlant([FromUri]KlantDTO klantDTO)    // Klant_id wordt ingevuld        
        {
            var klant = (await db.Klants
                .Where(x => x.Gebruikersnaam == klantDTO.Gebruikersnaam
                    && x.Pwd == klantDTO.Pwd)
                .FirstOrDefaultAsync());


            if (klant == null)
                return NotFound();


            Mapper.CreateMap<Klant, KlantDTO>();

            var klantDto = Mapper.Map<KlantDTO>(klant);

            return Ok(klantDto);
        }

        // POST: api/Klants
        [Route("")]
        [ResponseType(typeof(KlantDTO))]
        public async Task<IHttpActionResult> PostKlant(KlantDTO klantDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.CreateMap<KlantDTO, Klant>();

            var klant = Mapper.Map<Klant>(klantDTO);

            db.Klants.Add(klant);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbEntityValidationException)
            {

            }
            catch (Exception)
            {
            }
            return Ok(klant);
        }

        // DELETE: api/Klants/5
        [Route("{id}")]
        [ResponseType(typeof(Klant))]
        public async Task<IHttpActionResult> DeleteKlant(short id)
        {
            Klant klant = await db.Klants.FindAsync(id);
            if (klant == null)
            {
                return NotFound();
            }

            db.Klants.Remove(klant);
            await db.SaveChangesAsync();

            return Ok(klant);
        }
        
        private bool KlantExists(short id)
        {
            return db.Klants.Count(e => e.Klant_id == id) > 0;
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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Spionshop_API2.Models;
using Spionshop_API2.Models.dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Web.Http.Cors;

namespace Spionshop_API2.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/Artikels")]
    public class ArtikelsController : ApiController
    {
        private SpionshopEntities db = new SpionshopEntities();

        [Route("")]
        public IQueryable<ArtikelDTO> GetArtikels()
        {
            Mapper.CreateMap<Artikel, ArtikelDTO>();
            var artikels = db.Artikels.ProjectTo<ArtikelDTO>();
            return artikels;
        }

        // GET: api/Artikel/5
        [Route("{id}")]
        [ResponseType(typeof(ArtikelDTO))]
        public async Task<IHttpActionResult> GetArtikelPerArtikelID(short id)
        {
            Mapper.CreateMap<Artikel, ArtikelDTO>();

            var artikels = await db.Artikels.ToListAsync();
            var artikel = await db.Artikels
                .Where(a => a.Artikel_id == id)
                .ProjectTo<ArtikelDTO>()
                .FirstAsync();

            if (artikel == null)
            {
                return NotFound();
            }

            return Ok(artikel);
        }

        // POST: api/Artikels
        [Route("")]
        [ResponseType(typeof(ArtikelDTO))]
        public async Task<IHttpActionResult> PostArtikel(ArtikelDTO artikelDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.CreateMap<ArtikelDTO, Artikel>();
            var artikel = Mapper.Map<Artikel>(artikelDTO);

            db.Artikels.Add(artikel);
            await db.SaveChangesAsync();

            Mapper.CreateMap<Artikel, ArtikelDTO>();
            artikelDTO = Mapper.Map<ArtikelDTO>(artikel);     // Artikel_id invullen in artikelDTO
            return Ok(artikelDTO);
        }

        // PUT: api/Artikels/5
        [Route("")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutArtikel(short id, ArtikelDTO artikelDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != artikelDTO.Artikel_id)
            {
                return BadRequest();
            }

            Mapper.CreateMap<ArtikelDTO, Artikel>();

            var artikel = Mapper.Map<Artikel>(artikelDTO);

            db.Entry(artikel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtikelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Artikels/5
        [Route("")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteArtikel(short id)
        {
            Artikel artikel = await db.Artikels.FindAsync(id);
            if (artikel == null)
            {
                return NotFound();
            }

            if (!exitBestellingDetails(id))
            {
                db.Artikels.Remove(artikel);
                await db.SaveChangesAsync();
            }
            else
            {       // als er een bestelling van dit artikel bestaat kan het niet gewist worden
                return StatusCode(HttpStatusCode.Forbidden);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool exitBestellingDetails(short id)
        {
            return db.BestellingDetails.Count(e => e.Artikel_id == id) > 0;
        }

        private bool ArtikelExists(short id)
        {
            return db.Artikels.Count(e => e.Artikel_id == id) > 0;
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
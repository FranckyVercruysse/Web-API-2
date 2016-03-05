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
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Web.Http.Cors;

namespace Spionshop_API2.Controllers
{
    //https://msdn.microsoft.com/en-us/magazine/dn532203.aspx


    //[EnableCors(origins: "http://spionshopapi2.azurewebsites.net", headers: "*", methods: "*")]
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/Categorie")]
    public class CategorieController : ApiController
    {
        private SpionshopEntities db = new SpionshopEntities();

        // GET: api/Categorie
        [Route("")]
        public IEnumerable<CategorieDTO> GetCategorie()
        {
            Mapper.CreateMap<Categorie, CategorieDTO>();
            var categories = db.Categories.ProjectTo<CategorieDTO>();

            return categories;
        }

        // GET: api/Categorie/5
        [Route("{id}")]
        public IQueryable<ArtikelDTO> GetArtikelPerCat(short id)
        {
            Mapper.CreateMap<Artikel, ArtikelDTO>();

            var artikels = db.Artikels
                //.Include("Categorie")
                .Where(a => a.Categorie.Cat_id == id)
                .ProjectTo<ArtikelDTO>();

            return artikels;
        }

        // POST: api/Categorie
        [Route("")]
        [ResponseType(typeof(CategorieDTO))]
        public async Task<IHttpActionResult> PostCategorie(CategorieDTO categorieDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.CreateMap<CategorieDTO, Categorie>();

            var categorie = Mapper.Map<Categorie>(categorieDTO);

            db.Categories.Add(categorie);
            await db.SaveChangesAsync();


            Mapper.CreateMap<Categorie, CategorieDTO>();
            categorieDTO = Mapper.Map<CategorieDTO>(categorie);     // Cat_id invullen

            return Ok(categorieDTO);

            //return CreatedAtRoute("DefaultApi", new { id = categorie.Cat_id }, categorie);
        }

        // PUT: api/Categorie/5
        [Route("")]
        [ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutCategorie([FromUri]short id, [FromBody]CategorieDTO categorieDTO)
        public async Task<IHttpActionResult> PutCategorie(short id, CategorieDTO categorieDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categorieDTO.Cat_id)
            {
                return BadRequest();
            }

            Mapper.CreateMap<CategorieDTO, Categorie>();

            var categorie = Mapper.Map<Categorie>(categorieDTO);

            db.Entry(categorie).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategorieExists(id))
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

        // DELETE: api/Categorie/5
        [Route("")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteCategorie(short id)
        {
            Categorie categorie = await db.Categories.FindAsync(id);
            if (categorie == null)
            {
                return NotFound();
            }

            db.Categories.Remove(categorie);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent); ;
        }


        private bool CategorieExists(short id)
        {
            return db.Categories.Count(e => e.Cat_id == id) > 0;
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
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TrackRE.DTOs;
using TrackRE.Hubs;
using TrackRE.Models;

namespace TrackRE.Controllers
{
    [InvalidModelStateFilter]
    [RoutePrefix("api/properties")]
    public class PropertiesApiController : ApiControllerWithHub<TrackREHub>
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //private static int lastId = db.Max(tdi => tdi.ID);

        // Typed lambda expression for Select() method. 
        internal static readonly Expression<Func<PropertyRE, PropertyREDTO>> AsPropertyREDTO =
            x => new PropertyREDTO
            {
                PropertyREId = x.PropertyREId,
                Description = x.Description,
                PropertyTypeSub = x.PropertyTypeSub,
                Location = x.Location,
                //strPrice = x.Price.ToString("0,0", CultureInfo.InvariantCulture),
                Price = x.Price,
                PropertyTypeId = x.PropertyTypeId,
                CommunityId = x.CommunityId,
                OwnerID = x.OwnerID,
                PropertyType = x.PropertyType.Name,
                Community = x.Community.Name,
                Owner = x.Owner.Name            
            };


        // GET api/Property Types
        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/types")]
        public IEnumerable<PropertyType> PropertyTypes()
        {
            // Just a comment
            return db.PropertyTypes.ToArray();
        }

        // GET: api/PropertyREs
        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(PropertyREDTO))]
        public IQueryable<PropertyREDTO> Properties()
        {
            var propList = db.Properties.Include(b => b.PropertyType).Include(b => b.Community).Include(b => b.Owner)
                    .Select(AsPropertyREDTO);
            return propList;
        }        

        // GET api/Properties/5
        [AllowAnonymous]
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(PropertyRE))]
        public async Task<IHttpActionResult> Property(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            PropertyRE property = await db.Properties.FindAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        // GET api/Properties/5
        [AllowAnonymous]
        [HttpGet]
        [Route("{id:int}/DTO")]
        [ResponseType(typeof(PropertyREDTO))]
        public async Task<IHttpActionResult> PropertyDTO(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            PropertyREDTO property = await db.Properties.Include(b => b.Community).Include(b => b.Owner)
                .Where(b => b.PropertyREId == id)
                .Select(AsPropertyREDTO)
                .FirstOrDefaultAsync();

            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        // GET api/Properties/{genre}
        [AllowAnonymous]
        [HttpGet]
        [Route("{genre}")]
        [ResponseType(typeof(PropertyREDTO))]
        public IQueryable<PropertyREDTO> Properties(string genre)
        {
            var allproperties = db.Properties.Include(b => b.PropertyType).Include(b => b.Community).Include(b => b.Owner)
                .Where(b => b.PropertyType.Name.Equals(genre, StringComparison.OrdinalIgnoreCase))
                .Select(AsPropertyREDTO);

            return allproperties;
        }

        // ------------
        // PUT: api/PropertyREs/5
        [HttpPut]
        [Route("{id:int}/edit")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPropertyRE(int id, PropertyRE propertyRE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != propertyRE.PropertyREId)
            {
                return BadRequest();
            }

            db.Entry(propertyRE).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                
                PropertyREDTO propertyREDTO = db.Properties.Include(b => b.Community).Include(b => b.Owner).Include(b => b.PropertyType)
                    .Where(b => b.PropertyREId == id)
                    .Select(AsPropertyREDTO)
                    .FirstOrDefault();

                Hub.Clients.All.updateItem(propertyREDTO);                     // Notify the connected clients
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyREExists(id))
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

        // POST: api/PropertyREs
        [Route("add")]
        [ResponseType(typeof(PropertyRE))]
        public async Task<IHttpActionResult> PostPropertyRE(PropertyRE propertyRE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Properties.Add(propertyRE);
            await db.SaveChangesAsync();

            PropertyREDTO propertyREDTO = db.Properties.Include(b => b.Community).Include(b => b.Owner).Include(b => b.PropertyType)
                .Where(b => b.PropertyREId == propertyRE.PropertyREId)
                .Select(AsPropertyREDTO)
                .FirstOrDefault();

            Hub.Clients.All.updateItem(propertyREDTO);                     // Notify the connected clients

            // Return the new item, inside a 201 response (i.e., HttpStatusCode.Created)
            return CreatedAtRoute("DefaultApi", new { id = propertyRE.PropertyREId }, propertyRE);
        }

        // DELETE: api/PropertyREs/5
        //[HttpPost]
        [Route("{id:int}/delete")]
        [ResponseType(typeof(PropertyRE))]
        public async Task<IHttpActionResult> DeletePropertyRE(int id)
        {
            PropertyRE propertyRE = await db.Properties.FindAsync(id);
            if (propertyRE == null)
            {
                return NotFound();
            }

            db.Properties.Remove(propertyRE);
            await db.SaveChangesAsync();
            Hub.Clients.All.deleteItem(id);                             // Notify the connected clients

            return Ok(propertyRE);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PropertyREExists(int id)
        {
            return db.Properties.Count(e => e.PropertyREId == id) > 0;
        }
    }
}

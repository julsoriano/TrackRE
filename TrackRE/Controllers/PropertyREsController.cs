using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrackRE.DTOs;
using TrackRE.Hubs;
using TrackRE.Models;
using TrackRE.ServicesClient;

namespace TrackRE.Controllers
{
    // [Authorize(Roles = "canEdit")]
    public class PropertyREsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /* Original Code
        // GET: PropertyREs
        public ActionResult Index()
        {
            var properties = db.Properties.Include(p => p.Community).Include(p => p.Owner).Include(p => p.PropertyType);
            return View(properties.ToList());
         */

        // GET: PropertyREs
        public async Task<ActionResult> Index()
        {
            var propertyservice = new PropertyService();
            return View(await propertyservice.GetPropertyREsAsync());
        }

        // 
        // GET: /Property/Browse?proptype=Condo
        public async Task<ActionResult> Browse(string proptype)
        {
            // Retrieve Property Type and its Associated Properties from database 
            var propertyservice = new PropertyService();
            var propertyRE = await propertyservice.GetPropertyREsDTOAsyncByType(proptype);
            return View(propertyRE);
        }

        // 
        // GET: /Property/Details/5 
        public async Task<ActionResult> DetailsView(int id)
        {
            var propertyservice = new PropertyService();
            var propertyRE = await propertyservice.GetPropertyREDTOAsync(id);
            return View(propertyRE);
        }
    
        /* Original Code
        // GET: PropertyREs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyRE propertyRE = db.Properties.Find(id);
            if (propertyRE == null)
            {
                return HttpNotFound();
            }
            return View(propertyRE);
        } */

        // Converted to a call to API
        // GET: /PropertyREs/Details/5 
        public async Task<ActionResult> Details(int id)
        {
            var propertyservice = new PropertyService();
            var propertyRE = await propertyservice.GetPropertyREDTOAsync(id);
            return View(propertyRE);
        }

        // GET: PropertyREs/Create
        public ActionResult Create()
        {
            ViewBag.CommunityId = new SelectList(db.Communities, "CommunityId", "Name");
            ViewBag.OwnerID = new SelectList(db.Owners, "OwnerID", "Name");
            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes, "PropertyTypeId", "Name");
            return View();
        }

        /* Original Code
        // POST: PropertyREs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PropertyREId,PropertyTypeId,CommunityId,OwnerID,Description,PropertyTypeSub,Location,Price")] PropertyRE propertyRE)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Properties.Add(propertyRE);
                    db.SaveChanges();
                    
                    return RedirectToAction("Index");
                }
            }
            catch (DataException )
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            ViewBag.CommunityId = new SelectList(db.Communities, "CommunityId", "Name", propertyRE.CommunityId);
            ViewBag.OwnerID = new SelectList(db.Owners, "OwnerID", "Name", propertyRE.OwnerID);
            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes, "PropertyTypeId", "Name", propertyRE.PropertyTypeId);
            return View(propertyRE);
        } */

        // POST: PropertyREs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PropertyREId,PropertyTypeId,CommunityId,OwnerID,Description,PropertyTypeSub,Location,Price")] PropertyRE propertyRE)
        {
            /* This is an example of the magic of model binding, that is-
             * - An HTML form has been posted back in
             * - The system knows what a call object is
             * - Maps HTML form data (using Name attribute of an 'input' element, for example) to a call object
             * 
             * Watch "Developing ASP.NET MVC 4 Applications: (03) Developing MVC 4 Controllers". Jump to Time: 24:30 
            */
            /*
             * Model binder refers to the ASP.NET MVC functionality that makes it easier for you to work with data submitted by a form; 
             * a model binder converts posted form values to CLR types and passes them to the action method in parameters. 
             * In this case, the model binder instantiates a propertyRE entity for you using property values from the Form collection    
             * See: http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost
             */
            try
            {
                if (ModelState.IsValid)
                {
                    var propertyservice = new PropertyService();
                    var res = await propertyservice.PostPropertyREAsync(propertyRE);
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            /*
             * See http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost
             */
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            ViewBag.CommunityId = new SelectList(db.Communities, "CommunityId", "Name", propertyRE.CommunityId);
            ViewBag.OwnerID = new SelectList(db.Owners, "OwnerID", "Name", propertyRE.OwnerID);
            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes, "PropertyTypeId", "Name", propertyRE.PropertyTypeId);
            return View(propertyRE);
        }

        /* Original Code
        // GET: PropertyREs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyRE propertyRE = db.Properties.Find(id);
            if (propertyRE == null)
            {
                return HttpNotFound();
            }
            ViewBag.CommunityId = new SelectList(db.Communities, "CommunityId", "Name", propertyRE.CommunityId);
            ViewBag.OwnerID = new SelectList(db.Owners, "OwnerID", "Name", propertyRE.OwnerID);
            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes, "PropertyTypeId", "Name", propertyRE.PropertyTypeId);
            return View(propertyRE);
        }*/

        // GET: PropertyREs/Edit/5
        public async Task< ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var propertyservice = new PropertyService();
            var propertyRE = await propertyservice.GetPropertyREAsync(id);

            if (propertyRE == null)
            {
                return HttpNotFound();
            }

            ViewBag.CommunityId = new SelectList(db.Communities.OrderBy(x => x.Name), "CommunityId", "Name", propertyRE.CommunityId);
            ViewBag.OwnerID = new SelectList(db.Owners.OrderBy(x => x.Name), "OwnerID", "Name", propertyRE.OwnerID);
            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes.OrderBy(x => x.Name), "PropertyTypeId", "Name", propertyRE.PropertyTypeId);

            return View(propertyRE);
        }

        /* Original Code
        // POST: PropertyREs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PropertyREId,PropertyTypeId,CommunityId,OwnerID,Description,PropertyTypeSub,Location,Price")] PropertyRE propertyRE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(propertyRE).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.CommunityId = new SelectList(db.Communities.OrderBy(x => x.Name), "CommunityId", "Name", propertyRE.CommunityId);
            ViewBag.OwnerID = new SelectList(db.Owners.OrderBy(x => x.Name), "OwnerID", "Name", propertyRE.OwnerID);
            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes.OrderBy(x => x.Name), "PropertyTypeId", "Name", propertyRE.PropertyTypeId);

            return View(propertyRE);
        }*/

        // Converted to a call to API
        // POST: PropertyREs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PropertyREId,PropertyTypeId,CommunityId,OwnerID,Description,PropertyTypeSub,Location,Price")] PropertyRE propertyRE)
        // public async Task<ActionResult> Edit(int? id, byte[] rowVersion) // New procedure
        {
            /* New procedure
            string[] fieldsToBind = new string[] { "PropertyREId", "PropertyTypeId", "CommunityId", "OwnerID", "Description", "PropertyTypeSub", "Location,Price" };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/

            if (ModelState.IsValid)
            {
                var propertyservice = new PropertyService();
                var res = await propertyservice.EditPropertyREAsync(propertyRE.PropertyREId, propertyRE); ;
                return RedirectToAction("Index");
            }
            ViewBag.CommunityId = new SelectList(db.Communities.OrderBy(x => x.Name), "CommunityId", "Name", propertyRE.CommunityId);
            ViewBag.OwnerID = new SelectList(db.Owners.OrderBy(x => x.Name), "OwnerID", "Name", propertyRE.OwnerID);
            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes.OrderBy(x => x.Name), "PropertyTypeId", "Name", propertyRE.PropertyTypeId);

            return View(propertyRE);
        }

        /* Original Code
        // GET: PropertyREs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyRE propertyRE = db.Properties.Find(id);
            if (propertyRE == null)
            {
                return HttpNotFound();
            }
            return View(propertyRE);
        } */

        // Converted to a call to API
        // GET: PropertyREs/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var propertyservice = new PropertyService();
            return View(await propertyservice.GetPropertyREDTOAsync(id));
        }

        /* Original Code
        // POST: PropertyREs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PropertyRE propertyRE = db.Properties.Find(id);
            db.Properties.Remove(propertyRE);
            db.SaveChanges();

            return RedirectToAction("Index");
        }*/

        // Converted to a call to API
        // POST: PropertyREs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var propertyservice = new PropertyService();
            var res = await propertyservice.DeletePropertyREAsync(id); ;
            return RedirectToAction("Index");
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

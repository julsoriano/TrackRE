using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using TrackRE.Models;

namespace TrackRE.ServicesAPI
{
    public class OwnerService
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static IEnumerable<Owner> GetOwners()
        {
            foreach(Owner owner in db.Owners)
            {
                yield return owner;
            }
        }

        public static Owner GetOwner(int id)
        {
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return owner;
        }
    }
}
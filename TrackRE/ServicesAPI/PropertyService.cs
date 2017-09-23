using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using TrackRE.Models;

namespace TrackRE.ServicesAPI
{
    public class PropertyService
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static IEnumerable<PropertyRE> GetPropertyREs()
        {
            foreach(PropertyRE propertyre in db.Properties)
            {
                yield return propertyre;
            }
        }

        public static PropertyRE GetPropertyRE(int id)
        {
            PropertyRE propertyre = db.Properties.Find(id);
            if (propertyre == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return propertyre;
        }

        public static IEnumerable<PropertyRE> GetPropertyREsByType(string proptype)
        {
            foreach (PropertyRE propertyre in db.Properties.Where(p => p.PropertyType.Name == proptype))
            {
                yield return propertyre;
            }
        }
    }
}
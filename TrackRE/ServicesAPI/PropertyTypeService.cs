using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using TrackRE.Models;

namespace TrackRE.ServicesAPI
{
    public class PropertyTypeService
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static IEnumerable<PropertyType> GetPropertyTypes()
        {
            //var pt = db.PropertyTypes.Include("Properties");
            foreach (PropertyType propertytype in db.PropertyTypes)
            {
                yield return propertytype;
            }
        }

        public static PropertyType GetPropertyType(int id)
        {
            PropertyType propertytype = db.PropertyTypes.Find(id);
            if (propertytype == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return propertytype;
        }

        public static PropertyType GetPropertyTypeByName(string proptype)
        {
            PropertyType propertytype = db.PropertyTypes.FirstOrDefault(p => p.Name == proptype);
            if (propertytype == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return propertytype;

        }
    }
}
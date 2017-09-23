using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using TrackRE.Models;

namespace TrackRE.ServicesAPI
{
    public class CommunityService
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static IEnumerable<Community> GetCommunities()
        {
            foreach(Community community in db.Communities)
            {
                yield return community;
            }
        }

        public static Community GetCommunity(int id)
        {
            Community community = db.Communities.Find(id);
            if (community == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return community;
        }
    }
}
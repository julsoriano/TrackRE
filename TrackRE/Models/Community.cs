using System.Collections.Generic;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc; 

namespace TrackRE.Models
{
    public class Community
    {
        public int CommunityId { get; set; }
        [DisplayName("Community")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string CommunityUrl { get; set; }

        /*
         * Virtual: Enables EF feature called lazy loading. EF will automatically query and populate the contents
         * of this property whenever one tries to access them. See http://msdn.microsoft.com/en-us/data/jj193542
         */         
        public List<PropertyRE> Properties { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TrackRE.Models
{
    public class PropertyTypeSub
    {
        public int PropertyTypeSubId { get; set; }
        [DisplayName("Property Sub-Type")]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<PropertyRE> Properties { get; set; }
    }
}
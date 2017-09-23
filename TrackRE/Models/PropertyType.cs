using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TrackRE.Models
{
    /*public enum PropertyTypes
    {
        Condo,
        TownHouse,
        House,
        Lot,
        HouseandLot
    }
    */
    public partial class PropertyType
    {
        public int PropertyTypeId { get; set; }
        [DisplayName("Property Type")]
        public string Name { get; set; }
        public string Description { get; set; }

        /*
         * Virtual: Enables EF feature called lazy loading. EF will automatically query and populate the contents
         * of this property whenever one tries to access them. See http://msdn.microsoft.com/en-us/data/jj193542
         */
        public virtual IEnumerable<PropertyRE> Properties { get; set; }
    }
}
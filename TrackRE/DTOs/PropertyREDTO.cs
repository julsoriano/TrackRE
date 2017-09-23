using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrackRE.DTOs
{
    public class PropertyREDTO
    {
        public int PropertyREId { get; set; }

        [DisplayName("Type")]
        public int PropertyTypeId { get; set; }
        [DisplayName("Community")]
        public int CommunityId { get; set; }
        [DisplayName("Owner")]
        public int OwnerID { get; set; }       
        
        [Required(ErrorMessage = "A property description is required")]
        [StringLength(50)] 
        public string Description { get; set; }

        [Required(ErrorMessage = "A property subtype is required")]
        [StringLength(20)]
        [DisplayName("SubType")]
        public string PropertyTypeSub { get; set; }

        [Required(ErrorMessage = "A location is required")]
        [StringLength(50)] 
        public string Location { get; set; }
        
        [Required(ErrorMessage = "Price is required")]
        [Range(1000000, 100000000,
            ErrorMessage = "Price must be between 1000000 and 100000000")]
        [DisplayName("Price in PHP")]
        public decimal Price { get; set; }

        public string PropertyType { get; set; }
        public string Community { get; set; }
        public string Owner { get; set; }
    }
}
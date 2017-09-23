using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TrackRE.Models
{
    public partial class PropertyRE
    {
        public int PropertyREId { get; set; }
       
        [DisplayName("Type")]
        //[ForeignKey("PropertyTypeId")]
        public int PropertyTypeId { get; set; }
        [DisplayName("Community")]
        //[ForeignKey("CommunityId")]
        public int CommunityId { get; set; }
        [DisplayName("Owner")]
        //[ForeignKey("OwnerID")]
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

        [Timestamp]
        public byte[] RowVersion { get; set; }

        /*
        * Virtual: Enables EF feature called lazy loading. EF will automatically query and populate the contents
        * of these properties whenever one tries to access them. See http://msdn.microsoft.com/en-us/data/jj193542
        */
        [ForeignKey("PropertyTypeId")]
        [JsonIgnore]
        public virtual PropertyType PropertyType { get; set; }

        [ForeignKey("CommunityId")]
        [JsonIgnore]
        public virtual Community Community { get; set; }

        [ForeignKey("OwnerID")]
        [JsonIgnore]
        public virtual Owner Owner { get; set; }

    }
}
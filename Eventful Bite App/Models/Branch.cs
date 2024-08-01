using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eventful_Bite_App.Models
{
    public class Branch
    {
        [Key]
        public int BranchId { get; set; }
        public string Status { get; set; }
        public string Review { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }

        //A branch belongs to one restaurant
        //Each restaurant can have many branches 
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }

    public class BranchDto
    {
        public int BranchId { get; set; }
        public int RestaurantId { get; set; }
        public string Status { get; set; }
        public string Review { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantType { get; set; }
        public string Cuisine { get; set; }
        public string Budget { get; set; }
    }
}
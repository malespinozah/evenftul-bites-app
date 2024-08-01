using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eventful_Bite_App.Models
{
    public class District
    {
        [Key]
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
    }

    // A Data Transfer Object (DTO)
    // Communicating the clothing item information externally 
    public class DistrictDto
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
    }

}
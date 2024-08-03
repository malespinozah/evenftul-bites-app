using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eventful_Bite_App.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }

        [Required]
        // This ensures it will only store the date part
        public DateTime EventDate { get; set; }

        [Required]
        // This ensures it will only store the time part
        public string EventTime => EventDate.ToString("hh:mm tt");

        [Required]
        // This will format the price as a currency
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        // This ensures the price value is valid and formatted correctly
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price; Maximum Two Decimal Points.")]
        public decimal EventPrice { get; set; }

        public string EventAddress { get; set; }

        // An event happens in one city
        // A city can have many events
        [ForeignKey("District")]
        public int DistrictId { get; set; }
        public virtual District District { get; set; }
    }

    // A Data Transfer Object (DTO)
    // Communicating the clothing item information externally 
    public class EventDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public string EventTime { get; set; }
        public decimal EventPrice { get; set; }
        public string EventAddress { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
    }

}
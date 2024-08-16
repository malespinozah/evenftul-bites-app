using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using System.Linq;
using System.Web;

namespace Eventful_Bite_App.Models
{
    public class Journal
    {
        [Key]
        public int JournalId { get; set; }

        [Required]
        public string EventName { get; set; }

        [Required]
        public string RestaurantName { get; set; }
        public string JournalTitle { get; set; }
        public DateTime EntryDate { get; set; }
        public string JournalDescription { get; set; }

    }
    //Data Transfer Object
    public class JournalDto
    {
        public int JournalId { get; set; }
        public string EventName { get; set; }
        public string RestaurantName { get; set; }
        public string JournalTitle { get; set; }
        public DateTime EntryDate { get; set; }
        public string JournalDescription { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eventful_Bite_App.Models
{
    public class BranchDetails
    {
        public BranchDto Branch { get; set; }
        public IEnumerable<EventDto> Events { get; set; }
    }
}
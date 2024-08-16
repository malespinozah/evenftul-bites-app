using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eventful_Bite_App.Models
{
    public class EventDetails
    {

        public EventDto Event { get; set; }
        public IEnumerable<BranchDto> Branches { get; set; }

    }
}
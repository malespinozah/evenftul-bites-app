using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eventful_Bite_App.Models
{
    public class DetailsDistrict
    {
        public DistrictDto SelectedDistrict { get; set; }
        public IEnumerable<EventDto> KeptEvents { get; set; }
        public IEnumerable<BranchDto> Branches { get; set; } = new List<BranchDto>();
    }
}
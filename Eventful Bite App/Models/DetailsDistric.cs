using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eventful_Bite_App.Models
{
    public class DetailsDistric
    {
        public DistrictDto SelectedDistrict { get; set; }
        public IEnumerable<EventDto> KeptEvents { get; set; }

    }
}
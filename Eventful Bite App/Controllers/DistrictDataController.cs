using Eventful_Bite_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Eventful_Bite_App.Controllers
{
    public class DistrictDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all districts from the database.
        /// </summary>
        /// <returns>Returns an IHttpActionResult containing a list of DistrictDto objects representing the districts.</returns>
        [HttpGet]
        [ResponseType(typeof(EventDto))]
        public IHttpActionResult ListDistrict()
        {
            List<District> District = db.Districts.ToList();
            List<DistrictDto> DistrictDtos = new List<DistrictDto>();

            District.ForEach(C => DistrictDtos.Add(new DistrictDto()
            {
                DistrictId = C.DistrictId,
                DistrictName = C.DistrictName,
            }));

            return Ok(DistrictDtos);
        }

        [HttpGet]
        [Route("api/DistrictData/ListDistricts")]
        public List<DistrictDto> ListDistricts()
        {
            List<District> Districts = db.Districts.ToList();
            List<DistrictDto> DistrictDtos = new List<DistrictDto>();

            foreach (District District in Districts)
            {
                DistrictDto DistrictDto = new DistrictDto();

                DistrictDto.DistrictId = District.DistrictId;
                DistrictDto.DistrictName = District.DistrictName;

                DistrictDtos.Add(DistrictDto);

            }
            return DistrictDtos;
        }

        /// <summary>
        /// Retrieves the details of a specific district by ID.
        /// </summary>
        /// <param name="id">The ID of the district to retrieve.</param>
        /// <returns>Returns an IHttpActionResult containing the DistrictDto object with the district's details.</returns>
        [HttpGet]
        [Route("api/DistrictData/FindDistrict/{id}")]
        public IHttpActionResult FindDistrict(int id)
        {
            District District = db.Districts.Find(id);
            DistrictDto DistrictDto = new DistrictDto();
            DistrictDto.DistrictId = District.DistrictId;
            DistrictDto.DistrictName = District.DistrictName;

            return Ok(DistrictDto);
        }

    }
}

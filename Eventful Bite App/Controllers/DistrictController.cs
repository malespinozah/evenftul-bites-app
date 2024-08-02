using Eventful_Bite_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Eventful_Bite_App.Controllers
{
    public class DistrictController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DistrictController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44301/api/DistrictData/");
        }

        /// <summary>
        /// Retrieves a list of districts from the API and displays them.
        /// </summary>
        /// <returns>Returns the List view with a collection of DistrictDto objects.</returns>
        // GET: District/List
        public ActionResult List()
        {
            //objective: communicate with our District data api to retrieve a list of events
            //curl https://localhost:44301/api/DistrictData/ListDistricts


            string url = "https://localhost:44301/api/DistrictData/ListDistricts";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DistrictDto> Districts = response.Content.ReadAsAsync<IEnumerable<DistrictDto>>().Result;
            //Debug.WriteLine("Number of events received : ");
            //Debug.WriteLine(Events.Count());

            return View(Districts);
        }

        /// <summary>
        /// Retrieves the details of a specific district and its related events from the API.
        /// </summary>
        /// <param name="id">The ID of the district to retrieve.</param>
        /// <returns>Returns the Details view with the selected district details and related events.</returns>
        // GET: District/Details/{id}
        public async Task<ActionResult> Details(int id)
        {
            DetailsDistrict ViewModel = new DetailsDistrict();

            // Fetch district details
            string url = $"https://localhost:44301/api/DistrictData/FindDistrict/{id}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                ViewModel.SelectedDistrict = await response.Content.ReadAsAsync<DistrictDto>();
            }
            else
            {
                // Handle error or return a view with an error message
                return View("Error");
            }

            // Fetch events for the district
            url = $"https://localhost:44301/api/EventData/ListEventsForDistrict/{id}";
            response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                ViewModel.KeptEvents = await response.Content.ReadAsAsync<IEnumerable<EventDto>>();
            }
            else
            {
                ViewModel.KeptEvents = new List<EventDto>(); // Ensure KeptEvents is not null
            }

            // Fetch branches for the district (assuming district name matches location)
            url = $"https://localhost:44301/api/BranchData/ListBranchesByLocation/{ViewModel.SelectedDistrict.DistrictName}";
            HttpResponseMessage branchesResponse = await client.GetAsync(url);
            if (branchesResponse.IsSuccessStatusCode)
            {
                ViewModel.Branches = await branchesResponse.Content.ReadAsAsync<IEnumerable<BranchDto>>();
            }
            else
            {
                ViewModel.Branches = new List<BranchDto>(); // Ensure Branches is not null
            }

            return View(ViewModel);
        }

    }
}
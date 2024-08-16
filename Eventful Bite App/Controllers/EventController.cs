using Eventful_Bite_App.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Eventful_Bite_App.Controllers
{
    public class EventController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static EventController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44301/api/EventData/");
        }

        /// <summary>
        /// Retrieves a list of events from the API and displays them.
        /// </summary>
        /// <returns>Returns the List view with a collection of EventDto objects.</returns>
        // GET: Event/List
        //[Authorize]
        public ActionResult List()
        {
            //objective: communicate with our Event data api to retrieve a list of events
            //curl https://localhost:44301/api/EventData/ListEvents

            HttpClient client = new HttpClient();
            string url = "https://localhost:44301/api/EventData/ListEvents";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<EventDto> Events = response.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;
            //Debug.WriteLine("Number of events received : ");
            //Debug.WriteLine(Events.Count());

            return View(Events);
        }

        /// <summary>
        /// Retrieves the details of a specific Event from the API.
        /// </summary>
        /// <param name="id">The ID of the Event to retrieve.</param>
        /// <returns>Returns the Details view with the selected Event details.</returns>
        // GET: Event/Details/{id}
        public ActionResult Details(int id)
        {
            //objective: communicate with our Event data api to retrieve one Event
            //curl https://localhost:44301/api/EventData/FindEvents/{id}

            HttpClient client = new HttpClient();
            string eventUrl = "https://localhost:44301/api/EventData/FindEvents/" + id;
            HttpResponseMessage eventResponse = client.GetAsync(eventUrl).Result;

            EventDto eventDto = eventResponse.Content.ReadAsAsync<EventDto>().Result;

            //fetch branches in the same district or location
            string branchUrl = $"https://localhost:44301/api/BranchData/ListBranchesByLocation/{eventDto.DistrictName}";
            HttpResponseMessage branchResponse = client.GetAsync(branchUrl).Result;
            IEnumerable<BranchDto> branches = branchResponse.Content.ReadAsAsync<IEnumerable<BranchDto>>().Result;

            // Create and populate the view model
            var viewModel = new EventDetails
            {
                Event = eventDto,
                Branches = branches
            };
            return View(viewModel);
        }

        /// <summary>
        /// Retrieves the details of a specific Event from the API.
        /// </summary>
        /// <param name="id">The ID of the Event to retrieve.</param>
        /// <returns>Returns the Details view with the selected Event details.</returns>
        // GET: Event/Create
       [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Adds a new Event to the system using the API.
        /// </summary>
        /// <param name="Event">The Event object to add.</param>
        /// <returns>Redirects to the List action.</returns>
        // POST: Event/Add
        [HttpPost]
        public ActionResult Add(Event Event)
        {
            //Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Event.EventName);
            //objective: add a new Event into our system using the API
            //curl -H "Content-Type:application/json" -d @Event.json https://localhost:44301/api/EventData/AddEvent 
            HttpClient client = new HttpClient();
            string url = "https://localhost:44301/api/EventData/AddEvent";

            string jsonpayload = jss.Serialize(Event);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("List");

        }

        /// <summary>
        /// Retrieves the details of an Event for editing.
        /// </summary>
        /// <param name="id">The ID of the Event to edit.</param>
        /// <returns>Returns the Edit view with the selected Event details.</returns>
        // GET: Event/Edit/2
        //[Authorize]
        public ActionResult Edit(int id)
        {
            string url = "https://localhost:44301/api/EventData/FindEvents/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EventDto selectedEvent = response.Content.ReadAsAsync<EventDto>().Result;
            return View(selectedEvent);
        }

        /// <summary>
        /// Updates an existing Event in the system.
        /// </summary>
        /// <param name="id">The ID of the Event to update.</param>
        /// <param name="Event">The updated Event object.</param>
        /// <returns>Redirects to the List action.</returns>
        // POST: Event/Update/2
        [HttpPost]
        public ActionResult Update(int id, Event Event)
        {
            string url = "https://localhost:44301/api/EventData/UpdateEvents/" + id;
            string jsonpayload = jss.Serialize(Event);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return RedirectToAction("List");
        }

        /// <summary>
        /// Confirms the deletion of an Event.
        /// </summary>
        /// <param name="id">The ID of the Event to delete.</param>
        /// <returns>Returns the DeleteConfirm view with the selected Event details.</returns>
        // GET: Event/Delete/1
        //[Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "https://localhost:44301/api/EventData/FindEvents/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EventDto selectedEvent = response.Content.ReadAsAsync<EventDto>().Result;
            return View(selectedEvent);
        }

        /// <summary>
        /// Deletes an Event from the system.
        /// </summary>
        /// <param name="id">The ID of the Event to delete.</param>
        /// <returns>Redirects to the List action.</returns>
        // POST: Event/Delete/11
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "https://localhost:44301/api/EventData/DeleteEvent/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return RedirectToAction("List");
        }

        /// <summary>
        /// Displays an error view.
        /// </summary>
        /// <returns>Returns the Error view.</returns>
        public ActionResult Error()
        {
            return View();
        }

    }
}
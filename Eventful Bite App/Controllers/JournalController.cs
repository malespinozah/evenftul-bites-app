using Eventful_Bite_App.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Eventful_Bite_App.Controllers
{
    public class JournalController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static JournalController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44301/api/");
        }

        // GET: Journal/List
        public ActionResult List()
        {
            //Comunicate with the journal data API to retrive the list of journal entries
            //curl https://localhost:44301/api/journaldata/ListJournalEntries

            string url = "https://localhost:44301/api/journaldata/ListJournalEntries";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<JournalDto> Journals = response.Content.ReadAsAsync<IEnumerable<JournalDto>>().Result;

            //Debug.WriteLine("Number of journal entries received");
            //Debug.WriteLine(journals.Count());

            return View(Journals);
        }

        // GET: Journal/Details/3
        public ActionResult Details(int id)
        {
            //Comunicate with the Journal data API to retrive one journal entry
            //curl https://localhost:44301/api/journaldata/FindJournalEntry/{id}

            string url = "https://localhost:44301/api/journaldata/FindJournalEntry/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            JournalDto journalDto = response.Content.ReadAsAsync<JournalDto>().Result;

            //Debug.WriteLine("Entry received:");
            //Debug.WriteLine(selectedJournalEntry.JournalTitle);

            return View(journalDto);
        }


        public ActionResult Error()
        {
            return View();
        }

        // GET: Journal/Create
        public ActionResult Create()
        {
            return View();
        }


        ////REVISE THIS ONCE CODED IS COMPLETED TO SEE IF IT WILL ADD THE DATA

        //// POST: Journal/Create
        //[HttpPost]
        //public ActionResult Create(Journal Journal, int id)
        //{
        //    //Debug.WriteLine("The journal entry added is:");
        //    //Debug.WriteLine(journal.JournalEntryTitle);

        //    //Add a new journal entry into the system using the API
        //    //curl -d @journalentry.json -H "Content-Type:application/json" https://localhost:44301/api/EventJournal/Details/{id}

        //    string url = "EventJournal/Details/" + id;

        //    string jsonpayload = jss.Serialize(Journal);

        //    Debug.WriteLine(jsonpayload);

        //    HttpContent content = new StringContent(jsonpayload);
        //    content.Headers.ContentType.MediaType = "application/json";

        //    HttpResponseMessage response = client.PostAsync(url, content).Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("List");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Error");
        //    }
        //}
        [HttpPost]
        public ActionResult Create(string EventName, string RestaurantName, string JournalTitle, string JournalDescription)
        {
            var journal = new Journal
            {
                EventName = EventName,
                RestaurantName = RestaurantName,
                JournalTitle = JournalTitle,
                JournalDescription = JournalDescription,
                EntryDate = DateTime.Now
            };

            string jsonPayload = jss.Serialize(journal);
            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";

            string url = "journalData/AddJournalEntry";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            string responseContent = response.Content.ReadAsStringAsync().Result;
            Debug.WriteLine("Response Content: " + responseContent);

            return RedirectToAction("List");
            //if (response.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("List");
            //}
            //else
            //{
            //    // Check if the response indicates a specific error
            //    TempData["ErrorMessage"] = responseContent;
            //    return RedirectToAction("Error");
            //}
        }



        // GET: Journal/Edit/3
        public ActionResult Edit(int id)
        {
            //grab the information of a journal entry in the list of games

            //Comunicate with the Journal data API to retrive one journal entry
            //curl https://localhost:44301/api/journaldata/FindJournalEntry/{id}

            string url = "journaldata/FindJournalEntry/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            JournalDto selectedEntry = response.Content.ReadAsAsync<JournalDto>().Result;

            return View(selectedEntry);
        }

        // POST: Journal/Update/3
        [HttpPost]
        public ActionResult Update(int id, Journal Journal)
        {
            //After retriving the journal entry data, updates the entry data in the system using the API
            //curl -d @game.json -H "Content-Type:application/json" "https://localhost:44301/api/journaldata/UpdateJournal/3

            string url = "journaldata/UpdateJournal/" + id;

            string jsonpayload = jss.Serialize(Journal);

            HttpContent content = new StringContent(jsonpayload);

            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + id);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Journal/Delete/3
        public ActionResult DeleteConfirm(int id)
        {
            //Comunicate with the Journal data API to retrive one journal entry
            //curl https://localhost:44301/api/journaldata/FindJournalEntry/{id}

            string url = "journaldata/FindJournalEntry/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            JournalDto selectedEntry = response.Content.ReadAsAsync<JournalDto>().Result;

            return View(selectedEntry);
        }

        // POST: Journal/Delete/3
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Comunicate with the Journal data API to delete one journal entry
            //curl https://localhost:44301/api/journaldata/DeleteJournalEntry/{id}

            string url = "journaldata/DeleteJournalEntry/" + id;

            HttpContent content = new StringContent("");

            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        //Might merge EventJournal Controller with this Controller
        //Unsure if Add has to be rendered from this controller or EventJournal Controller
    }
}

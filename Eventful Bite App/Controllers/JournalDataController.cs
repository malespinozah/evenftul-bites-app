using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Eventful_Bite_App.Models;

namespace Eventful_Bite_App.Controllers
{
    public class JournalDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all the journal entries in the system
        /// </summary>
        /// <return>
        /// HEADERL 200 (OK)
        /// CONTENT: All the journals entries in the database
        /// </return>
        /// <example>
        /// GET: api/JournalData/ListJournalsEntries
        /// </example>
        [HttpGet]
        [Route("api/JournalData/ListJournalEntries")]
        public IEnumerable<JournalDto> ListJournalEntries()
        {
            List<Journal> Journals = db.Journals.ToList();
            List<JournalDto> JournalDtos = new List<JournalDto>();

            Journals.ForEach(j => JournalDtos.Add(new JournalDto()
            {
                JournalId = j.JournalId,
                EventName = j.EventName,
                RestaurantName = j.RestaurantName,
                JournalTitle = j.JournalTitle,
                EntryDate = j.EntryDate,
                JournalDescription = j.JournalDescription
            }));

            return JournalDtos;
        }

        /// <summary>
        /// Retrieves the details of a specific Journal Entry, by ID.
        /// </summary>
        /// <param name="id">The Id of the specific entry.</param>
        /// <returns>/// CONTENT: A Journal Entry and the information of the Player in the system matching the ID provided.</returns>
        /// <example>
        /// GET: api/JournalData/FindJournalEntry/2
        /// </example>
        [ResponseType(typeof(Journal))]
        [HttpGet]
        public IHttpActionResult FindJournalEntry(int id)
        {
            Journal Journal = db.Journals.Find(id);

            // Check if the journal entry is not found
            if (Journal == null)
            {
                return NotFound();
            }
            JournalDto JournalDto = new JournalDto();

            JournalDto.JournalId = Journal.JournalId;
            JournalDto.EventName = Journal.EventName;
            JournalDto.RestaurantName = Journal.RestaurantName;
            JournalDto.JournalTitle = Journal.JournalTitle;
            JournalDto.EntryDate = JournalDto.EntryDate;
            JournalDto.JournalDescription = Journal.JournalDescription;

            if(Journal == null)
            {
                return NotFound();
            }

            return Ok(JournalDto);
        }

        /// <summary>
        /// Adds a new Journal Entry into the system
        /// </summary>
        /// <param name="Journal">JSON form Data of a Journal Entry</param>
        /// <return>
        /// CONTENT: journalEntry ID, Journal Data
        /// or 
        /// HEADER: 400 (Bad Request)
        /// </return>
        /// <example>
        /// POST: api/JournalData/AddJournal
        /// FORM DATA: journal JSON Object
        /// </example>

        [HttpPost]
        //[Route("api/EventJournal/Details/{id}")] //Not sure if this might work, since the controller was created on a separate page????
        [Route("api/JournalData/AddJournalEntry")]
        [ResponseType(typeof(Journal))]

        public IHttpActionResult AddJournalEntry(Journal Journal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Journals.Add(Journal);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Journal.JournalId }, Journal);
        }


        /// <summary>
        /// Updates a journal entry in the system with the POST Data input
        /// </summary>
        /// <param name="id">Represents the journal entry ID primary key</param>
        /// <param name="Journal">JSON form Data of a journal entry</param>
        /// <return>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </return>
        /// <example>
        /// POST: api/JournalData/UpdateJournalEntry/3
        /// FORM DATA: journal JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/JournalData/UpdateJournalEntry/{id}")]
        public IHttpActionResult UpdateJournalEntry(int id, Journal Journal)
        {
            Debug.WriteLine("I have reached the journal update method");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Journal.JournalId)
            {
                Debug.WriteLine("Incorrect ID");
                Debug.WriteLine("GET parameter" + id);
                return BadRequest();
            }

            db.Entry(Journal).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JournalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("No condition triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes a journal entry from the system by it's ID
        /// </summary>
        /// <param name="id">JSON form Data of a Journal Entry</param>
        /// <return>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </return>
        /// <example>
        /// POST: api/JournalData/DeleteJournalEntry/3
        /// FORM DATA: (empty)
        /// </example>

        [ResponseType(typeof(Journal))]
        [HttpPost]
        public IHttpActionResult DeleteJournalEntry(int id)
        {
            Journal Journal = db.Journals.Find(id);
            if (Journal == null)
            {
                return NotFound();
            }

            db.Journals.Remove(Journal);
            db.SaveChanges();

            return Ok(Journal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JournalExists(int id)
        {
            return db.Journals.Count(e => e.JournalId == id) > 0;
        }
    }
}
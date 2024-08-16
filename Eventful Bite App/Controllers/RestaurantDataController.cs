using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics;
using Eventful_Bite_App.Models;



namespace Eventful_Bite_App.Controllers
{
    public class RestaurantDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns a list of restaurants in the system
        /// </summary>
        /// <returns>
        /// Returns an array of restaurants that matches the Search Key (Restaurant Name). Will return complete list if no search key is input.
        /// </returns>
        /// <example>
        /// //GET: /api/RestaurantData/ListRestaurants -> [{"RestaurantId": "1", "RestaurantName": "Miss Lin Cafe"}, {"RestaurantId": "2", "RestaurantName": "Daldongnae Korean BBQ"}]
        /// </example>
        [HttpGet]
        [Route("api/RestaurantData/ListRestaurants")]
        public IEnumerable<RestaurantDto> ListRestaurants(string SearchKey = null)
        {
            List<Restaurant> Restaurants;

            if (!string.IsNullOrEmpty(SearchKey))
            {
                Restaurants = db.Restaurants.Where(x => x.RestaurantName.Contains(SearchKey)).ToList();
            }
            else
            {
                Restaurants = db.Restaurants.ToList();

            }

            List<RestaurantDto> RestaurantDtos = new List<RestaurantDto>();

            foreach (Restaurant Restaurant in Restaurants)
            {
                RestaurantDto RestaurantDto = new RestaurantDto();

                RestaurantDto.RestaurantId = Restaurant.RestaurantId;
                RestaurantDto.RestaurantName = Restaurant.RestaurantName;
                RestaurantDto.RestaurantType = Restaurant.RestaurantType;
                RestaurantDto.Cuisine = Restaurant.Cuisine;
                RestaurantDto.Budget = Restaurant.Budget;

                RestaurantDtos.Add(RestaurantDto);
            }
            return RestaurantDtos;
        }



        /// <summary>
        /// Returns information of a restaurant
        /// </summary>
        /// <param name="id">Restaurant id</param>
        /// <returns>
        /// singular restaurant of the corresponding restaurant id</returns>
        /// <example>
        /// //GET: /api/RestaurantData/ListRestaurants/1 -> [{"RestaurantId": "1", "RestaurantName": "Miss Lin Cafe"}]
        /// </example>
        [HttpGet]
        [Route("api/RestaurantData/FindRestaurant/{id}")]
        public RestaurantDto FindRestaurant(int id)
        {
            Restaurant Restaurant = db.Restaurants.Find(id);

            RestaurantDto RestaurantDto = new RestaurantDto();

            RestaurantDto.RestaurantId = Restaurant.RestaurantId;
            RestaurantDto.RestaurantName = Restaurant.RestaurantName;
            RestaurantDto.RestaurantType = Restaurant.RestaurantType;
            RestaurantDto.Cuisine = Restaurant.Cuisine;
            RestaurantDto.Budget = Restaurant.Budget;

            return RestaurantDto;

        }


        /// <summary>
        /// Updates information of an existing restaurant
        /// </summary>
        /// <param name="id">Restaurant Id</param>
        /// <param name="Restaurant">Restarurant data to be updated</param>
        /// <returns>
        /// Returns no content if update is successful
        /// Returns Bad Request/Not found along with the status code if update is invalid
        /// </returns>
        // POST: api/RestaurantData/UpdateRestaurant/2
    
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/restaurantdata/updaterestaurant/{id}")]
        public IHttpActionResult UpdateRestaurant(int id, Restaurant Restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Restaurant.RestaurantId)
            {

                return BadRequest();
            }

            db.Entry(Restaurant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        //check if restaurant exists
        private bool RestaurantExists(int id)
        {
            return db.Restaurants.Count(e => e.RestaurantId == id) > 0;
        }


        /// <summary>
        /// Adds restaurant to database
        /// </summary>
        /// <param name="Restaurant">Restaurant data to be added to database </param>
        /// <returns>
        /// Returns bad request if update is invalid
        /// Returns success message if update is successful
        /// </returns>
        // POST: api/RestaurantData/AddRestaurant
        [ResponseType(typeof(Restaurant))]
        [HttpPost]
        [Route("api/restaurantdata/addrestaurant")]

        public IHttpActionResult AddRestaurant(Restaurant Restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Restaurants.Add(Restaurant);
            db.SaveChanges();

            // Return the newly created restaurant with its ID
            return CreatedAtRoute("RestaurantDetails", new { id = Restaurant.RestaurantId }, Restaurant);
        }


        /// <summary>
        /// Deletes a restaurant from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the restaurant</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/restaurantData/Deleterestaurant/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Restaurant))]
        [HttpPost]
        [Route("api/restaurantdata/deleterestaurant/{id}")]

        public IHttpActionResult DeleteRestaurant(int id)
        {
            Restaurant Restaurant = db.Restaurants.Find(id);
            if (Restaurant == null)
            {
                return NotFound();
            }

            db.Restaurants.Remove(Restaurant);
            db.SaveChanges();

            return Ok();
        }

    }

}

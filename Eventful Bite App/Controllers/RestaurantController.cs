using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Eventful_Bite_App.Models;

namespace Eventful_Bite_App.Controllers
{
    public class RestaurantController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RestaurantController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44301/api/restaurantdata/");
        }

        // GET: Restaurant/List
        [HttpGet]
        public ActionResult List(string SearchKey)
        {
            //objective: communicate with our restaurant data api to retrieve a list of restaurants
            //curl https://localhost:44301/api/restaurantdata/listrestaurants


            string url = "listrestaurants";


            if (!string.IsNullOrEmpty(SearchKey))
            {
                url += "?SearchKey=" + SearchKey;
            }

            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<RestaurantDto> Restaurants = response.Content.ReadAsAsync<IEnumerable<RestaurantDto>>().Result;
            //var json = response.Content.ReadAsStringAsync().Result;
            //IEnumerable<RestaurantDto> Restaurants = JsonConvert.DeserializeObject<IEnumerable<RestaurantDto>>(json);
            return View(Restaurants);
        }


        // GET: Restaurant/Details/1
        public ActionResult Details(int id)
        {
            //objective: communicate with our restarurant data api to retrieve one restaurant
            //curl https://localhost:44355/api/restaurantdata/findrestaurant/{id}

            string url = "findrestaurant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            RestaurantDto SelectedRestaurant = response.Content.ReadAsAsync<RestaurantDto>().Result;
            //Debug.WriteLine("restaurant received : ");
            //Debug.WriteLine(selectedrestaurant.RestaurantName);

            return View(SelectedRestaurant);
        }


        // GET: Restaurant/Edit/1
        public ActionResult Edit(int id)
        {
            //grab the Restaurant information

            //objective: communicate with our restaurant data api to retrieve one restaurant
            //curl https://localhost:44355/api/restaurantdata/findrestaurant/{id}

            string url = "findrestaurant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            RestaurantDto SelectedRestaurant = response.Content.ReadAsAsync<RestaurantDto>().Result;

            return View(SelectedRestaurant);
        }



        // POST: Restaurant/Update/5
        [Authorize]
        [HttpPost]
        public ActionResult Update(int id, Restaurant Restaurant)
        {
            try
            {
                Debug.WriteLine("The new restaurant info is:");
                Debug.WriteLine(Restaurant.RestaurantName);
                Debug.WriteLine(Restaurant.RestaurantType);
                Debug.WriteLine(Restaurant.Cuisine);
                Debug.WriteLine(Restaurant.Budget);

                //serialize into JSON
                //Send the request to the API

                string url = "UpdateRestaurant/" + id;


                string jsonpayload = jss.Serialize(Restaurant);
                Debug.WriteLine(jsonpayload);


                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                Debug.WriteLine("Still good");


                //POST: api/RestaurantData/UpdateRestaurant/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                Debug.WriteLine("Reached the end");
                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }




        // POST: Restaurant/Add
        public ActionResult Add()
        {
            return View();
        }


        // POST: Restaurant/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Restaurant Restaurant)
        {
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(Restaurant.RestaurantName);
            //objective: add a new animal into our system using the API
            //curl -H "Content-Type:application/json" -d @restaurant.json https://localhost:44355/api/restaurantdata/addrestaurant 
            string url = "addrestaurant";


            string jsonpayload = jss.Serialize(Restaurant);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response to get the created restaurant with its ID
                var createdRestaurant = response.Content.ReadAsAsync<RestaurantDto>().Result;
                // Redirect to the Details page of the newly created restaurant
                return RedirectToAction("Details", new { id = createdRestaurant.RestaurantId });
            }
            else
            {
                Debug.WriteLine("Error: Response status code: " + response.StatusCode);
                Debug.WriteLine("Error: Response content: " + response.Content.ReadAsStringAsync().Result);
                return RedirectToAction("Error");
            }
        }



        // POST: Restaurant/Error
        public ActionResult Error()
        {
            return View();
        }


        // GET: Restaurant/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findrestaurant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RestaurantDto SelectedRestaurant = response.Content.ReadAsAsync<RestaurantDto>().Result;
            return View(SelectedRestaurant);
        }

        // POST: Restaurant/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "deleterestaurant/" + id;
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
    }
}
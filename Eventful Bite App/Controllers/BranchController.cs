using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Eventful_Bite_App.Models;

namespace Eventful_Bite_App.Controllers
{
    public class BranchController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BranchController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44301/api/branchdata/");
        }

        // GET: Branch/List
        public ActionResult List(string SearchKey)
        {
            //objective: communicate with our restaurant's branches data api to retrieve a list of branches
            //curl https://localhost:44301/api/restaurantdata/listrestaurants

            string url = "listbranches";

            if (!string.IsNullOrEmpty(SearchKey))
            {
                url += "?SearchKey=" + SearchKey;
            }

            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<BranchDto> Branches = response.Content.ReadAsAsync<IEnumerable<BranchDto>>().Result;

            return View(Branches);
        }


        // GET: Branch/Details/1
        public ActionResult Details(int id)
        {
            //objective: communicate with our restarurant branch data api to retrieve details of one single branch 
            //curl https://localhost:44301/api/branchdata/findbranch/{id}

            string url = "findbranch/" + id;
            HttpResponseMessage branchResponse = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            BranchDto SelectedBranch = branchResponse.Content.ReadAsAsync<BranchDto>().Result;
            //Debug.WriteLine("branch received : ");
            //Debug.WriteLine(SelectedBranch.RestaurantId.RestaurantName);

            //Fetcch events based on the branch location
            string location = SelectedBranch.Location;
            string eventsUrl = "https://localhost:44301/api/eventdata/listeventsbylocation/" + location;

            HttpResponseMessage eventsResponse = client.GetAsync(eventsUrl).Result;
            IEnumerable<EventDto> events = eventsResponse.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;

            var viewModel = new BranchDetails
            {
                Branch = SelectedBranch,
                Events = events
            };

            return View(viewModel);
        }



        // GET: Branch/Edit/1
        public ActionResult Edit(int id)
        {
            //grab the Restaurant Branch information

            //objective: communicate with our branch data api to retrieve one branch of a restaurant
            //curl https://localhost:44301/api/branchdata/findbranch/{id}

            string url = "findbranch/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            BranchDto SelectedBranch = response.Content.ReadAsAsync<BranchDto>().Result;

            return View(SelectedBranch);
        }



        // POST: Branch/Update/5
        [Authorize]
        [HttpPost]
        public ActionResult Update(int id, Branch Branch)
        {
            try
            {
                Debug.WriteLine("The new branch info is:");
                Debug.WriteLine(Branch.Status);
                Debug.WriteLine(Branch.Review);
                Debug.WriteLine(Branch.Location);
                Debug.WriteLine(Branch.Address);

                //serialize into JSON
                //Send the request to the API

                string url = "UpdateBranch/" + id;


                string jsonpayload = jss.Serialize(Branch);
                Debug.WriteLine(jsonpayload);


                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                Debug.WriteLine("Still good");


                //POST: api/BranchData/UpdateBranch/{id}
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


        // GET: Branch/Add/1
        public ActionResult Add(int id)
        {

            //grab the Restaurant information

            //objective: communicate with our restaurant data api to retrieve one restaurant
            //curl https://localhost:44301/api/restaurantdata/findrestaurant/{id}

            HttpClient client = new HttpClient();
            string url = "https://localhost:44301/api/restaurantdata/findrestaurant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            RestaurantDto SelectedRestaurant = response.Content.ReadAsAsync<RestaurantDto>().Result;

            return View(SelectedRestaurant);
        }



        // POST: Branch/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Branch Branch)
        {
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(Branch.Location);
            //objective: add a new branch into our system using the API
            //curl -H "Content-Type:application/json" -d @restaurant.json https://localhost:44355/api/branchdata/addbranch 
            string url = "addbranch";


            string jsonpayload = jss.Serialize(Branch);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response to get the created branch with its ID
                var createdBranch = response.Content.ReadAsAsync<BranchDto>().Result;
                // Redirect to the Details page of the newly created restaurant
                return RedirectToAction("ListByRestaurant/" + Branch.RestaurantId);
            }
            else
            {
                Debug.WriteLine("Error: Response status code: " + response.StatusCode);
                Debug.WriteLine("Error: Response content: " + response.Content.ReadAsStringAsync().Result);
                return RedirectToAction("Error");
            }
        }



        // POST: Branch/Error
        public ActionResult Error()
        {
            return View();
        }


        // GET: Branch/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findbranch/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BranchDto SelectedBranch = response.Content.ReadAsAsync<BranchDto>().Result;
            return View(SelectedBranch);
        }

        // POST: Branch/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "deletebranch/" + id;
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

        // GET: Branch/ListByRestaurant
        public ActionResult ListByRestaurant(int id)
        {
            //objective: communicate with our restaurant branch data api to retrieve a list of branches based on the restaurant
            //curl https://localhost:44301/api/restaurantdata/listbyrestaurant

            //Get all branches by id
            string url = "listbranchesbyrestaurant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);
            IEnumerable<BranchDto> Branches = response.Content.ReadAsAsync<IEnumerable<BranchDto>>().Result;

            //Get restaurant name from restaurant details based on id
            string restUrl = "https://localhost:44301/api/restaurantdata/findrestaurant/" + id;
            HttpResponseMessage nameResponse = client.GetAsync(restUrl).Result;
            RestaurantDto Restaurant = nameResponse.Content.ReadAsAsync<RestaurantDto>().Result;

            ViewData["id"] = id;
            ViewData["RestaurantName"] = Restaurant.RestaurantName;
            return View(Branches);
        }


    }

}
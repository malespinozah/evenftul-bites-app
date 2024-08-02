using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Eventful_Bite_App.Models;


namespace Eventful_Bite_App.Controllers
{
    public class BranchDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

    /// <summary>
    /// Returns a list of restaurant branches 
    /// </summary>
    /// <param name="SearchKey">User search key for restaurant name</param>
    /// <returns>
    /// Returns an array of branches that matches the Search Key (Restaurant Name). Will return complete list if no search key is input.
    /// </returns>
    /// <example>
    /// //GET: /api/BranchData/ListBranches -> [{"BranchId":"1", "Status": "Visited", "Rating": "4.00"}, {"BranchId":"2", "Status": "Not Visited", "Rating": "4.50"}]
    /// //GET: /api/BranchData/ListBranches/Miss -> [{"BranchId":"1", "RestaurantName": "Miss Lin Cafe", "Status": "Visited", "Rating": "4.00"}]
    /// </example>

    [HttpGet]
    [Route("api/BranchData/ListBranches")]
    public IEnumerable<BranchDto> ListBranches(string SearchKey = null)

    {
        IQueryable<Branch> branchesQuery = db.Branches.Include(x => x.Restaurant);

        if (!string.IsNullOrEmpty(SearchKey))
        {
            branchesQuery = branchesQuery.Where(x => x.Restaurant.RestaurantName.Contains(SearchKey));
        }

        List<BranchDto> BranchDtos = branchesQuery.Select(x => new BranchDto
        {
            BranchId = x.BranchId,
            RestaurantId = x.RestaurantId,
            Status = x.Status,
            Review = x.Review,
            Location = x.Location,
            Address = x.Address,
            RestaurantName = x.Restaurant.RestaurantName,
            RestaurantType = x.Restaurant.RestaurantType,
            Cuisine = x.Restaurant.Cuisine,
            Budget = x.Restaurant.Budget

        }).ToList();
        return BranchDtos;
    }



    /// <summary>
    /// Returns information of a branch of the restaurant
    /// </summary>
    /// <param name="id">restaurant branch id</param>
    /// <returns>
    /// singular restaurant branch of the corresponding branch id</returns>
    /// <example>
    /// //GET: /api/BranchData/Listbranches/1 -> [{"BranchtId": "1", "RestaurantId": "1"}]
    /// </example>
    [HttpGet]
    [Route("api/BranchData/FindBranch/{id}")]
    public BranchDto FindBranch(int id)
    {
        Branch Branch = db.Branches.Find(id);

        BranchDto BranchDto = new BranchDto();

        BranchDto.BranchId = Branch.BranchId;
        BranchDto.RestaurantId = Branch.RestaurantId;
        BranchDto.Status = Branch.Status;
        BranchDto.Review = Branch.Review;
        BranchDto.Location = Branch.Location;
        BranchDto.Address = Branch.Address;
        BranchDto.RestaurantName = Branch.Restaurant.RestaurantName;
        BranchDto.RestaurantType = Branch.Restaurant.RestaurantType;
        BranchDto.Cuisine = Branch.Restaurant.Cuisine;
        BranchDto.Budget = Branch.Restaurant.Budget;

        return BranchDto;

    }


    /// <summary>
    /// Updates details of an existing branch by branch id
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <param name="Branch">Branch data to be updated</param>
    /// <returns>
    /// Nothing if successful
    /// Bad Request or Not found if error is found
    /// </returns>
    // POST: api/BranchData/UpdateBranch/2
    [ResponseType(typeof(void))]
    [HttpPost]
    [Route("api/branchdata/updatebranch/{id}")]
    public IHttpActionResult UpdateBranch(int id, Branch Branch)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != Branch.BranchId)
        {

            return BadRequest();
        }

        db.Entry(Branch).State = EntityState.Modified;

        try
        {
            db.SaveChanges();
        }

        catch (DbUpdateConcurrencyException)
        {
            if (!BranchExists(id))
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


    /// <summary>
    /// To check if a branch exists
    /// </summary>
    /// <param name="id">Branch ID</param>
    private bool BranchExists(int id)
    {
        return db.Branches.Count(e => e.BranchId == id) > 0;
    }


    /// <summary>
    /// Adds branch to the database
    /// </summary>
    /// <param name="Branch">Branch data to be added to the database</param>
    /// <returns>
    /// Returns successful response if update is success.
    /// Returns Bad Request is update is invalid
    /// </returns>

    // POST: api/BranchData/AddBranch
    [ResponseType(typeof(Branch))]
    [HttpPost]
    [Route("api/branchdata/addbranch")]

    public IHttpActionResult AddBranch(Branch Branch)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        db.Branches.Add(Branch);
        db.SaveChanges();

        return Ok();
    }


    /// <summary>
    /// Deletes a branch from the system by it's ID.
    /// </summary>
    /// <param name="id">The primary key of the branch</param>
    /// <returns>
    /// HEADER: 200 (OK)
    /// or
    /// HEADER: 404 (NOT FOUND)
    /// </returns>
    /// <example>
    /// POST: api/BranchData/DeleteBranch/5
    /// FORM DATA: (empty)
    /// </example>
    [ResponseType(typeof(Branch))]
    [HttpPost]
    [Route("api/branchdata/deletebranch/{id}")]

    public IHttpActionResult DeleteBranch(int id)
    {
        Branch Branch = db.Branches.Find(id);
        if (Branch == null)
        {
            return NotFound();
        }

        db.Branches.Remove(Branch);
        db.SaveChanges();

        return Ok();
    }



    /// <summary>
    /// Gets list of branches by Restaurant ID
    /// </summary>
    /// <param name="restaurantId">Restaurant ID</param>
    /// <returns>
    /// List of restaurant branches that matches the restaurant ID
    /// </returns>
    /// <example>
    /// //GET: /api/BranchData/ListBranchesByRestaurant/1 -> [{"BranchId":"1", "RestaurantId": "1", "Status": "Visited", "Rating": "4.00"}, {"BranchId":"2", "RestaurantId": "1", "Status": "Not Visited", "Rating": "4.50"}]
    /// </example>
    //Filtering
    // GET: api/BranchData/ListBranchesByRestaurant/1
    [HttpGet]
    [Route("api/BranchData/ListBranchesByRestaurant/{restaurantId}")]
    public IEnumerable<BranchDto> ListBranchesByRestaurant(int restaurantId)
    {
        List<Branch> branches = db.Branches.Where(b => b.RestaurantId == restaurantId).ToList();
        List<BranchDto> branchDtos = new List<BranchDto>();

        foreach (Branch branch in branches)
        {
            BranchDto branchDto = new BranchDto
            {
                BranchId = branch.BranchId,
                RestaurantId = branch.RestaurantId,
                Status = branch.Status,
                Review = branch.Review,
                Location = branch.Location,
                Address = branch.Address,
                RestaurantName = branch.Restaurant.RestaurantName,
                RestaurantType = branch.Restaurant.RestaurantType,
                Cuisine = branch.Restaurant.Cuisine,
                Budget = branch.Restaurant.Budget
            };
            branchDtos.Add(branchDto);
        }

        return branchDtos;
    }

        /// <summary>
        /// Gets list of branches by Location
        /// </summary>
        /// <param name="location">Branch Location</param>
        /// <returns>
        /// List of branches that match the location
        /// </returns>
        /// <example>
        /// //GET: /api/BranchData/ListBranchesByLocation/North York -> [{"BranchId":"1", "RestaurantId": "1", "Status": "Visited", "Rating": "4.00"}, {"BranchId":"2", "RestaurantId": "1", "Status": "Not Visited", "Rating": "4.50"}]
        /// </example>
        // Filtering
        // GET: api/BranchData/ListBranchesByLocation/{location}
        [HttpGet]
        [Route("api/BranchData/ListBranchesByLocation/{location}")]
        public IEnumerable<BranchDto> ListBranchesByLocation(string location)
        {
            List<Branch> branches = db.Branches.Where(b => b.Location.Equals(location, StringComparison.OrdinalIgnoreCase)).ToList();
            List<BranchDto> branchDtos = new List<BranchDto>();

            foreach (Branch branch in branches)
            {
                BranchDto branchDto = new BranchDto
                {
                    BranchId = branch.BranchId,
                    RestaurantId = branch.RestaurantId,
                    Status = branch.Status,
                    Review = branch.Review,
                    Location = branch.Location,
                    Address = branch.Address,
                    RestaurantName = branch.Restaurant.RestaurantName,
                    RestaurantType = branch.Restaurant.RestaurantType,
                    Cuisine = branch.Restaurant.Cuisine,
                    Budget = branch.Restaurant.Budget,
                };
                branchDtos.Add(branchDto);
            }

            return branchDtos;
        }
    }
}

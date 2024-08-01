using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Eventful_Bite_App
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
            name: "RestaurantDetails",
            url: "details/{id}",
            defaults: new { controller = "Restaurant", action = "Details", id = UrlParameter.Optional }
);
        }
    }
}

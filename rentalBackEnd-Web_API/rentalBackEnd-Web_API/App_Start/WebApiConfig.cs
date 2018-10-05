using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace rentalBackEnd_Web_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
           // config.EnableCors(new EnableCorsAttribute("http://localhost:4200","*","*"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional  }
            );


            //Apply [Authorize] to all Verb Methods
            config.Filters.Add(new AuthorizeAttribute());
        }
    }
}

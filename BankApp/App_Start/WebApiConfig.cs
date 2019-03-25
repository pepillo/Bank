using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

//JDR: https://developerslogblog.wordpress.com/2016/12/30/adding-web-api-support-to-an-existing-asp-net-mvc-project/#Adding-WebAPI-support-to-our-MVC-Application

namespace Bank.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id:int}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

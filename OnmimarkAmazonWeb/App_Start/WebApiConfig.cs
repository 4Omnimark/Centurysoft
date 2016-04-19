using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ManualReview
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute("MyRoute", "api/ManualReviewAjax/{table}/{asin}/{state}", new { controller = "ManualReviewAjax", action = "GetSetValue" });

        }
    }
}

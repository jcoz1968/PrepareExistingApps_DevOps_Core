using System.Web.Http;
using System.Web.Http.Cors;

namespace FloggingApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Refs: Flogging.Core
            // NuGet: Swashbuckle, Microsoft.AspNet.WebApi.Cors, Serilog.Sinks.File
            var cors = new EnableCorsAttribute("https://samplemvc.knowyourtoolset.com, http://localhost:4200", 
                "*", "*");
            config.EnableCors(cors);


            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "logging/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

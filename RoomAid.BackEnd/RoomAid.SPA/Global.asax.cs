using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RoomAid.SPA
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

/*        protected void Application_BeginRequest()
        {
            if (Request.HttpMethod == "OPTIONS")
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                Response.AppendHeader("Access-Control-Allow-Origin", "*"); // Request.Headers.GetValues("Origin")[0]
                Response.AppendHeader("Access-Control-Allow-Headers", "Content-Type, Accept, Access-Control-Allow-Origin, Access-Control-Allow-Headers, Access-Control-Allow-Methods");
                Response.AppendHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                Response.AppendHeader("Access-Control-Allow-Credentials", "true");
                Response.End();
            }
        }*/

    }
}

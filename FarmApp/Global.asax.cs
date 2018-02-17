using FarmApp.Util;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FarmApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
			AutofacConfig.ConfigureContainer();
			AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            Response.Redirect("/Content/ExceptionFound.html");            
        }
    }
}

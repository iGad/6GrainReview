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

        //1. Я бы добавил какое-нибудь логгирование. В данном случае информация об исключении теряется
        //2. Лучше возвращать ответ со статусом отличным от 200, а не делать редирект, так пользователь не поймет что случилось и, как минимум, потеряются все введенные данные (если было заполение формы)
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            Response.Redirect("/Content/ExceptionFound.html");            
        }
    }
}

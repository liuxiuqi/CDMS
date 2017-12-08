using CDMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CDMS.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AutofacConfig.Run();
        }

        protected void Application_Error()
        {
            Exception e = HttpContext.Current.Server.GetLastError();
            if (e != null)
            {
                var log = LogFactory.GetLogger();
                log.Log("SYSTEM_ERROR", "系统错误", e.ToString());
            }
        }
    }
}

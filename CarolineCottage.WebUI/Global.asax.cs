using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace CarolineCottage.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            if (!Context.Request.IsSecureConnection
                        && !Context.Request.IsLocal // to avoid switching to https when local testing
                        )
            {
                // The 301 Moved Permanently redirect status response code is considered a best practice for upgrading users from HTTP to HTTPS (see Google recommendations)
                Response.Clear();
                Response.Status = "301 Moved Permanently";
                Response.AddHeader("Location", Context.Request.Url.ToString().Insert(4, "s"));
                Response.End();
                //  OTHERWISE use
                // Only insert an "s" to the "http:", and avoid replacing wrongly http: in the url parameters
                // Response.Redirect(Context.Request.Url.ToString().Insert(4, "s"));
            }
        }
        protected void Application_AuthenticateRequest()
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Get the stored user-data, in this case, our roles
                        string userData = ticket.UserData;
                        string[] roles = userData.Split('|');
                        HttpContext.Current.User = new GenericPrincipal(id, roles);
                    }
                }
            }
        }

    }
}

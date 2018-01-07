using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CarolineCottage.WebUI.ActionFilters
{
    public class CCAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    var roles = ticket.UserData.Split('|');
                    var identity = new GenericIdentity(ticket.Name);
                    httpContext.User = new GenericPrincipal(identity, roles);
                }
            }
            return base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //  ToDo: should this be more user friendly - or just obfuscate for unathorised access
            filterContext.Result = new HttpUnauthorizedResult();
            // OR

            //filterContext.Result = new RedirectToRouteResult(
            //                       new RouteValueDictionary 
            //                       {
            //                           { "action", "ActionName" },
            //                           { "controller", "ControllerName" }
            //                       });


        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;
using CarolineCottage.Domain;
using CarolineCottage.WebUI.ActionFilters;

namespace CarolineCottage.WebUI.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {

            UserView userView = new UserView();

            return View(userView);
        }

        [HttpPost]
        public ActionResult Login(UserView userView)
        {
            if (userView.ValidateLogin(ConfigurationManager.ConnectionStrings["CCConnectionString"].ConnectionString))
            {
                SetAuthentication(userView);
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [CCAuthorize]
        [HttpPost]
        public ActionResult AdminView()
        {
            // load booking view
            List<BookingView> bookings = new List<BookingView>();
            bookings = BookingView.GetCurrentBookings(ConfigurationManager.ConnectionStrings["CCConnectionString"].ConnectionString, true, DateTime.MaxValue);

            return PartialView("AdminHome", bookings);
        }

        [CCAuthorize]
        [HttpPost]
        public ActionResult TransferToTextFile()
        {
            //var fileContents = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/file.txt"));
            var filePath = HostingEnvironment.MapPath(@"~/App_Data");
            return Json(new { success = true });
        }
        public ActionResult LoadUserList()
        {
            return Json(UserView.GetUserList(ConfigurationManager.ConnectionStrings["CCConnectionString"].ConnectionString));
        }

        public ActionResult LoadUserDetails(int userID)
        {
            UserView userView = UserView.GetUserByID(userID, ConfigurationManager.ConnectionStrings["CCConnectionString"].ConnectionString);
            return PartialView("EditUserDetails", userView);
        }

        [CCAuthorize]
        [HttpPost]
        public ActionResult EditAddUser(UserView userView)
        {
            if (userView.Save(ConfigurationManager.ConnectionStrings["CCConnectionString"].ConnectionString))
            {
                return Json(new { saved = true });
            }
            return Json( new {saved= false});
        }

        [CCAuthorize]
        [HttpPost]
        public ActionResult GetBookingRow(string bookingLineID)
        {
            int bookingID = Convert.ToInt32(bookingLineID.Substring(bookingLineID.LastIndexOf("WeekID_") + "WeekID".Length + 1));
            BookingView bookingRow = BookingView.GetBookingByID(bookingID, ConfigurationManager.ConnectionStrings["CCConnectionString"].ConnectionString);
            return PartialView("EditBooking", bookingRow);
        }

        public ActionResult SaveBooking(BookingView bookingView)
        {
            bookingView.Save(ConfigurationManager.ConnectionStrings["CCConnectionString"].ConnectionString);

            // check for save error
            if (bookingView.BookingID == 0)
            {
                return null;
            }
            return PartialView("BookingRow", bookingView);
        }

        public ActionResult CopyCurrentBookingLineToNext(string currentLineID)
        {
            int bookingID = Convert.ToInt32(currentLineID.Substring(currentLineID.LastIndexOf("WeekID_") + "WeekID".Length + 1));

            BookingView bookingView = BookingView.CopyLineToNextLine(bookingID, ConfigurationManager.ConnectionStrings["CCConnectionString"].ConnectionString);
            if(bookingView.BookingID == 0)
            {
                return null;
            }
            return PartialView("BookingRow", bookingView);
        }

        #region "helper function"

        private void SetAuthentication(UserView userView)
        {
            FormsAuthentication.SetAuthCookie(userView.Name, false);
            //  set ticket details
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,  //  ticket details
                userView.Name, // user for this ticket
                DateTime.Now,   // date created
                DateTime.Now.AddMinutes(30),    // date/time expires
                false,  // don't persist
                //String.Join("|", userView.Profile.Roles.Select(m => m.RoleCode).ToArray())   // add user data - roles                        
                "admin"
                );

            //  encrypt ticket and add as cookie
            string hash = FormsAuthentication.Encrypt(ticket);

            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash)
            {
                // IIRC this property is only available in .NET 4.0,
                // so you might need a constant here to match the domain property
                // in the <forms> tag of the web.config
                Domain = FormsAuthentication.CookieDomain,
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
            };

            Response.AppendCookie(authCookie);
        }

        #endregion
    }
}
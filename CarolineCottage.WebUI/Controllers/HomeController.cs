using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using CarolineCottage.Domain;
using System.Configuration;
using System.Web.Configuration;
using CarolineCottage.WebUI.Application;

namespace CC.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewData["Message"] = TempData["Message"];
            }

            string path = Server.MapPath("~/Content/ImagesCarousel");
            CarolineCottageService.CarouselDisplay carouselDisplay = new CarolineCottageService.CarouselDisplay();
            carouselDisplay.ImagePath = "~/Content/ImagesCarousel";
            carouselDisplay.GetImageDisplayList(path);

            return View(carouselDisplay);
        }

        public ActionResult Cottage()
        {
            string path = Server.MapPath("~/Content/ImagesCarouselCottage");
            CarolineCottageService.CarouselDisplay carouselDisplay = new CarolineCottageService.CarouselDisplay();
            carouselDisplay.ImagePath = "~/Content/ImagesCarouselCottage";
            carouselDisplay.GetImageDisplayList(path);
            return View(carouselDisplay);
        }

        public ActionResult Mousehole()
        {
            return View();
        }


        public ActionResult Visit()
        {
            return View();
        }

        public ActionResult Guest()
        {
            return View();
        }

        public ActionResult Download()
        {
            return View();
        }

        public ActionResult Video()
        {
            return View();
        }

        public ActionResult ContactCC()
        {
            var cvm = new ContactViewModel();
            return View(cvm);
        }

        public ActionResult Availability()
        {
            string endDateForDisplay = WebConfigurationManager.AppSettings["EndDateForDisplay"];
            DateTime endDate = Convert.ToDateTime(endDateForDisplay);
            bool debugSQLConnection = Convert.ToBoolean(WebConfigurationManager.AppSettings["DebugSQLConnection"]);
            // load booking view
            BookingViewReturn bookings = BookingView.GetCurrentBookings(ConfigurationManager.ConnectionStrings["CCConnectionString"].ConnectionString, false, endDate, debugSQLConnection);
            return View(bookings);
        }

        [HttpPost]
        public ActionResult ContactCC(ContactViewModel contactVM)
        {

            if (!ModelState.IsValid)
            {
                return View(contactVM);
            }

            var contact = new Contact
            {
                From = contactVM.From,
                Subject = contactVM.Subject,
                Message = contactVM.Message
            };

            var message = new Email().Send(contact);
            message = CheckMessageErrorState(message);
            return RedirectToAction("index", "Home");
        }

        
        [HttpPost]
        public ActionResult PrivacyStatement()
        {
            return PartialView("PrivacyStatement");
        }

        [HttpPost]
        public ActionResult Enquiry(ContactViewModel contactView)
        {
            Contact contact = new Contact
            {
                From = contactView.From,
                Subject = "Enquiry for week " + contactView.WeekDate,
                Message = contactView.Message
            };
            var message = new Email().Send(contact);
            message = CheckMessageErrorState(message);

            if (message != string.Empty)
            {
                TempData["Message"] = null;
                return Json(new { response = false });
            }

            contact.To = contact.From;
            contact.From = "caroline@pugwash.com";
            contact.Subject = "Confirmation of your enquiry for week  " + contactView.WeekDate;
            contact.Message = "This is a confirmation that you have contacted us and does not need a reply. You can also contact us on 01865 727423";
            new Email().Send(contact);

            return Json(new { response = true });
        }

        #region "helper functions"
        private string CheckMessageErrorState(string message)
        {
            if (message != string.Empty)
            {
                if (!Convert.ToBoolean(WebConfigurationManager.AppSettings["DebugSQLConnection"]))
                {
                    message = "There was an error sending your message: please call us on 01865 727423";
                }

                TempData["Message"] = message;
            }

            return message;
        }

        #endregion

        #region "email"
        public class ContactViewModel
        {
            [Required(ErrorMessage = "Your email address is required")]
            [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
            [Display(Name = "Your email address")]
            public string From { get; set; }

            [Required]
            [Display(Name = "Subject")]
            public string Subject { get; set; }

            [Required]
            [Display(Name = "Message")]
            public string Message { get; set; }

            public int BookingWeekID { get; set; }
            public string WeekDate { get; set; }
        }

        public class Contact
        {
            public string From { get; set; }
            public string To { get; set; }
            public string Subject { get; set; }

            public string Message { get; set; }

            public Contact()
            {
                To = null;
            }
        }

        public class Email
        {
            public string Send(Contact contact)
            {
                string returnMessage = "";
                MailMessage mail = new MailMessage(
                    "caroline@pugwash.com",
                    contact.To ?? "caroline@pugwash.com",
                    contact.Subject,
                    contact.Message + "\n\nFrom\n\n" + contact.From);
                mail.ReplyToList.Add(contact.From);

                try
                {
                    using (SmtpClient smtpClient = new SmtpClient("SMTP.Livemail.co.uk"))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential("caroline@pugwash.com", "ARD1329");
                        smtpClient.Port = 587;
                        smtpClient.Send(mail);
                    }
                }
                catch (Exception e)
                {

                    returnMessage = $"Error sending: 1: {e.Message} 2: {e.InnerException.Message}";
                }

                return returnMessage;
            }
        }
        #endregion

    }
}

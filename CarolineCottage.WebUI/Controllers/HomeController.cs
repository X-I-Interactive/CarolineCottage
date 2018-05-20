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

namespace CC.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Information()
        {
            return View();
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

            // load booking view
            List<BookingView> bookings = BookingView.GetCurrentBookings(ConfigurationManager.ConnectionStrings["CCConnectionString"].ConnectionString, false, endDate);            
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

            new Email().Send(contact);

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
            new Email().Send(contact);

            contact.To = contact.From;
            contact.Subject = "Enquiry confirmation for week  " + contactView.WeekDate;
            new Email().Send(contact);

            return Json(new { response = true });
        }

        #region "helper functions"


        #endregion

        #region "email"
        public class ContactViewModel
        {
            [Required (ErrorMessage ="Your email address is required")]
            [DataType(DataType.EmailAddress, ErrorMessage ="Please enter a valid email address")]
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
            public void Send(Contact contact)
            {
                MailMessage mail = new MailMessage(
                    "caroline@pugwash.com",
                    contact.To ?? "caroline@pugwash.com",
                    contact.Subject,
                    contact.Message + "\n\nFrom\n\n" + contact.From);
                mail.ReplyToList.Add(contact.From);             

                using (SmtpClient smtpClient = new SmtpClient("smtp.pugwash.com"))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("caroline@pugwash.com", "ARD1329");
                    smtpClient.Send(mail);
                }
            }            
        }
        #endregion

    }
}

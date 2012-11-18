using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using SendGridMail;
using SendGridMail.Transport;
using TfsManualTester.Web.Models;

namespace TfsManualTester.Web.Controllers
{
    public class ErrorReportingController : Controller
    {
        [HttpPost]
        public ActionResult Report(string errorReport)
        {
            SendEmail(errorReport);
            StoreInRaven(errorReport);

            return Json("Successful email.");
        }

        private static void SendEmail(string errorReport)
        {
            var message = SendGrid.GenerateInstance();
            message.From = new MailAddress("ryan.aidan@gmail.com");
            message.AddTo("ryan.aidan@gmail.com");
            message.Subject = "TFS Test Steps Editor Error";
            message.Text = errorReport;

            var username = ConfigurationManager.AppSettings["SENDGRID_USERNAME"];
            var password = ConfigurationManager.AppSettings["SENDGRID_PASSWORD"];
            var restTransport = REST.GetInstance(new NetworkCredential(username, password));

            restTransport.Deliver(message);
        }

        private static void StoreInRaven(string errorReportText)
        {
            var errorReport = new ErrorReport
            {
                ReportedDateTime = DateTime.UtcNow,
                Source = "HTTP Post",
                Text = errorReportText
            };

            using (var session = MvcApplication.Store.OpenSession())
            {
                session.Store(errorReport);
                session.SaveChanges();
            }
        }
    }
}

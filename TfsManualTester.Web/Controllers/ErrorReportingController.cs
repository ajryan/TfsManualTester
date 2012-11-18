using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using SendGridMail;
using SendGridMail.Transport;

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

            var username = ConfigurationManager.AppSettings["SendGrid_UserName"];
            var password = ConfigurationManager.AppSettings["SendGrid_Password"];
            var restTransport = REST.GetInstance(new NetworkCredential(username, password));

            restTransport.Deliver(message);
        }

        private static void StoreInRaven(string errorReport)
        {
            
        }
    }
}

using System;
using System.Net;
using System.Web.Mvc;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Client;

namespace TfsManualTester.Web.Controllers
{
    public class AccessController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult TfsLogin(string tfsUrl, string userName, string password)
        {
            bool success = true;
            string errorMessage = null;

            try
            {
                var tfs = new TfsTeamProjectCollection(
                    new Uri(tfsUrl),
                    new NetworkCredential(userName, password));
                
                tfs.EnsureAuthenticated();
            }
            catch (Exception authEx)
            {
                success = false;
                errorMessage = authEx.GetType().Name + ": " + authEx.Message;
            }

            return Json(
                new
                {
                    Success = success,
                    ErrorMessage = errorMessage,
                    TfsUrl = tfsUrl,
                    UserName = userName,
                    Password = password
                });
        }
    }
}

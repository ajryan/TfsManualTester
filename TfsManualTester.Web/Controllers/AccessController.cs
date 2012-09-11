using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TfsManualTester.Web.Authorization;
using TfsManualTester.Web.Tfs;

namespace TfsManualTester.Web.Controllers
{
    public class AccessController : Controller
    {
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public JsonResult TfsLogin(string tfsUrl, string serviceIdUsername, string serviceIdPassword)
        {
            bool success = true;
            string errorMessage = null;

            try
            {
                new TeamProject(new Uri(tfsUrl), serviceIdUsername, serviceIdPassword).EnsureAuthenticated();

                var userData = new TfsUserData {Password = serviceIdPassword, TfsUrl = tfsUrl};

                var authTicket = new FormsAuthenticationTicket(
                    2,
                    serviceIdUsername,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                    false,
                    userData.Serialize());

                var authCookie = new HttpCookie(
                    FormsAuthentication.FormsCookieName,
                    FormsAuthentication.Encrypt(authTicket))
                {
                    HttpOnly = true
                };
                Response.AppendCookie(authCookie);
            }
            catch (Exception authEx)
            {
                success = false;
                errorMessage = authEx.GetType().Name + ": " + authEx.Message + Environment.NewLine + authEx.ToString();
            }

            return Json(
                new
                {
                    Success = success,
                    ErrorMessage = errorMessage,
                    RedirectUrl = Url.Action("Edit","Test")
                });
        }
    }
}

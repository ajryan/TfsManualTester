using System;
using System.Web.Mvc;
using TfsManualTester.Web.Authorization;
using TfsManualTester.Web.Tfs;

namespace TfsManualTester.Web.Controllers
{
    public class AccessController : Controller
    {
        public ActionResult Login(string returnUrl)
        {
            var authorizedPrincipal = UserDataPrincipal.InitFromAuthCookie(HttpContext);
            if (authorizedPrincipal != null)
            {
                ViewBag.Principal = authorizedPrincipal;
            }
            
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
                new TeamProject(new Uri(tfsUrl), serviceIdUsername, serviceIdPassword)
                    .EnsureAuthenticated(false);

                var userData = new TfsUserData {Password = serviceIdPassword, TfsUrl = tfsUrl};
                var authCookie = UserDataPrincipal.CreateAuthCookie(serviceIdUsername, userData);
                Response.AppendCookie(authCookie);
            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.GetType().Name + ": " + ex.Message + Environment.NewLine + ex.ToString();
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

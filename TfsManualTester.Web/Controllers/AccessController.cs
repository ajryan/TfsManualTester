using System;
using System.Net;
using System.Web.Mvc;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.TestManagement.Client;

namespace TfsManualTester.Web.Controllers
{
    public class AccessController : Controller
    {
        public ActionResult Login()
        {
            ViewBag.LiveIdAppId = "00000000440D0A0B"; // TODO: move to appSettings
            ViewBag.LiveIdRedirect = "https://tfsmanualtester.azurewebsites.net/";

            return View();
        }

        [HttpPost]
        public JsonResult TfsLogin(string tfsUrl, string userName, string password, string domain)
        {
            bool success = true;
            string errorMessage = null;
            int planCount = 0;

            try
            {
                var tfs = new TfsTeamProjectCollection(
                    new Uri(tfsUrl),
                    new NetworkCredential(userName, password, domain));
                
                tfs.EnsureAuthenticated();

                var testManagement = tfs.GetService<ITestManagementService>();
                var testProject = testManagement.GetTeamProject("CMMI_Scratch");
                var testPlans = testProject.TestPlans.Query("SELECT * FROM TestPlan");
                planCount = testPlans.Count;
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
                    TfsUrl = tfsUrl,
                    UserName = userName,
                    Password = password,
                    Domain = domain,
                    PlanCount = planCount
                });
        }
    }
}

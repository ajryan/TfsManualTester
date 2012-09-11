using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TfsManualTester.Web.Authorization;
using TfsManualTester.Web.Tfs;

namespace TfsManualTester.Web.Controllers
{
    public class TestController : Controller
    {
        [UserDataAuthorize]
        public ActionResult Edit()
        {
            // TODO: split into two models - test load vs actual test

            var testCase = new TestCase("Unknown project");
            //testCase.TestSteps.Add(new TestStep { Title = "title", Expected = "expected" });
            ViewBag.TestJson = new JavaScriptSerializer().Serialize(testCase);
            return View();
        }

        [HttpPost]
        [UserDataAuthorize]
        public ActionResult Load(string teamProject, int testCaseId)
        {
            try
            {
                var user = (UserDataPrincipal) HttpContext.User;

                var tfs = new TeamProject(new Uri(user.TfsUrl), user.UserName, user.Password);
                var tfsTestCase = tfs.GetTestCase(teamProject, testCaseId);
                var testCase = new TestCase(tfsTestCase);

                return Json(testCase);
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        ErrorMessage = ex.ToString(),
                        Success = false
                    });
            }
        }
    }
}

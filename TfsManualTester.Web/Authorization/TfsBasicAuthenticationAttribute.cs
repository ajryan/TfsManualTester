using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.TeamFoundation;
using TfsManualTester.Web.Tfs;

namespace TfsManualTester.Web.Authorization
{
    public class TfsBasicAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var userDataPrincipal = UserDataPrincipal.InitFromHeaders(actionContext.Request.Headers);

            if (userDataPrincipal == null)
            {
                SetUnauthorizedResponse(actionContext);
                return;
            }

            try
            {
                var tfsProject = new TeamProject(
                    new Uri(userDataPrincipal.TfsUrl),
                    userDataPrincipal.UserName,
                    userDataPrincipal.Password);
                
                tfsProject.EnsureAuthenticated(false);
            }
            catch (Exception ex)
            {
                if (ex is UriFormatException || ex is WebException || ex is TeamFoundationServiceUnavailableException)
                {
                    SetUnauthorizedResponse(actionContext, ex.Message);
                    return;
                }
                throw;
            }

            HttpContext.Current.User = userDataPrincipal;

            base.OnActionExecuting(actionContext);
        }

        private static void SetUnauthorizedResponse(HttpActionContext actionContext, string message = null)
        {
            string messageValue = message ?? "Authorization (Basic) and TfsUrl headers are required.";
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { Message = messageValue });
        }
    }
}
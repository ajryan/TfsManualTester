using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Security;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Client;

namespace TfsManualTester.Web.Authorization
{
    public class TfsBasicAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var userDataPrincipal = HttpContext.Current.User as UserDataPrincipal;
            if (userDataPrincipal == null)
            {
                userDataPrincipal = UserDataPrincipal.InitFromHeaders(actionContext.Request.Headers);
            }
            if (userDataPrincipal == null)
            {
                userDataPrincipal = UserDataPrincipal.InitFromAuthCookie(actionContext.Request.Headers);
            }

            if (userDataPrincipal == null)
            {
                SetUnauthorizedResponse(actionContext);
                return;
            }

            try
            {
                var configUri = new Uri(userDataPrincipal.TfsUrl);

                var provider = userDataPrincipal.GetCredentialsProvider();
                var tfsConfigServer = new TfsConfigurationServer(configUri, provider.GetCredentials(null, null), provider);

                tfsConfigServer.EnsureAuthenticated();

                HttpContext.Current.Items["TFS_CONFIG_SERVER"] = tfsConfigServer;
            }
            catch (Exception ex)
            {
                if (ex is UriFormatException || ex is WebException || ex is TeamFoundationServerException)
                {
                    SetUnauthorizedResponse(actionContext, ex.Message);
                    return;
                }
                throw;
            }

            HttpContext.Current.User = userDataPrincipal;

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var currentUser = HttpContext.Current.User as UserDataPrincipal;
            if (currentUser != null && actionExecutedContext.Response != null)
            {
                var authCookie = currentUser.CreateAuthCookie();
                var cookieHeaderValue = new CookieHeaderValue(authCookie.Name, authCookie.Value);
                actionExecutedContext.Response.Headers.AddCookies(new[] { cookieHeaderValue });
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        private void SetUnauthorizedResponse(HttpActionContext actionContext, string message = null)
        {
            string messageValue = message ?? "Authorization (Basic) and TfsUrl headers are required.";
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { Message = messageValue });

            string tfsUrl = actionContext.Request.Headers.GetTfsUrl();
            if (!String.IsNullOrWhiteSpace(tfsUrl))
            {
                actionContext.Response.Headers.Add(
                    "WWW-Authenticate", 
                    String.Format("Basic realm=\"{0}\"", tfsUrl));
            }
        }
    }
}
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace TfsManualTester.Web.Authorization
{
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Class,
        Inherited = true,
        AllowMultiple = true)]
    public class UserDataAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthenticated = base.AuthorizeCore(httpContext);
            if (!isAuthenticated)
                return false;

            string authCookieName = FormsAuthentication.FormsCookieName;
            
            if (!httpContext.User.Identity.IsAuthenticated ||
                httpContext.Request.Cookies == null ||
                httpContext.Request.Cookies[authCookieName] == null)
            {
                if (httpContext.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    httpContext.Response.Write(new JavaScriptSerializer().Serialize(new { ErrorMessage = "Not authorized", Success = false }));
                    return true;
                }
                else
                {
                    return false;
                }
            }

            var authCookie = httpContext.Request.Cookies[authCookieName];
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            var userData = TfsUserData.DeSerialize(authTicket.UserData);

            var principal = new UserDataPrincipal
            {
                UserName = authTicket.Name,
                Password = userData.Password,
                TfsUrl = userData.TfsUrl
            };

            httpContext.User = principal;
            return true;
        }
    }
}
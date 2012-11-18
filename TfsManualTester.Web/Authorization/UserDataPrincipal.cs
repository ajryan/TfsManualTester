using System;
using System.Security.Principal;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace TfsManualTester.Web.Authorization
{
    public class UserDataPrincipal : IPrincipal
    {
        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IIdentity Identity { get; private set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string TfsUrl { get; set; }

        public static HttpCookie CreateAuthCookie(string serviceIdUsername, TfsUserData userData)
        {
            // TODO: inject "remember me" checkbox from form
            var authTicket = new FormsAuthenticationTicket(
                    2,
                    serviceIdUsername,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                    true,
                    userData.Serialize());

            var authCookie = new HttpCookie(
                FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(authTicket))
            {
                HttpOnly = true
            };

            return authCookie;
        }

        public static UserDataPrincipal InitFromAuthCookie(HttpContextBase httpContext)
        {
            string authCookieName = FormsAuthentication.FormsCookieName;

            if (!httpContext.User.Identity.IsAuthenticated ||
                httpContext.Request.Cookies == null ||
                httpContext.Request.Cookies[authCookieName] == null)
            {
                return null;
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

            return principal;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;

namespace TfsManualTester.Web.Authorization
{
    public class UserDataPrincipal : IPrincipal
    {
        public string TfsUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public static UserDataPrincipal Current
        {
            get { return HttpContext.Current.User as UserDataPrincipal; }
        }

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

        public static UserDataPrincipal InitFromHeaders(HttpRequestHeaders headers)
        {
            var authHeader = headers.Authorization;
            IEnumerable<string> tfsUrlValues;
            if (authHeader == null || 
                !authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) ||
                !headers.TryGetValues("TfsUrl", out tfsUrlValues))
            {
                return null;
            }

            string tfsUrl = tfsUrlValues.FirstOrDefault();
            
            string userName = null;
            string password = null;

            string decodedAuthHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader.Parameter));
            var usernamePasswordTokens = decodedAuthHeader.Split(new[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (usernamePasswordTokens.Length >= 2)
            {
                userName = usernamePasswordTokens[0];
                password = usernamePasswordTokens[1];
            }

            if (String.IsNullOrWhiteSpace(tfsUrl) || String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(password))
                return null;

            return new UserDataPrincipal
            {
                UserName = userName,
                Password = password,
                TfsUrl = tfsUrl
            };
        }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}
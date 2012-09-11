using System;
using System.Security.Principal;

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
    }
}
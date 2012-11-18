using System;
using System.Web.Mvc;

namespace TfsManualTester.Web.Authorization
{

    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Class,
        Inherited = true,
        AllowMultiple = true)]
    public class UserDataAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            // if we get an authorized principal, good to go
            var principal = UserDataPrincipal.InitFromAuthCookie(filterContext.HttpContext);
            if (principal != null)
            {
                filterContext.HttpContext.User = principal;
                return;
            }

            // not authorized - special case for HTTP POST
            if (filterContext.HttpContext.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                filterContext.Result = new JsonResult {Data = new {ErrorMessage = "Not authorized", Success = false}};
            }
        }
    }
}
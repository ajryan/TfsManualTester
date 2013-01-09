using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using TfsManualTester.Web.Authorization;

namespace TfsManualTester.Web.Controllers
{
    [TfsBasicAuthentication]
    public class WorkItemsController : ApiController
    {
        // GET api/workitems
        public IEnumerable<string> Get()
        {
            var user = UserDataPrincipal.Current;
            return new string[] { user.TfsUrl, user.UserName, user.Password };
        }
    }
}

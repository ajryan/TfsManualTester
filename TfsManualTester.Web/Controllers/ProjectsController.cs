using System;
using System.Collections.Generic;
using System.Web.Http;
using TfsManualTester.Web.Authorization;

namespace TfsManualTester.Web.Controllers
{
    [TfsBasicAuthentication]
    public class ProjectsController : ApiController
    {
        // GET api/projects
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}

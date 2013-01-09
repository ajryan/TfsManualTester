using System;
using System.Collections.Generic;
using System.Web.Http;

namespace TfsManualTester.Web.Controllers
{
    public class ProjectsController : ApiController
    {
        // GET api/projects
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}

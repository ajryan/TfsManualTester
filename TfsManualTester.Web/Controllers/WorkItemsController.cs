using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Newtonsoft.Json.Linq;
using TfsManualTester.Web.Authorization;
using TfsManualTester.Web.Models;

namespace TfsManualTester.Web.Controllers
{
    [TfsBasicAuthentication]
    public class WorkItemsController : ApiController
    {
        // GET api/workitems
        public IEnumerable<WorkItemInfo> Get(string collectionId, string projectName)
        {
            if (String.IsNullOrWhiteSpace(collectionId) || String.IsNullOrWhiteSpace(projectName))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound) { Content = new StringContent("collectionUri and projectName are required") });
            }

            var user = UserDataPrincipal.Current;

            var configServer = (TfsConfigurationServer)HttpContext.Current.Items["TFS_CONFIG_SERVER"];
            var collection = configServer.GetTeamProjectCollection(new Guid(collectionId));

            var workItemStore = collection.GetService<WorkItemStore>();
            var query = workItemStore.Query(String.Format("Select [Title] From WorkItems Where [System.TeamProject] = '{0}'", projectName));

            return query
                .Cast<WorkItem>()
                .Take(10)
                .Select(
                    wi => new WorkItemInfo
                    {
                        Id = wi.Id,
                        Title = wi.Title,
                        Description = wi.Description,
                        AssignedTo = wi.Fields["System.AssignedTo"].Value.ToString(),
                        WorkItemType = wi.Type.Name
                    });
        }
    }
    
}

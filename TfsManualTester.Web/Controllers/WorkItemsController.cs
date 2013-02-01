using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsManualTester.Web.Authorization;
using TfsManualTester.Web.Models;

namespace TfsManualTester.Web.Controllers
{
    [TfsBasicAuthentication]
    public class WorkItemsController : ApiController
    {
        // GET api/workitems
        public IEnumerable<WorkItemInfo> Get(string collectionId, string projectName, int page, string workItemType)
        {
            if (String.IsNullOrWhiteSpace(collectionId) || String.IsNullOrWhiteSpace(projectName))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound) { Content = new StringContent("collectionUri and projectName are required") });
            }

            var configServer = (TfsConfigurationServer)HttpContext.Current.Items["TFS_CONFIG_SERVER"];
            var collection = configServer.GetTeamProjectCollection(new Guid(collectionId));

            var workItemStore = collection.GetService<WorkItemStore>();
            var wiql = String.Format("Select * From WorkItems Where [System.TeamProject] = '{0}'", projectName);
            if (!String.IsNullOrWhiteSpace(workItemType) &&
                !workItemType.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                wiql += String.Format(" AND [System.WorkItemType] = '{0}'", workItemType);
            }
            var query = workItemStore.Query(wiql);

            return query
                .Cast<WorkItem>()
                .Skip(10 * page)
                .Take(10)
                .Select(
                    wi => new WorkItemInfo
                    {
                        WorkItemType = wi.Type.Name,
                        Id = wi.Id,
                        Title = wi.Title,
                        AssignedTo = wi.Fields["System.AssignedTo"].Value.ToString(),
                        State = wi.State,
                        Reason = wi.Reason,
                        Area = wi.AreaPath,
                        Iteration = wi.IterationPath,
                        History = GetHistory(wi),
                        CreatedDate = wi.CreatedDate,
                        ChangedDate = wi.ChangedDate,
                        Description = GetDescription(wi),
                        OtherFields = GetOtherFields(wi)
                    });
        }

        private static readonly string[] _ExcludeOtherFields =
        {
            "Title", "State", "Reason", "Assigned To", "Work Item Type", "Description", 
            "HTMLDescription", "DescriptionHTML", "History", "Iteration Path", "Iteration ID", "Team Project",
            "Node Name", "Area Path", "Changed Date", "ID", "Area ID"
        };
        private List<WorkItemFieldInfo> GetOtherFields(WorkItem workItem)
        {
            return workItem.Fields
                .Cast<Field>()
                .Where(f => !_ExcludeOtherFields.Contains(f.Name))
                .Select(
                    field => new WorkItemFieldInfo
                    {
                        Name = field.Name, 
                        Value = (field.Value != null)? field.Value.ToString() : ""
                    })
                .Where(fi => !String.IsNullOrWhiteSpace(fi.Value) && fi.Value != "0")
                .OrderBy(fi => fi.Name)
                .ToList();
        }

        private static readonly string[] _DescriptionFields = { "HTMLDescription", "DescriptionHTML", "Description" };
        private static string GetDescription(WorkItem workItem)
        {
            foreach (string field in _DescriptionFields)
            {
                if (workItem.Fields.Contains(field))
                {
                    string description = workItem.Fields[field].Value.ToString();
                    if (!String.IsNullOrWhiteSpace(description))
                        return description;
                }
            }
            return workItem.Description;
        }

        private static string GetHistory(WorkItem workItem)
        {
            var historyBuilder = new StringBuilder();
            foreach (var rev in workItem.Revisions.Cast<Revision>().OrderByDescending(r => r.Index))
            {
                historyBuilder.AppendFormat("{0} {1}\r\n", rev.Fields["Changed Date"].Value, rev.GetTagLine());
                historyBuilder.AppendLine(rev.Fields["History"].Value.ToString());
                historyBuilder.AppendLine();
                    
            }
            return historyBuilder.ToString();
        }
    }
}

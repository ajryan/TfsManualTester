using System;

namespace TfsManualTester.Web.Models
{
    public class WorkItemInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
        public string WorkItemType { get; set; }
    }
}
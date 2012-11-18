using System;

namespace TfsManualTester.Web.Models
{
    public class ErrorReport
    {
        public DateTime ReportedDateTime { get; set; }
        public string Source { get; set; }
        public string Text { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace TfsManualTester.Web.Authorization
{
    public static class Extensions
    {
        public static string GetTfsUrl(this HttpRequestHeaders headers)
        {
            IEnumerable<string> tfsUrlValues;
            if (headers.TryGetValues("TfsUrl", out tfsUrlValues))
                return tfsUrlValues.FirstOrDefault();
            return null;
        }
    }
}
using System;
using Microsoft.TeamFoundation.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TfsManualTester.Test
{
    [TestClass]
    public class TfsAuthTest
    {
        [TestMethod]
        public void OAuthTest()
        {
            var cred =
                new OAuthTokenCredential(
                    "EwAwAq1DBAAUlbRWyAJjK5w968Ru3Cyt/6GvwXwAATeIbV0Y0OeGrphv8z2n1CpLnlouty+RFlI/QIWAUmz6amQFLa5dYbDdJ/p0Lw9KIeQii/J+t4NNtthhOCOtNRTXY9VD9FQyPSI62mW0kdkPCFhopHl4BEzG2I+qFJxd/K0p/D341M/C/CObdzlQVzKdwGLpRvgi+i2lAmuYkUKwpi4pHdYSbakhNe/MHHRg4UPxj50R9LM9zwZVPNO+SF1JmRvTZShDrPcE1kI8NbB6w8YTqtFjy+xIz1sgIHavldgeVSJRGp5t5ktjm1wzP0kB/twt/poRt8TtjF6ay82PIgjU9BGq2PRjFbC9noVItqOYjhbiEC1W6JnIe6ygzA4DZgAACF5JlNgI2yGAAAF9sa8SNnmO8xbPk1GkP+SHELIZn0mjhZ3amV6JRSHOuFJEBkk4VIRyQtwGGGIqhqhNYi3ypNsQ77AcjCitZNA3ZExJUNtUlktfhi1GOduB8Od9cf32HRxsAUFr+Q7hDtIw7wgPR69zI087RMBAnl9PWHcVvL31ax19MAJcbmXT0cXsyepE4IV8JiH/1SBFwHP9kbOx0zt1TppZVjsL/P03psJ5rHrbx8uTCbh30XaXE3SCe1+VMFzuUGzczJydw53djrjokhZ/6plni8fw6b7qpECtY5PNV+XEbCqh+7a9y5Ylo8SxqRMXcF6ZwHOiBtGsw9+OHYxoY8n/DjmypenzAAA=");
            //var cred =
            //    new OAuthTokenCredential(
            //    new Uri("https://tfsmanualtester.azurewebsites.net/"),
            //            //new Uri("https://oauth.live.com/authorize"),
            //            "00000000440D0A0B",
            //            "5H1-ldM6zS98mFBUviVB7u3Yyk9b3gRi",
            //            "eyJhbGciOiJIUzI1NiIsImtpZCI6IjAiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTM0NTU2NzAwMSwidWlkIjoiODIyZTk1YjEwOTE0YzBkNzdkZDNkNzg4YTg0MDk3MmMiLCJhdWQiOiJ0ZnNtYW51YWx0ZXN0ZXIuYXp1cmV3ZWJzaXRlcy5uZXQiLCJ1cm46bWljcm9zb2Z0OmFwcHVyaSI6ImFwcGlkOi8vMDAwMDAwMDA0NDBEMEEwQiIsInVybjptaWNyb3NvZnQ6YXBwaWQiOiIwMDAwMDAwMDQ0MEQwQTBCIn0.bqeYZlowg88X747AVCy4eUvoPeO_aNBuUGl1rTcxCUo");

            var tfs = new TfsTeamProjectCollection(
                    new Uri("https://ajryan.tfspreview.com/DefaultCollection"),
                    new TfsClientCredentials(cred));
            tfs.EnsureAuthenticated();
        }

        [TestMethod]
        public void UiProviderTest()
        {
            var picker = new TeamProjectPicker(TeamProjectPickerMode.NoProject, false);
            picker.ShowDialog();
        }

        [TestMethod]
        public void AutoCredentialsTest()
        {
            var tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri("https://ajryan.tfspreview.com"));
            tfs.EnsureAuthenticated();
        }
    }
}

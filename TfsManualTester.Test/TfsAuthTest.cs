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
            //var cred =
            //    new OAuthTokenCredential(
            //        "eyJhbGciOiJIUzI1NiIsImtpZCI6IjAiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTM0NTU2NzAwMSwidWlkIjoiODIyZTk1YjEwOTE0YzBkNzdkZDNkNzg4YTg0MDk3MmMiLCJhdWQiOiJ0ZnNtYW51YWx0ZXN0ZXIuYXp1cmV3ZWJzaXRlcy5uZXQiLCJ1cm46bWljcm9zb2Z0OmFwcHVyaSI6ImFwcGlkOi8vMDAwMDAwMDA0NDBEMEEwQiIsInVybjptaWNyb3NvZnQ6YXBwaWQiOiIwMDAwMDAwMDQ0MEQwQTBCIn0.bqeYZlowg88X747AVCy4eUvoPeO_aNBuUGl1rTcxCUo");
            var cred =
                new OAuthTokenCredential(
                new Uri("https://tfsmanualtester.azurewebsites.net/"),
                        //new Uri("https://oauth.live.com/authorize"),
                        "00000000440D0A0B",
                        "5H1-ldM6zS98mFBUviVB7u3Yyk9b3gRi",
                        "eyJhbGciOiJIUzI1NiIsImtpZCI6IjAiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTM0NTU2NzAwMSwidWlkIjoiODIyZTk1YjEwOTE0YzBkNzdkZDNkNzg4YTg0MDk3MmMiLCJhdWQiOiJ0ZnNtYW51YWx0ZXN0ZXIuYXp1cmV3ZWJzaXRlcy5uZXQiLCJ1cm46bWljcm9zb2Z0OmFwcHVyaSI6ImFwcGlkOi8vMDAwMDAwMDA0NDBEMEEwQiIsInVybjptaWNyb3NvZnQ6YXBwaWQiOiIwMDAwMDAwMDQ0MEQwQTBCIn0.bqeYZlowg88X747AVCy4eUvoPeO_aNBuUGl1rTcxCUo");

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
    }
}

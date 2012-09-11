using System;
using System.Net;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsManualTester.Web.Tfs
{
    public class TeamProject
    {
        private readonly Uri _uri;
        private readonly string _userName;
        private readonly string _password;

        private TfsTeamProjectCollection _tfs;

        public TeamProject(Uri uri, string userName, string password)
        {
            _uri = uri;
            _userName = userName;
            _password = password;
        }

        public void EnsureAuthenticated()
        {
            EnsureTfs();
            _tfs.EnsureAuthenticated();
        }

        public int GetTestPlanCount(string teamProject)
        {
            EnsureTfs();

            var testProject = GetTestProject(teamProject);
            var testPlans = testProject.TestPlans.Query("SELECT * FROM TestPlan");
            return testPlans.Count;
        }

        public ITestCase GetTestCase(string teamProject, int testCaseId)
        {
            EnsureTfs();

            var testProject = GetTestProject(teamProject);
            return testProject.TestCases.Find(testCaseId);
        }
        
        private void EnsureTfs()
        {
            if (_tfs != null)
                return;

            var credentialsProvider = new ServiceIdentityCredentialsProvider(_userName, _password);
            _tfs = new TfsTeamProjectCollection(
                _uri, CredentialCache.DefaultCredentials, credentialsProvider);
        }

        private ITestManagementTeamProject GetTestProject(string teamProject)
        {
            var testManagement = _tfs.GetService<ITestManagementService>();
            return testManagement.GetTeamProject(teamProject);
        }
    }
}
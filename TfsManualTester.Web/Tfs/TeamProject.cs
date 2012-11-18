using System;
using System.Net;
using System.Web;
using System.Web.Caching;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;

namespace TfsManualTester.Web.Tfs
{
    public class TeamProject
    {
        private static readonly object _cacheLock = new object();

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

        private string CacheKey
        {
            get { return _uri.ToString() + "_" + _userName; }
        }

        public void EnsureAuthenticated(bool allowCache)
        {
            EnsureTfs(allowCache);
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

        private void EnsureTfs(bool allowCache = true)
        {
            if (_tfs != null)
                return;

            if (allowCache)
            {
                var tfsFromCache = HttpRuntime.Cache[CacheKey] as TfsTeamProjectCollection;
                if (tfsFromCache != null)
                {
                    _tfs = tfsFromCache;
                    return;
                }
            }

            lock (_cacheLock)
            {
                if (allowCache && _tfs != null)
                    return;

                var credentialsProvider = new ServiceIdentityCredentialsProvider(_userName, _password);
                _tfs = new TfsTeamProjectCollection(
                    _uri, CredentialCache.DefaultCredentials, credentialsProvider);

                HttpRuntime.Cache.Add(
                    CacheKey,
                    _tfs,
                    null,
                    Cache.NoAbsoluteExpiration,
                    new TimeSpan(1, 0, 0),
                    CacheItemPriority.Normal,
                    null);
            }
        }

        private ITestManagementTeamProject GetTestProject(string teamProject)
        {
            var testManagement = _tfs.GetService<ITestManagementService>();
            return testManagement.GetTeamProject(teamProject);
        }
    }
}
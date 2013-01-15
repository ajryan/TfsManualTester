using System;
using System.Web;
using System.Web.Caching;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using TfsManualTester.Web.Authorization;

namespace TfsManualTester.Web.Tfs
{
    public class TeamProject
    {
        private static readonly object _CacheLock = new object();

        private readonly UserDataPrincipal _principal;

        private TfsTeamProjectCollection _tfs;

        public TeamProject(UserDataPrincipal principal)
        {
            _principal = principal;
        }

        private string CacheKey
        {
            get { return _principal.TfsUrl + "_" + _principal.UserName; }
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

            lock (_CacheLock)
            {
                if (allowCache && _tfs != null)
                    return;

                _tfs = new TfsTeamProjectCollection(
                    new Uri(_principal.TfsUrl), _principal.GetCredentialsProvider());
                    //new Uri(_principal.TfsUrl), CredentialCache.DefaultCredentials, _principal.GetCredentialsProvider());

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
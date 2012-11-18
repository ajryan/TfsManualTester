using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace TfsManualTester.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public static DocumentStore Store;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //string ravenHqConnectionString = ConfigurationManager.AppSettings["RAVENHQ_CONNECTION_STRING"];
            Store = new DocumentStore {ConnectionStringName = "RavenDB"};
            Store.Initialize();

            // TODO: enable once we have indexes
            //IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), Store);
        }
    }
}
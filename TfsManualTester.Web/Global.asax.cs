using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Raven.Client.Document;

namespace TfsManualTester.Web
{
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

            Store = new DocumentStore {ConnectionStringName = "RavenDB"};
            Store.Initialize();

            // TODO: enable once we have indexes
            //IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), Store);
        }
    }
}
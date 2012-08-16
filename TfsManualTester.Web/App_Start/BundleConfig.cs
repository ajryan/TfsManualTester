using System.Web.Optimization;

namespace TfsManualTester.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(
                new ScriptBundle("~/bundles/jquery")
                    .Include("~/Scripts/jquery-{version}.js"));
        }
    }
}
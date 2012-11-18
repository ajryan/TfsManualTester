using System.Web.Optimization;

namespace TfsManualTester.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(
                new ScriptBundle("~/bundles/app")
                    .Include("~/Scripts/app.js"));

            bundles.Add(
                new ScriptBundle("~/bundles/jquery")
                    .Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(
                new ScriptBundle("~/bundles/bootstrap-js")
                    .Include("~/Scripts/bootstrap.js")
                );

            bundles.Add(
                new ScriptBundle("~/bundles/knockout")
                    .Include("~/Scripts/knockout-{version}.js")
                    .Include("~/Scripts/knockout.mapping-latest.js")
                    .Include("~/Scripts/knockout.validation.js"));

            bundles.Add(
                new StyleBundle("~/bundles/bootstrap-css")
                    .Include("~/Content/bootstrap.css")
                    .Include("~/Content/bootstrap-custom.css")
                    .Include("~/Content/bootstrap-responsive.css"));

        }
    }
}
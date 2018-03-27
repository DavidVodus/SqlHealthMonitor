using System.Web.Optimization;

namespace SqlHealthMonitor
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
          
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-{version}.js",
                         "~/Scripts/jquery/jquery-migrate-3.0.0.min.js",
                         "~/Scripts/jquery/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/gridApp").Include(
                     "~/Scripts/gridApp/*.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                    "~/Scripts/jszip/jszip.min.js",
                     "~/Scripts/kendo/*.js"));
            bundles.Add(new StyleBundle("~/Content/kendo/css")
                .Include("~/Content/kendo/*.css",
                       "~/Content/bootstrap/*.css",
                           "~/Content/gridApp/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/JQueryUI").Include(
                   "~/Scripts/JQueryUI/*.js"));
            bundles.Add(new StyleBundle("~/Content/JQueryUI/css")
                .Include("~/Content/JQueryUI/*.css"));
            bundles.IgnoreList.Clear();
        }
    }
}

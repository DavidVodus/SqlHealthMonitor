using System.Web.Optimization;

namespace SqlHealthMonitor
{
    public class BundleConfig
    {

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
#if DEBUG
        BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-{version}.js",
                         //"~/Scripts/jquery/jquery-migrate-3.0.0.min.js",
                         "~/Scripts/jquery/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/utility").Include(
                   "~/Scripts/Utility/*.js"));
            bundles.Add(new ScriptBundle("~/bundles/jtable").Include(
                     "~/Scripts/jtable/*.js"));
            bundles.Add(new StyleBundle("~/Content/jtable/css")
             .Include("~/Content/jtable/themes/jqueryui/*.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                 "~/Scripts/bootstrap/popper.js",
                      "~/Scripts/bootstrap/*.js"));
            bundles.Add(new StyleBundle("~/Content/bootstrap/css")
            .Include("~/Content/bootstrap/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/ChartJS").Include(
                   "~/Scripts/ChartJS/*.js"));

            bundles.Add(new StyleBundle("~/Content/main/css")
       .Include("~/Content/main/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/JQueryUI").Include(
                   "~/Scripts/JQueryUI/*.js"));
            bundles.Add(new StyleBundle("~/Content/JQueryUI/css")
                .Include("~/Content/JQueryUI/*.css"));
            bundles.IgnoreList.Clear();
        }
    }
}

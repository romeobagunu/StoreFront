using System.Web.Optimization;

namespace StoreFront.UI.MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        //"~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      //"~/Scripts/bootstrap.js",
                      //"~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      //"~/Content/site.css"));

            bundles.Add(new Bundle("~/bundles/scripts-template").Include(
                "~/Content/vendor/bootstrap/js/bootstrap.bundle.min.js",
                "~/Content/vendor/waypoints/lib/noframework.waypoints.js",
                "~/Content/vendor/swiper/swiper-bundle.min.js",
                "~/Content/vendor/choices.js/public/assets/scripts/choices.js",
                "~/Scripts/theme.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css-template").Include(
                "~/Content/vendor/swiper/swiper-bundle.min.css",
                "~/Content/vendor/choices.js/public/assets/styles/choices.css",
                "~/Content/css/style.red.min.css",
                "~/Content/css/custom.css"
                ));

            

        }
    }
}

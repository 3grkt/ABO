using System;
using System.Web;
using System.Web.Optimization;

namespace ABO.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Reset and re-add ignore list
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            // Bootstrap
            // JS
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                       "~/Scripts/bootstrap.js"));
            bundles.Add(new StyleBundle("~/Content/jqueryui").Include("~/Content/jquery-ui-{version}.css"));
            // CSS
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap/bootstrap.css"));

            // FontAwesome
            bundles.Add(new StyleBundle("~/Content/font-awesome").Include("~/Content/font-awesome/font-awesome.css"));

            // Alertify
            bundles.Add(new StyleBundle("~/Content/alertify").Include("~/Content/alertify.min.css",
                "~/Content/themes/alertify.theme.default.min.css"));

            bundles.Add(new ScriptBundle("~/Scripts/alertify").Include("~/Scripts/alertify.min.js"));

            // Should not use pdfjs with bundle on which included worker file cannot load separately.
            //bundles.Add(new ScriptBundle("~/Scripts/pdfjs").Include("~/Scripts/pdf-{version}.js"));
            //bundles.Add(new ScriptBundle("~/Scripts/pdfjs.worker").Include("~/Scripts/pdf-{version}.worker.js"));

            bundles.Add(new ScriptBundle("~/Scripts/encoding").Include("~/Scripts/encoding-indexes-{version}.js", "~/Scripts/encoding-{version}.js"));
        }

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }
    }
}
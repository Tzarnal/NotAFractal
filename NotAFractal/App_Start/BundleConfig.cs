using System.Web.Optimization;

namespace NotAFractal
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts")
                            .Include("~/Scripts/jquery-{version}.js")
                            .Include("~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/bundles/css")
                            .Include("~/Content/css/bootstrap.css")
                            .Include("~/Content/css/bootstrap-responsive.css"));
        }
    }
}

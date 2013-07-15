using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;
using System.Web;
using System.Web.Optimization;

namespace ODF.Site
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
            var cssTransformer = new CssTransformer();
            var jsTransformer = new JsTransformer();
            var nullOrderer = new NullOrderer();

            var css = new Bundle("~/css/base")
                .Include("~/Less/main.less");
            css.Transforms.Add(cssTransformer);
            css.Orderer = nullOrderer;

            bundles.Add(css);

            //var jquery = new Bundle("~/bundles/jquery")
            //    .Include("~/Scripts/jquery-1.*");
            //jquery.Transforms.Add(jsTransformer);
            //jquery.Orderer = nullOrderer;

            //bundles.Add(jquery);

		}
	}
}
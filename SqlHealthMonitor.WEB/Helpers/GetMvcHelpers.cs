using System.Web.WebPages;

namespace SqlHealthMonitor.Helpers
{
    public static class GetMvcHelpers
    {
        public static System.Web.Mvc.AjaxHelper Ajax => ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page).Ajax;

        public static System.Web.Mvc.HtmlHelper Html => ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page).Html;

        public static System.Web.Mvc.UrlHelper Url => ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page).Url;
    }
}
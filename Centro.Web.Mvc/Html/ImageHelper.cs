using System.Web.Mvc;
using System.Web.Routing;

namespace Centro.Web.Mvc.Html
{
    public static class ImageHelper
    {
        public static string ImageContent(this HtmlHelper helper, string contentPath, string alt)
        {
            return ImageContent(helper, contentPath, alt, string.Empty, null);
        }

        public static string ImageContent(this HtmlHelper helper, string contentPath, string alt, string id)
        {
            return ImageContent(helper, contentPath, alt, id, null);
        }

        public static string ImageContent(this HtmlHelper helper, string contentPath, string alt, object htmlAttributes)
        {
            return ImageContent(helper, contentPath, alt, string.Empty, htmlAttributes);
        }

        public static string ImageContent(this HtmlHelper helper, string contentPath, string alt, string id, object htmlAttributes)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var builder = new TagBuilder("img");
            if (!string.IsNullOrEmpty(id))
                builder.GenerateId(id);
            builder.MergeAttribute("src", urlHelper.Content(contentPath));
            builder.MergeAttribute("alt", alt);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.SelfClosing);
        }
    }
}
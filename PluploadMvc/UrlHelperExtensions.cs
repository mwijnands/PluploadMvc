using System;
using System.Web.Mvc;

namespace XperiCode.PluploadMvc
{
    public static class UrlHelperExtensions
    {
        public static string PluploadHandler(this UrlHelper urlHelper, string reference)
        {
            return urlHelper.Content(string.Concat("~/Plupload.axd?reference=", reference));
        }

        public static string PluploadHandler(this UrlHelper urlHelper, PluploadFileCollection collection)
        {
            return urlHelper.Content(string.Concat("~/Plupload.axd?reference=", collection.Reference));
        }
    }
}

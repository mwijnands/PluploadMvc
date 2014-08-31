using System;
using System.Linq;
using System.Web;

namespace XperiCode.PluploadMvc
{
    public static class PluploadExtensions
    {
        private const string PluploadContextKey = "PluploadContext";

        public static IPluploadContext GetPluploadContext(this HttpContextBase httpContext)
        {
            var context = httpContext.Items[PluploadContextKey] as PluploadContext;
            if (context == null)
            {
                context = new PluploadContext(httpContext);
                httpContext.SetPluploadContext(context);
            }
            return context;
        }

        public static void SetPluploadContext(this HttpContextBase httpContext, IPluploadContext context)
        {
            httpContext.Items[PluploadContextKey] = context;
        }
    }
}

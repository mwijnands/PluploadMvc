using System;
using System.Web;

namespace XperiCode.PluploadMvc
{
    public class PluploadHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var wrapper = new HttpContextWrapper(context);
            ProcessRequest(wrapper);
        }

        public void ProcessRequest(HttpContextBase context)
        {
            var pluploadContext = context.GetPluploadContext();
            var reference = Guid.Parse(context.Request.Params.Get("reference"));

            foreach (var fileKey in context.Request.Files.AllKeys)
            {
                var file = context.Request.Files.Get(fileKey);
                pluploadContext.SaveFile(file, reference);
            }

            context.Response.Write("OK");
        }
    }
}

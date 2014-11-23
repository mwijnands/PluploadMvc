using System;
using System.Linq;
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
            string reference = context.Request.Params["reference"];

            if (!PluploadContext.ValidateReference(reference))
            {
                context.Response.StatusCode = 500;
                context.Response.Write("No reference found in postdata or querystring, or reference contains invalid filename chars.");
                return;
            }

            if (!context.Request.Files.AllKeys.Any())
            {
                return;
            }

            int chunks;
            int.TryParse(context.Request.Params["chunks"], out chunks);

            var pluploadContext = context.GetPluploadContext();
            var file = context.Request.Files[0];

            if (chunks > 0)
            {
                int chunk = int.Parse(context.Request.Params["chunk"]);
                string fileName = context.Request.Params["name"];

                pluploadContext.SaveChunk(file, reference, fileName, chunk, chunks);
            }
            else
            {
                pluploadContext.SaveFile(file, reference);
            }

            context.Response.Write("OK");
        }
    }
}

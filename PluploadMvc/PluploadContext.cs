using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XperiCode.PluploadMvc
{
    public class PluploadContext : IPluploadContext
    {
        private readonly HttpContextBase _httpContext;

        public PluploadContext(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public ContentResult SaveFile(HttpPostedFileBase file, Guid reference)
        {
            string uploadPath = GetUploadPath(reference);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string fileSavePath = Path.Combine(uploadPath, Path.GetFileName(file.FileName));
            file.SaveAs(fileSavePath);

            string contentTypeSavePath = string.Concat(fileSavePath, PluploadFile.ContentTypeExtension);
            System.IO.File.WriteAllText(contentTypeSavePath, file.ContentType);

            return new ContentResult
            {
                Content = "OK"
            };
        }

        public IEnumerable<HttpPostedFileBase> GetFiles(Guid reference)
        {
            string uploadPath = GetUploadPath(reference);
            if (!Directory.Exists(uploadPath))
            {
            }

            var fileNamePaths = Directory.GetFiles(uploadPath).Where(p => !p.EndsWith(PluploadFile.ContentTypeExtension));
            foreach (var fileNamePath in fileNamePaths)
            {
                var file = new PluploadFile(fileNamePath);

                _httpContext.DisposeOnPipelineCompleted(file);

                yield return file;
            }
        }

        private string GetUploadPath(Guid reference)
        {
            return Path.Combine(_httpContext.Server.MapPath("~/App_Data/PluploadMvc"), reference.ToString());
        }
    }
}

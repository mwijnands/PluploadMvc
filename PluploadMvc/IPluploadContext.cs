using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XperiCode.PluploadMvc
{
    public interface IPluploadContext
    {
        void SaveChunk(HttpPostedFileBase file, Guid reference, string fileName, int chunk, int chunks);
        void SaveFile(HttpPostedFileBase file, Guid reference);
        IEnumerable<HttpPostedFileBase> GetFiles(Guid reference);
        void DeleteFiles(Guid reference);
        void DeleteFiles(PluploadFileCollection collection);
    }
}
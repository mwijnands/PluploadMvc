using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XperiCode.PluploadMvc
{
    public interface IPluploadContext
    {
        void SaveChunk(HttpPostedFileBase file, string reference, string fileName, int chunk, int chunks);
        void SaveFile(HttpPostedFileBase file, string reference);
        IEnumerable<HttpPostedFileBase> GetFiles(string reference);
        void DeleteFiles(string reference);
        void DeleteFiles(PluploadFileCollection collection);
    }
}
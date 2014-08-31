using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XperiCode.PluploadMvc
{
    public interface IPluploadContext
    {
        void SaveFile(HttpPostedFileBase file, Guid reference);
        IEnumerable<HttpPostedFileBase> GetFiles(Guid reference);
    }
}
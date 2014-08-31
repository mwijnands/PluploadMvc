using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XperiCode.PluploadMvc
{
    public interface IPluploadContext
    {
        ContentResult SaveFile(HttpPostedFileBase file, Guid reference);
        IEnumerable<HttpPostedFileBase> GetFiles(Guid reference);
    }
}
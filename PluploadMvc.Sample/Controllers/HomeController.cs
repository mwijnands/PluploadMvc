using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XperiCode.PluploadMvc.Sample.Models;

namespace XperiCode.PluploadMvc.Sample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new HomeIndexViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitForm1(HomeIndexViewModel model)
        {
            var files1 = HttpContext.GetPluploadContext().GetFiles(model.UploadReference1);
            var files2 = HttpContext.GetPluploadContext().GetFiles(model.UploadReference2);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, Guid reference)
        {
            HttpContext.GetPluploadContext().SaveFile(file, reference);
            return Content("OK");
        }
    }
}

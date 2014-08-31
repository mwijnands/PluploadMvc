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
            var pluploadContext = HttpContext.GetPluploadContext();

            var files1 = pluploadContext.GetFiles(model.UploadReference1).ToList();
            var files2 = pluploadContext.GetFiles(model.UploadReference2).ToList();

            pluploadContext.DeleteFiles(model.UploadReference1);
            pluploadContext.DeleteFiles(model.UploadReference2);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, Guid reference)
        {
            var pluploadContext = HttpContext.GetPluploadContext();

            pluploadContext.SaveFile(file, reference);

            return Content("OK");
        }
    }
}

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
            return View();
        }

        public ActionResult SampleForm1()
        {
            var model = new SampleForm1Model();
            return View(model);
        }

        public ActionResult SampleForm2()
        {
            var model = new SampleForm2Model();
            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitForm1(SampleForm1Model model)
        {
            var pluploadContext = HttpContext.GetPluploadContext();

            var files1 = pluploadContext.GetFiles(model.UploadReference1).ToList();
            var files2 = pluploadContext.GetFiles(model.UploadReference2).ToList();

            pluploadContext.DeleteFiles(model.UploadReference1);
            pluploadContext.DeleteFiles(model.UploadReference2);

            return RedirectToAction("SampleForm1");
        }

        [HttpPost]
        public ActionResult SubmitForm2(SampleForm2Model model)
        {
            var files1 = model.Files;
            var files2 = model.OtherFiles;

            var pluploadContext = HttpContext.GetPluploadContext();

            pluploadContext.DeleteFiles(model.Files.Reference);
            pluploadContext.DeleteFiles(model.OtherFiles.Reference);

            return RedirectToAction("SampleForm2");
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

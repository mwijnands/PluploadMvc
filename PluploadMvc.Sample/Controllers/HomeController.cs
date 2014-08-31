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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult SubmitForm(HomeIndexViewModel model)
        {
            var files = HttpContext.GetPluploadContext().GetFiles(model.UploadReference);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, Guid reference)
        {
            return HttpContext.GetPluploadContext().SaveFile(file, reference);
        }
  
    }
}

using System;
using System.Collections.Generic;
using System.IO;
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

        private const string PluploadUploadedFileContentTypeExtension = ".contenttype";

        [HttpPost]
        public ActionResult SubmitForm(HomeIndexViewModel model)
        {
            string uploadPath = GetUploadPath(model.UploadReference);
            if (Directory.Exists(uploadPath))
            {
                var fileNamePaths = Directory.GetFiles(uploadPath).Where(p => !p.EndsWith(PluploadUploadedFileContentTypeExtension));
                var files = new List<PluploadFile>();
                foreach (var fileNamePath in fileNamePaths)
                {
                    // TODO: add eventhandler to end of request where stream is closed and file deleted?
                    string contentType = string.Empty;
                    string contentTypeFileNamePath = GetContentTypeFilePathName(fileNamePath);
                    if (System.IO.File.Exists(contentTypeFileNamePath))
                    {
                        contentType = System.IO.File.ReadAllText(contentTypeFileNamePath);
                    }

                    var stream = System.IO.File.OpenRead(fileNamePath);
                    var file = new PluploadFile(Path.GetFileName(fileNamePath), stream, contentType);
                    files.Add(file);
                }
                // Do something with the files.
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase file, Guid reference)
        {
            string uploadPath = GetUploadPath(reference);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string fileSavePath = Path.Combine(uploadPath, Path.GetFileName(file.FileName));
            file.SaveAs(fileSavePath);

            string contentTypeSavePath = GetContentTypeFilePathName(fileSavePath);
            System.IO.File.WriteAllText(contentTypeSavePath, file.ContentType);

            return Content("OK");
        }
  
        private string GetUploadPath(Guid reference)
        {
            return Path.Combine(HttpContext.Server.MapPath("~/App_Data/PluploadMvc"), reference.ToString());
        }
  
        private static string GetContentTypeFilePathName(string fileNamePath)
        {
            return string.Concat(fileNamePath, PluploadUploadedFileContentTypeExtension);
        }
    }
}

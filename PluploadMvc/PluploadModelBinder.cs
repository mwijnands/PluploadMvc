using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace XperiCode.PluploadMvc
{
    public class PluploadModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string reference = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;

            var pluploadContext = controllerContext.HttpContext.GetPluploadContext();
            var files = pluploadContext.GetFiles(reference).Cast<PluploadFile>().ToList();

            return new PluploadFileCollection(files, reference);
        }
    }
}

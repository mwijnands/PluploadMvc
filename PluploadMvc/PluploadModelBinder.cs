using System;
using System.Linq;
using System.Web.Mvc;

namespace XperiCode.PluploadMvc
{
    public class PluploadModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string referenceKey = string.Concat(bindingContext.ModelName, ".Reference");
            Guid reference = Guid.Parse(bindingContext.ValueProvider.GetValue(referenceKey).AttemptedValue);

            var pluploadContext = controllerContext.HttpContext.GetPluploadContext();
            var files = pluploadContext.GetFiles(reference).Cast<PluploadFile>().ToList();

            return new PluploadFileCollection(files)
            {
                Reference = reference
            };
        }
    }
}

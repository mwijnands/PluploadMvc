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
            string referencePropertyName = GetPropertyName(() => ((PluploadFileCollection)bindingContext.Model).Reference);
            string referenceKey = string.Format("{0}.{1}", bindingContext.ModelName, referencePropertyName);
            Guid reference = Guid.Parse(bindingContext.ValueProvider.GetValue(referenceKey).AttemptedValue);

            var pluploadContext = controllerContext.HttpContext.GetPluploadContext();
            var files = pluploadContext.GetFiles(reference).Cast<PluploadFile>().ToList();

            return new PluploadFileCollection(files)
            {
                Reference = reference
            };
        }

        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            var body = expression.Body as MemberExpression;
            if (body != null)
            {
                return body.Member.Name;
            }
            return string.Empty;
        }    
    }
}

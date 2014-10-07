using System.Web.Mvc;
using XperiCode.PluploadMvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.PluploadMvcConfig), "PreStart")]

namespace $rootnamespace$.App_Start
{
    public class PluploadMvcConfig
    {
        public static void PreStart() 
        { 
            ModelBinders.Binders.Add(typeof(PluploadFileCollection), new PluploadModelBinder());
        }
    }
}

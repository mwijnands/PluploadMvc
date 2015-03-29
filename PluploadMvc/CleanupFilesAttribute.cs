using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace XperiCode.PluploadMvc
{
    public class CleanupFilesAttribute : ActionFilterAttribute
    {
        private readonly IDictionary<string, PluploadFileCollection> _fileCollections;

        public CleanupFilesAttribute()
        {
            this._fileCollections = new Dictionary<string, PluploadFileCollection>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!IsPost(filterContext))
            {
                return;
            }

            CollectFileCollections(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!IsPost(filterContext))
            {
                return;
            }

            DeleteUploadedFiles(filterContext);
        }

        private bool IsPost(ControllerContext filterContext)
        {
            return filterContext.HttpContext.Request.HttpMethod == HttpMethod.Post.Method;
        }

        private void CollectFileCollections(ActionExecutingContext filterContext)
        {
            var pluploadFileCollectionType = typeof(PluploadFileCollection);

            foreach (var param in filterContext.ActionParameters)
            {
                if (param.Value == null)
                {
                    continue;
                }

                var paramValueType = param.Value.GetType();
                if (paramValueType.Equals(pluploadFileCollectionType))
                {
                    _fileCollections.Add(param.Key, (PluploadFileCollection)param.Value);
                    continue;
                }

                foreach (var prop in paramValueType.GetProperties().Where(p => p.PropertyType.Equals(pluploadFileCollectionType)))
                {
                    _fileCollections.Add(prop.Name, (PluploadFileCollection)prop.GetValue(param.Value));
                }
            }
        }

        private void DeleteUploadedFiles(ActionExecutedContext filterContext)
        {
            foreach (var fileCollection in _fileCollections)
            {
                filterContext.HttpContext.GetPluploadContext().DeleteFiles(fileCollection.Value);
            }
        }
    }
}

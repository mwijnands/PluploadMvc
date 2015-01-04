using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace XperiCode.PluploadMvc.Tests
{
    [TestClass]
    public class UrlHelperExtensionsTests
    {
        [TestMethod]
        public void PluploadHandler_Should_Return_PluploadHandler_Url_When_Supplied_Reference()
        {
            var urlHelper = CreateUrlHelper();
            string reference = "123";

            string url = UrlHelperExtensions.PluploadHandler(urlHelper, reference);

            Assert.AreEqual(string.Concat("/Plupload.axd?reference=", reference), url);
        }

        [TestMethod]
        public void PluploadHandler_Should_Return_PluploadHandler_Url_When_Supplied_Collection()
        {
            var urlHelper = CreateUrlHelper();
            var collection = new PluploadFileCollection();

            string url = UrlHelperExtensions.PluploadHandler(urlHelper, collection);

            Assert.AreEqual(string.Concat("/Plupload.axd?reference=", collection.Reference), url);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void PluploadHandler_Should_Throw_ArgumentNullException_When_Collection_Is_Null()
        {
            var urlHelper = CreateUrlHelper();
            PluploadFileCollection collection = null;

            UrlHelperExtensions.PluploadHandler(urlHelper, collection);
        }

        private UrlHelper CreateUrlHelper()
        {
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(x => x.ApplicationPath).Returns("/");

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.Setup(x => x.GetService(typeof(HttpWorkerRequest))).Returns(null);

            var urlHelper = new UrlHelper(new RequestContext(context.Object, new RouteData()), new RouteCollection());
            return urlHelper;
        }
    }
}

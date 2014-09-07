using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Specialized;
using System.Web;

namespace XperiCode.PluploadMvc.Tests
{
    [TestClass]
    public class PluploadExtensionsTests
    {
        [TestMethod]
        public void Should_Get_New_PluploadContext_When_None_Exists_In_HttpContext_Items()
        {
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.SetupGet(c => c.Items).Returns(new ListDictionary());
            var httpContext = httpContextMock.Object;

            var pluploadContext = httpContext.GetPluploadContext();

            Assert.IsNotNull(pluploadContext);
        }

        [TestMethod]
        public void Should_Get_Existing_PluploadContext_When_One_Is_Set()
        {
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.SetupGet(c => c.Items).Returns(new ListDictionary());
            var httpContext = httpContextMock.Object;

            var newPluploadContext = new PluploadContext(httpContext);
            httpContext.SetPluploadContext(newPluploadContext);
            var pluploadContext = httpContext.GetPluploadContext();

            Assert.AreSame(newPluploadContext, pluploadContext);
        }
    }
}

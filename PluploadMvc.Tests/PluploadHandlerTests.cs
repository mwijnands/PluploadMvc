using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XperiCode.PluploadMvc.Tests
{
    [TestClass()]
    public class PluploadHandlerTests
    {
        [TestMethod()]
        public void ProcessRequest_Should_Save_File()
        {
            var reference = Guid.NewGuid();
            var pluploadContextMock = new Mock<IPluploadContext>();
            var httpPostedFileMock = new Mock<HttpPostedFileBase>();
            var httpFileCollectionMock = new Mock<HttpFileCollectionBase>();
            httpFileCollectionMock.SetupGet(c => c.AllKeys).Returns(new string[] { "FileName.Extension" });
            httpFileCollectionMock.Setup(c => c.Get("FileName.Extension")).Returns(httpPostedFileMock.Object);
            var httpRequestMock = new Mock<HttpRequestBase>();
            httpRequestMock.SetupGet(r => r.Files).Returns(httpFileCollectionMock.Object);
            httpRequestMock.SetupGet(r => r.Params).Returns(new NameValueCollection() { { "reference", reference.ToString() } });
            var httpResponseMock = new Mock<HttpResponseBase>();
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.SetupGet(c => c.Request).Returns(httpRequestMock.Object);
            httpContextMock.SetupGet(c => c.Response).Returns(httpResponseMock.Object);
            httpContextMock.SetupGet(c => c.Items).Returns(new ListDictionary());
            httpContextMock.Object.SetPluploadContext(pluploadContextMock.Object);

            var handler = new PluploadHandler();
            handler.ProcessRequest(httpContextMock.Object);

            pluploadContextMock.Verify(c => c.SaveFile(httpPostedFileMock.Object, reference), Times.Once);
            httpResponseMock.Verify(r => r.Write("OK"), Times.Once);
        }
    }
}

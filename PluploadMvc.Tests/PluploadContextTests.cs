using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Web;

namespace XperiCode.PluploadMvc.Tests
{
    [TestClass()]
    public class PluploadContextTests
    {
        [TestMethod()]
        public void Should_Be_Disposed_When_Request_Pipeline_Completed()
        {
            var httpContextMock = new Mock<HttpContextBase>();
            var httpContext = httpContextMock.Object;

            var pluploadContext = new PluploadContext(httpContext);

            httpContextMock.Verify(c => c.DisposeOnPipelineCompleted(pluploadContext), Times.Once);
        }

        [TestMethod()]
        public void Should_Get_Saved_File_Including_Content_Type()
        {
            string uploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString());

            var httpServerUtilityMock = new Mock<HttpServerUtilityBase>();
            httpServerUtilityMock.Setup(u => u.MapPath("~/App_Data/PluploadMvc"))
                .Returns(uploadPath);

            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.SetupGet(c => c.Server).Returns(httpServerUtilityMock.Object);

            var httpPostedFileMock = new Mock<HttpPostedFileBase>();
            httpPostedFileMock.SetupGet(f => f.FileName).Returns("FileName.Extension");
            httpPostedFileMock.SetupGet(f => f.ContentLength).Returns(2);
            httpPostedFileMock.SetupGet(f => f.ContentType).Returns("application/pdf");

            using (var stream = new MemoryStream(new byte[] { 111, 222 }))
            {
                httpPostedFileMock.SetupGet(f => f.InputStream).Returns(stream);

                var reference = Guid.NewGuid();

                using (var pluploadContext = new PluploadContext(httpContextMock.Object))
                {
                    pluploadContext.SaveFile(httpPostedFileMock.Object, reference);

                    var file = pluploadContext.GetFiles(reference).FirstOrDefault();

                    Assert.IsNotNull(file);
                    Assert.AreEqual(file.FileName, httpPostedFileMock.Object.FileName);
                    Assert.AreEqual(file.ContentLength, httpPostedFileMock.Object.ContentLength);
                    Assert.AreEqual(file.ContentType, httpPostedFileMock.Object.ContentType);
                    Assert.AreEqual(file.InputStream.Length, httpPostedFileMock.Object.InputStream.Length);
                }
            }

            try
            {
                Directory.Delete(uploadPath, true);
            }
            catch (Exception)
            {
                // Could happen..
            }
        }
    }
}

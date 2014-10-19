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
            httpServerUtilityMock.Setup(u => u.MapPath(PluploadContext.UploadVirtualPath))
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

                    // TODO: Split up this test in separate tests for saving, getting and deleting.
                    pluploadContext.DeleteFiles(reference);

                    Assert.AreEqual(0, pluploadContext.GetFiles(reference).Count());
                    Assert.AreEqual(0, Directory.GetFiles(uploadPath).Count());
                }
            }

            try
            {
                Directory.Delete(uploadPath, true);
            }
            catch (IOException)
            {
                // Files could always be in use by virusscanners and what not.. So ignore it.
            }
        }

        [TestMethod()]
        public void DeleteFiles_Should_Only_Dispose_And_Delete_Files_Of_Given_Reference()
        {
            string uploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString());

            var httpServerUtilityMock = new Mock<HttpServerUtilityBase>();
            httpServerUtilityMock.Setup(u => u.MapPath(PluploadContext.UploadVirtualPath))
                .Returns(uploadPath);

            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.SetupGet(c => c.Server).Returns(httpServerUtilityMock.Object);

            var httpPostedFile1Mock = new Mock<HttpPostedFileBase>();
            httpPostedFile1Mock.SetupGet(f => f.FileName).Returns("FileName1.Extension");
            httpPostedFile1Mock.SetupGet(f => f.ContentLength).Returns(2);
            httpPostedFile1Mock.SetupGet(f => f.ContentType).Returns("application/pdf");

            var httpPostedFile2Mock = new Mock<HttpPostedFileBase>();
            httpPostedFile2Mock.SetupGet(f => f.FileName).Returns("FileName2.Extension");
            httpPostedFile2Mock.SetupGet(f => f.ContentLength).Returns(2);
            httpPostedFile2Mock.SetupGet(f => f.ContentType).Returns("application/pdf");

            using (var stream1 = new MemoryStream(new byte[] { 111, 222 }))
            using (var stream2 = new MemoryStream(new byte[] { 111, 222 }))
            {
                httpPostedFile1Mock.SetupGet(f => f.InputStream).Returns(stream1);
                httpPostedFile2Mock.SetupGet(f => f.InputStream).Returns(stream2);

                var reference1 = Guid.NewGuid();
                var reference2 = Guid.NewGuid();

                using (var pluploadContext = new PluploadContext(httpContextMock.Object))
                {
                    pluploadContext.SaveFile(httpPostedFile1Mock.Object, reference1);
                    pluploadContext.SaveFile(httpPostedFile2Mock.Object, reference2);

                    var file2 = pluploadContext.GetFiles(reference2).FirstOrDefault();

                    pluploadContext.DeleteFiles(reference1);

                    Assert.AreEqual(0, pluploadContext.GetFiles(reference1).Count());
                    Assert.IsTrue(!Directory.Exists(pluploadContext.GetUploadPath(reference1))
                        || !Directory.GetFiles(pluploadContext.GetUploadPath(reference1)).Any());
                    Assert.AreEqual(1, pluploadContext.GetFiles(reference2).Count());
                    Assert.AreEqual(2, Directory.GetFiles(pluploadContext.GetUploadPath(reference2)).Count());
                    Assert.IsTrue(file2.InputStream.Length > 0);
                }
            }

            try
            {
                Directory.Delete(uploadPath, true);
            }
            catch (IOException)
            {
                // Files could always be in use by virusscanners and what not.. So ignore it.
            }
        }
    }
}

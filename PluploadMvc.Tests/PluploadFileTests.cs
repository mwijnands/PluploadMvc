using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace XperiCode.PluploadMvc.Tests
{
    [TestClass()]
    public class PluploadFileTests
    {
        [TestMethod()]
        public void Should_Set_Properties_When_Constructing_New_File_And_Should_Be_Deleted_When_Disposed()
        {
            string tempFileName = "FileName.Extension";
            string tempFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString());
            string tempFileNamePath = Path.Combine(tempFilePath, tempFileName);
            Guid reference = Guid.NewGuid();

            if (!Directory.Exists(tempFilePath))
            {
                Directory.CreateDirectory(tempFilePath);
            }

            using (var fileStream = File.Create(tempFileNamePath))
            {
                fileStream.WriteByte(111);
                fileStream.WriteByte(222);
                fileStream.Flush();
            }

            PluploadFile file;

            using (file = new PluploadFile(tempFileNamePath, reference))
            {
                Assert.AreEqual(tempFileName, file.FileName);
                Assert.AreEqual(2, file.ContentLength);
                Assert.IsNull(file.ContentType);
                Assert.IsNotNull(file.InputStream);
                Assert.AreEqual(2, file.InputStream.Length);
                Assert.AreEqual(reference, file.Reference);
            }

            try
            {
                file.InputStream.Position = 0;
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ObjectDisposedException));
            }

            try
            {
                Directory.Delete(tempFilePath, true);
            }
            catch (IOException)
            {
                // Files could always be in use by virusscanners and what not.. So ignore it.
            }
        }

        [TestMethod]
        public void Should_Have_Empty_Properties_When_Supplied_Not_Existing_FileNamePath()
        {
            using (var file = new PluploadFile(string.Concat(@"c:\", Guid.NewGuid(), Guid.NewGuid()), Guid.NewGuid()))
            {
                Assert.IsNull(file.FileName);
                Assert.IsNull(file.ContentType);
                Assert.IsNull(file.InputStream);
                Assert.AreEqual(0, file.ContentLength);
            }
        }

        [TestMethod]
        public void Should_Save_File_To_Disk_When_SaveAs_Is_Called()
        {
            string tempFileName = "FileName.Extension";
            string tempFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString());
            string tempFileNamePath = Path.Combine(tempFilePath, tempFileName);

            if (!Directory.Exists(tempFilePath))
            {
                Directory.CreateDirectory(tempFilePath);
            }

            using (var fileStream = File.Create(tempFileNamePath))
            {
                fileStream.WriteByte(111);
                fileStream.WriteByte(222);
                fileStream.Flush();
            }

            using (var file = new PluploadFile(tempFileNamePath, Guid.NewGuid()))
            {
                string tempFileName2 = "FileName.Extension";
                string tempFilePath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString());
                string tempFileNamePath2 = Path.Combine(tempFilePath2, tempFileName2);

                if (!Directory.Exists(tempFilePath2))
                {
                    Directory.CreateDirectory(tempFilePath2);
                }

                file.SaveAs(tempFileNamePath2);

                Assert.IsTrue(File.Exists(tempFileNamePath2));

                try
                {
                    Directory.Delete(tempFilePath2, true);
                }
                catch (IOException)
                {
                    // Files could always be in use by virusscanners and what not.. So ignore it.
                }
            }

            try
            {
                Directory.Delete(tempFilePath, true);
            }
            catch (IOException)
            {
                // Files could always be in use by virusscanners and what not.. So ignore it.
            }
        }
    }
}

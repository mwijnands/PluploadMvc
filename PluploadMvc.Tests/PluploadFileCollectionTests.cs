using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XperiCode.PluploadMvc.Tests
{
    [TestClass]
    public class PluploadFileCollectionTests
    {
        [TestMethod]
        public void Should_Initialize_Reference_With_Non_Empty_Guid_On_Construction()
        {
            var collection = new PluploadFileCollection();

            Assert.AreNotEqual(Guid.Empty, collection.Reference);
        }
    }
}

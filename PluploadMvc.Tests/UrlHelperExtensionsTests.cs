using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XperiCode.PluploadMvc.Tests
{
    [TestClass]
    public class UrlHelperExtensionsTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void PluploadHandler_Should_Throw_ArgumentNullException_When_Collection_Is_Null()
        {
            PluploadFileCollection collection = null;
            var helper = new UrlHelper(null);

            UrlHelperExtensions.PluploadHandler(helper, collection);
        }
    }
}

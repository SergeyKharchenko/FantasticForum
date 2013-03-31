using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Models;
using Moq;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ImageHelperTests
    {
        [Test]
        public void SaveTest()
        {
            var imageMock = new Mock<HttpPostedFileBase>();
            imageMock.Setup(image => image.FileName).Returns("love.jpg");

            var path = Directory.GetCurrentDirectory();

            var helper = new ImageHelper();
            var filePath = helper.Save(imageMock.Object, path, 4);

            imageMock.Verify(x => x.FileName, Times.Once());
            imageMock.Verify(image => image.SaveAs(filePath));
            Assert.That(filePath, Is.EqualTo(Path.Combine(path, "4.jpg")));
        }
    }
}

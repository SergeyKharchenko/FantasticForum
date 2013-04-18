using System.IO;
using System.Web;
using Moq;
using Mvc.Infrastructure.Concrete;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FileHelperTests
    {
        private FileHelper fileHelper;

        [SetUp]
        public void SetUp()
        {
            fileHelper = new FileHelper();
        }

        [Test]
        public void FileBaseToByteArrayTest()
        {
            var streamMock = new Mock<Stream>();

            var fileBaseMock = new Mock<HttpPostedFileBase>();
            fileBaseMock.Setup(fileBase => fileBase.ContentType).Returns("file/jpg");
            fileBaseMock.Setup(fileBase => fileBase.ContentLength).Returns(100);
            fileBaseMock.Setup(fileBase => fileBase.InputStream).Returns(streamMock.Object);

            var data = fileHelper.FileBaseToByteArray(fileBaseMock.Object);

            streamMock.Verify(stream => stream.Read(data, 0, 100), Times.Once());
        }
    }
}
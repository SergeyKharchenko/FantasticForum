using Moq;
using Mvc.Infrastructure.Assistants.Concrete;
using NUnit.Framework;
using System.IO;
using System.Web;

namespace Tests.Assistants
{
    [TestFixture]
    public class FileAssistantTests
    {
        private FileAssistant fileAssistant;

        [SetUp]
        public void SetUp()
        {
            fileAssistant = new FileAssistant();
        }

        [Test]
        public void FileBaseToByteArrayTest()
        {
            var streamMock = new Mock<Stream>();

            var fileBaseMock = new Mock<HttpPostedFileBase>();
            fileBaseMock.Setup(fileBase => fileBase.ContentType).Returns("file/jpg");
            fileBaseMock.Setup(fileBase => fileBase.ContentLength).Returns(100);
            fileBaseMock.Setup(fileBase => fileBase.InputStream).Returns(streamMock.Object);

            var data = fileAssistant.FileBaseToByteArray(fileBaseMock.Object);

            streamMock.Verify(stream => stream.Read(data, 0, 100), Times.Once());
        }
    }
}
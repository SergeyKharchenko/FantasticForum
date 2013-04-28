using Models;
using MongoDB.Bson;
using Moq;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.Assistants.Concrete;
using Mvc.Infrastructure.DAL.Abstract;
using NUnit.Framework;
using System.Web;

namespace Tests.Assistants
{
    [TestFixture]
    public class EntityWithImageAssistantTests
    {
        private IEntityWithImageAssistant<Section> imageAssistant;
        private Mock<IRepository<Image>> imageMongoRepositoryMock;
        private Mock<IRepository<Section>> sectionRepositoryMock;
        private Mock<IFileAssistant> fileAssistantMock;

        [SetUp]
        public void SetUp()
        {
            imageMongoRepositoryMock = new Mock<IRepository<Image>>();
            sectionRepositoryMock = new Mock<IRepository<Section>>();
            fileAssistantMock = new Mock<IFileAssistant>();

            imageAssistant = new EntityWithImageAssistant<Section>(sectionRepositoryMock.Object,
                                                                   imageMongoRepositoryMock.Object,
                                                                   fileAssistantMock.Object);
        }

        [Test]
        public void CreateImageTest()
        {
            var imageMock = new Mock<HttpPostedFileBase>();
            imageMock.Setup(image => image.ContentType).Returns("file/jpg");
            imageMock.Setup(image => image.ContentLength).Returns(100);

            var imageData = new byte[] { 1, 2, 3 };
            fileAssistantMock.Setup(helper => helper.FileBaseToByteArray(imageMock.Object)).Returns(imageData);

            var objectId = new ObjectId("1234567890ab1234567890ab");
            imageMongoRepositoryMock.Setup(repo => repo.Create(It.IsAny<Image>()))
                .Callback((Image image) => image.Id = objectId)
                .Returns((Image image) => image);

            var imageId = imageAssistant.CreateImage(imageMock.Object);

            fileAssistantMock.Verify(helper => helper.FileBaseToByteArray(imageMock.Object), Times.Once());
            imageMongoRepositoryMock.Verify(
                repo =>
                repo.Create(
                    It.Is((Image image) => image.Data.Equals(imageData) && image.ImageMimeType.Equals("file/jpg"))), Times.Once());
            Assert.That(imageId, Is.EqualTo(objectId.ToString()));
        }

        [Test]
        public void RemoveImageTest()
        {
            var section = new Section {Id = 1, ImageId = "123"};

            imageAssistant.RemoveImageFrom(section);

            imageMongoRepositoryMock.Verify(repo => repo.Remove("123"), Times.Once());
        }

        [Test]
        public void GetImageIdTest()
        {
            sectionRepositoryMock.Setup(repo => repo.GetById(42)).Returns(new Section {ImageId = "123"});

            var imageId = imageAssistant.GetImageId(42);

            sectionRepositoryMock.Verify(repo => repo.GetById(42), Times.Once());
            Assert.That(imageId, Is.EqualTo("123"));
        }

        [Test]
        public void GetImageFromEntityWithIdTest()
        {
            var section = new Section { ImageId = "42" };
            sectionRepositoryMock.Setup(repo => repo.GetById(1)).Returns(section);
            var image = new Image {Data = new byte[] {1, 2, 3}, ImageMimeType = "file"};
            imageMongoRepositoryMock.Setup(repo => repo.GetById("42"))
                .Returns(image);

            var imageUtilityModel = imageAssistant.GetImageFromEntityWithId(1);

            sectionRepositoryMock.Verify(repo => repo.GetById(1), Times.Once());
            imageMongoRepositoryMock.Verify(repo => repo.GetById("42"), Times.Once());
            Assert.That(imageUtilityModel.HasImage, Is.True);
            Assert.That(imageUtilityModel.ImageMimeType, Is.EqualTo(image.ImageMimeType));
            Assert.That(imageUtilityModel.Data, Is.EqualTo(image.Data));
        }

        [Test]
        public void GetImageFromEntityWithIdFailTest()
        {
            var section = new Section();
            sectionRepositoryMock.Setup(repo => repo.GetById(1)).Returns(section);

            var imageUtilityModel = imageAssistant.GetImageFromEntityWithId(1);

            sectionRepositoryMock.Verify(repo => repo.GetById(1), Times.Once());
            Assert.That(imageUtilityModel.HasImage, Is.False);
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Web;
using Models;
using MongoDB.Bson;
using Moq;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using NUnit.Framework;

namespace Tests.UnitOfWork
{
    [TestFixture]
    public class SectionTests
    {
        private SectionUnitOfWork unitOfWork;
        private Mock<IMongoRepository<Image>> imageMongoRepositoryMock;
        private Mock<IRepository<Section>> sectionRepositoryMock;
        private Mock<IFileHelper> fileHelperMock;
        private Collection<Section> sections;
            
        [SetUp]
        public void SetUp()
        {
            imageMongoRepositoryMock = new Mock<IMongoRepository<Image>>(); 
            sections = new Collection<Section>
                {
                    new Section {Id = 1, Title = "Sport"},
                    new Section {Id = 2, Title = "Life"},
                    new Section {Id = 3, Title = "Programming"}
                };
            sectionRepositoryMock = new Mock<IRepository<Section>>();
            sectionRepositoryMock.Setup(repo => repo.Entities).Returns(sections);
            fileHelperMock = new Mock<IFileHelper>();
            unitOfWork = new SectionUnitOfWork(sectionRepositoryMock.Object,
                                               imageMongoRepositoryMock.Object,
                                               fileHelperMock.Object);
        }

        [Test]
        public void GetAllSectionsTest()
        {
            var actualSections = unitOfWork.Section;

            sectionRepositoryMock.Verify(repo => repo.Entities, Times.Once());
            Assert.That(actualSections, Is.EquivalentTo(sections));
        }

        [Test]
        public void CreateSectionTest()
        {
            var imageMock = new Mock<HttpPostedFileBase>();
            imageMock.Setup(image => image.ContentType).Returns("file/jpg");
            imageMock.Setup(image => image.ContentLength).Returns(100);

            var imageData = new byte[] {1, 2, 3};
            fileHelperMock.Setup(helper => helper.FileBaseToByteArray(imageMock.Object)).Returns(imageData);

            var objectId = new ObjectId("1234567890ab1234567890ab");
            imageMongoRepositoryMock.Setup(repo => repo.Create(It.IsAny<Image>())).Callback((Image image) => image.Id = objectId);

            var section = new Section {Title = "Love"};


            unitOfWork.Create(section, imageMock.Object);


            fileHelperMock.Verify(helper => helper.FileBaseToByteArray(imageMock.Object), Times.Once());
            imageMongoRepositoryMock.Verify(
                repo =>
                repo.Create(
                    It.Is((Image image) => image.Data.Equals(imageData) && image.ImageMimeType.Equals("file/jpg"))), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.Create(section), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once());
            Assert.That(section.ImageId, Is.EqualTo(objectId.ToString()));
        }

        [Test]
        public void CreateSectionWithEmptyImageDBTest()
        {
            var section = new Section { Title = "Love" };

            unitOfWork.Create(section, null);

            sectionRepositoryMock.Verify(repo => repo.Create(section), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once());
            imageMongoRepositoryMock.Verify(repo => repo.Create(It.IsAny<Image>()), Times.Never());
        }

        [Test]
        public void GetAvatarSuccessTest()
        {
            const int sectionId = 1;
            var section = new Section {ImageId = "1"};
            sectionRepositoryMock.Setup(repo => repo.GetById(sectionId)).Returns(section);
            var data = new byte[] {1, 2, 3};
            imageMongoRepositoryMock.Setup(repo => repo.Get("1"))
                .Returns(new Image { Data = data, ImageMimeType = "file" });

            var getAvatarSM = unitOfWork.GetAvatar(sectionId);

            Assert.That(getAvatarSM.HasAvatar, Is.True);
            sectionRepositoryMock.Verify(repo => repo.GetById(sectionId), Times.Once());
            imageMongoRepositoryMock.Verify(repo => repo.Get("1"), Times.Once());
            Assert.That(getAvatarSM.AvatarData, Is.EquivalentTo(data));
            Assert.That(getAvatarSM.ImageMimeType, Is.EqualTo("file"));
        }

        [Test]
        public void GetAvatarUnsuccessTest()
        {
            const int sectionId = 1;
            var section = new Section {ImageId = ""};
            sectionRepositoryMock.Setup(repo => repo.GetById(sectionId)).Returns(section);

            var getAvatarSM = unitOfWork.GetAvatar(sectionId);

            Assert.That(getAvatarSM.HasAvatar, Is.False);
            sectionRepositoryMock.Verify(repo => repo.GetById(sectionId), Times.Once());
            imageMongoRepositoryMock.Verify(repo => repo.Get("1"), Times.Never());
        }
    }
}
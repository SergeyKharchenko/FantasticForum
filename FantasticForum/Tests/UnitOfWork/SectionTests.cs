using System.Collections.ObjectModel;
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
        private ISectionUnitOfWork unitOfWork;
        private Mock<IRepository<Image>> imageMongoRepositoryMock;
        private Mock<IRepository<Section>> sectionRepositoryMock;
        private Mock<IFileHelper> fileHelperMock;
        private Collection<Section> sections;
            
        [SetUp]
        public void SetUp()
        {
            imageMongoRepositoryMock = new Mock<IRepository<Image>>(); 
            sections = new Collection<Section>
                {
                    new Section {Id = 1, Title = "Sport", ImageId = "abcd"},
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
        public void GetSectionByIdTest()
        {
            sectionRepositoryMock.Setup(repo => repo.GetById(1))
                .Returns(new Section {Id = 1, Title = "Games"});

            var section = unitOfWork.GetSectionById(1);

            sectionRepositoryMock.Verify(repo => repo.GetById(1), Times.Once());
            Assert.That(section.Id, Is.EqualTo(1));
            Assert.That(section.Title, Is.EqualTo("Games"));
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
            imageMongoRepositoryMock.Setup(repo => repo.CreateOrUpdate(It.IsAny<Image>())).Callback((Image image) => image.Id = objectId);

            var section = new Section {Id = 42, Title = "Love"};
            sectionRepositoryMock.Setup(repo => repo.GetById(42)).Returns(section);

            unitOfWork.CreateOrUpdateSection(section, imageMock.Object);


            fileHelperMock.Verify(helper => helper.FileBaseToByteArray(imageMock.Object), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once());
            imageMongoRepositoryMock.Verify(repo => repo.Remove(objectId.ToString()), Times.Never());
            imageMongoRepositoryMock.Verify(
                repo =>
                repo.CreateOrUpdate(
                    It.Is((Image image) => image.Data.Equals(imageData) && image.ImageMimeType.Equals("file/jpg"))), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.CreateOrUpdate(section), Times.Once());
            Assert.That(section.ImageId, Is.EqualTo(objectId.ToString()));
        }

        [Test]
        public void UpdateSectionTest()
        {
            var imageMock = new Mock<HttpPostedFileBase>();
            imageMock.Setup(image => image.ContentType).Returns("file/jpg");
            imageMock.Setup(image => image.ContentLength).Returns(100);

            var imageData = new byte[] {1, 2, 3};
            fileHelperMock.Setup(helper => helper.FileBaseToByteArray(imageMock.Object)).Returns(imageData);

            var objectId = new ObjectId("1234567890ab1234567890ab");
            imageMongoRepositoryMock.Setup(repo => repo.CreateOrUpdate(It.IsAny<Image>())).Callback((Image image) => image.Id = objectId);

            var section = new Section { Id = 42, Title = "Love", ImageId = "1234567890ab1234567890cc" };
            sectionRepositoryMock.Setup(repo => repo.GetById(42)).Returns(section);


            unitOfWork.CreateOrUpdateSection(section, imageMock.Object);


            fileHelperMock.Verify(helper => helper.FileBaseToByteArray(imageMock.Object), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.GetById(42), Times.Once());
            imageMongoRepositoryMock.Verify(repo => repo.Remove("1234567890ab1234567890cc"), Times.Once());
            imageMongoRepositoryMock.Verify(
                repo =>
                repo.CreateOrUpdate(
                    It.Is((Image image) => image.Data.Equals(imageData) && image.ImageMimeType.Equals("file/jpg"))), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.CreateOrUpdate(section), Times.Once());
            Assert.That(section.ImageId, Is.EqualTo(objectId.ToString()));
        }

        [Test]
        public void CreateSectionWithEmptyImageDBTest()
        {
            var section = new Section {Id = 42, Title = "Love"};
            sectionRepositoryMock.Setup(repo => repo.GetById(42)).Returns(section);

            unitOfWork.CreateOrUpdateSection(section, null);

            sectionRepositoryMock.Verify(repo => repo.GetById(42), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.CreateOrUpdate(section), Times.Once());
            imageMongoRepositoryMock.Verify(repo => repo.CreateOrUpdate(It.IsAny<Image>()), Times.Never());
        }

        [Test]
        public void GetAvatarSuccessTest()
        {
            const int sectionId = 1;
            var section = new Section {ImageId = "1"};
            sectionRepositoryMock.Setup(repo => repo.GetById(sectionId)).Returns(section);
            var data = new byte[] {1, 2, 3};
            imageMongoRepositoryMock.Setup(repo => repo.GetById("1"))
                .Returns(new Image { Data = data, ImageMimeType = "file" });

            var getAvatarSM = unitOfWork.GetAvatar(sectionId);

            Assert.That(getAvatarSM.HasAvatar, Is.True);
            sectionRepositoryMock.Verify(repo => repo.GetById(sectionId), Times.Once());
            imageMongoRepositoryMock.Verify(repo => repo.GetById("1"), Times.Once());
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
            imageMongoRepositoryMock.Verify(repo => repo.GetById("1"), Times.Never());
        }

        [Test]
        public void RemoveSectionWithAvatarTest()
        {
            sectionRepositoryMock.Setup(repo => repo.GetById(1))
                .Returns(new Section { Id = 1, Title = "Sport", ImageId = "abcd" });

            unitOfWork.RemoveSection(1);

            imageMongoRepositoryMock.Verify(repo => repo.Remove("abcd"), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.Remove(1), Times.Once());
        }

        [Test]
        public void RemoveSectionWithoutAvatarTest()
        {
            sectionRepositoryMock.Setup(repo => repo.GetById(1))
                .Returns(new Section { Id = 1, Title = "Sport" });

            unitOfWork.RemoveSection(1);

            imageMongoRepositoryMock.Verify(repo => repo.Remove("abcd"), Times.Never());
            sectionRepositoryMock.Verify(repo => repo.Remove(1), Times.Once());
        }
    }
}
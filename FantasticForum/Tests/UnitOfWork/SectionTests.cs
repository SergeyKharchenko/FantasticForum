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
        private SectionUnitOfWork unitOfWork;
        private Mock<IRepository<Section>> sectionRepositoryMock;
        private Mock<IEntityWithImageAssistant<Section>> imageAssistantMock;
        private Collection<Section> sections;
            
        [SetUp]
        public void SetUp()
        {
            sections = new Collection<Section>
                {
                    new Section {Id = 1, Title = "Sport", ImageId = "abcd"},
                    new Section {Id = 2, Title = "Life"},
                    new Section {Id = 3, Title = "Programming"}
                };
            sectionRepositoryMock = new Mock<IRepository<Section>>();
            sectionRepositoryMock.Setup(repo => repo.Entities).Returns(sections);
            imageAssistantMock = new Mock<IEntityWithImageAssistant<Section>>();
            unitOfWork = new SectionUnitOfWork(null,
                                               sectionRepositoryMock.Object,
                                               imageAssistantMock.Object);
        }

        [Test]
        public void CreateSectionTest()
        {
            var imageMock = new Mock<HttpPostedFileBase>();
            var section = new Section {Title = "Love"};
            imageAssistantMock.Setup(assistant => assistant.CreateImage(imageMock.Object))
                              .Returns("123");

            unitOfWork.CreateOrUpdateSection(section, imageMock.Object);

            imageAssistantMock.Verify(assistant => assistant.CreateImage(imageMock.Object), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.Create(section), Times.Once());
            Assert.That(section.ImageId, Is.EqualTo("123"));
        }

        [Test]
        public void CreateSectionWithoutAvatarTest()
        {
            var section = new Section { Title = "Love" };

            unitOfWork.CreateOrUpdateSection(section, null);

            imageAssistantMock.Verify(assistant => assistant.CreateImage(It.IsAny<HttpPostedFileBase>()), Times.Never());
            sectionRepositoryMock.Verify(repo => repo.Create(section), Times.Once());
            Assert.That(section.ImageId, Is.Null);
        }

        [Test]
        public void UpdateSectionTest()
        {
            var imageMock = new Mock<HttpPostedFileBase>();
            var section = new Section { Id = 1, Title = "Love" };
            imageAssistantMock.Setup(assistant => assistant.CreateImage(imageMock.Object))
                              .Returns("new");
            imageAssistantMock.Setup(assistant => assistant.GetImageId(1))
                              .Returns("old");
            imageAssistantMock.Setup(assistant => assistant.RemoveImageFrom(section));
            sectionRepositoryMock.Setup(repo => repo.Update(section)).Returns(section);

            var crudResult = unitOfWork.CreateOrUpdateSection(section, imageMock.Object);

            imageAssistantMock.Verify(assistant => assistant.CreateImage(imageMock.Object), Times.Once());
            imageAssistantMock.Verify(assistant => assistant.GetImageId(1), Times.Once());
            imageAssistantMock.Verify(assistant => assistant.RemoveImageFrom(section), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.Update(section), Times.Once());
            Assert.That(crudResult.Success, Is.True);
            Assert.That(crudResult.Entity.ImageId, Is.EqualTo("new"));

        }

        [Test]
        public void UpdateSectionWithoutAvatarTest()
        {
            var section = new Section { Id = 1, Title = "Love" };
            imageAssistantMock.Setup(assistant => assistant.GetImageId(1))
                              .Returns("old");
            sectionRepositoryMock.Setup(repo => repo.Update(section)).Returns(section);

            var crudResult = unitOfWork.CreateOrUpdateSection(section, null);

            imageAssistantMock.Verify(assistant => assistant.CreateImage(It.IsAny<HttpPostedFileBase>()), Times.Never());
            imageAssistantMock.Verify(assistant => assistant.GetImageId(1), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.Update(section), Times.Once());
            Assert.That(crudResult.Success, Is.True);
            Assert.That(crudResult.Entity.ImageId, Is.EqualTo("old"));
        }

        [Test]
        public void GetAvatarTest()
        {
            unitOfWork.GetAvatar(1);

            imageAssistantMock.Verify(assistant => assistant.GetImageFromEntityWithId(1), Times.Once());
        }

        [Test]
        public void RemoveSectionWithAvatarTest()
        {
            var section = new Section { Id = 1, };
            sectionRepositoryMock.Setup(repo => repo.GetById(1)).Returns(section);
            imageAssistantMock.Setup(assistant => assistant.RemoveImageFrom(section));

            unitOfWork.RemoveSection(section);

            sectionRepositoryMock.Verify(repo => repo.Remove(section), Times.Once());
        }
    }
}
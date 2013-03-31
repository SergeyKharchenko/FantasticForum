using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web;
using Models;
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
        private Mock<IImageHelper> imageHelperMock;
        private Mock<IRepository<Section>> sectionRepositoryMock;
        private Mock<IRepository<Image>> imageRepositoryMock;
        private Collection<Section> sections;
            
        [SetUp]
        public void SetUp()
        {
            imageHelperMock = new Mock<IImageHelper>();
            sections = new Collection<Section>
                {
                    new Section {Id = 1, Title = "Sport"},
                    new Section {Id = 2, Title = "Life"},
                    new Section {Id = 3, Title = "Programming"}
                };
            sectionRepositoryMock = new Mock<IRepository<Section>>();
            sectionRepositoryMock.Setup(repo => repo.Entities).Returns(sections);
            imageRepositoryMock = new Mock<IRepository<Image>>();
            unitOfWork = new SectionUnitOfWork(imageHelperMock.Object, sectionRepositoryMock.Object, imageRepositoryMock.Object);
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
            imageRepositoryMock.Setup(repo => repo.Entities).Returns(new List<Image>
                {
                    new Image {Id = 1},
                    new Image {Id = 2},
                    new Image {Id = 3},
                });

            var imageMock = new Mock<HttpPostedFileBase>();
            imageMock.Setup(image => image.FileName).Returns("love.jpg");
            imageMock.Setup(image => image.ContentType).Returns("file/jpg");
            imageHelperMock.Setup(helper => helper.Save(imageMock.Object, @"c:\work\app_data", 4)).Returns(@"c:\work\app_data\4.jpg");

            var section = new Section {Title = "Love"};            

            unitOfWork.Create(section, imageMock.Object, @"c:\work\app_data", "/Images/Section/");

            imageHelperMock.Verify(helper => helper.Save(imageMock.Object, @"c:\work\app_data", 4), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.Create(section), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once());
            Assert.That(section.Image.FileName, Is.EqualTo("/Images/Section/4.jpg"));
            Assert.That(section.Image.ImageMimeType, Is.EqualTo("file/jpg"));
        }

        [Test]
        public void CreateSectionWithEmptyImageDBTest()
        {
            imageRepositoryMock.Setup(repo => repo.Entities).Returns(new List<Image>());

            var imageMock = new Mock<HttpPostedFileBase>();
            imageMock.Setup(image => image.FileName).Returns("love.jpg");
            imageMock.Setup(image => image.ContentType).Returns("file/jpg");
            imageHelperMock.Setup(helper => helper.Save(imageMock.Object, @"c:\work\app_data", 1)).Returns(@"c:\work\app_data\1.jpg");

            var section = new Section {Title = "Love"};            

            unitOfWork.Create(section, imageMock.Object, @"c:\work\app_data", "/Images/Section/");

            imageHelperMock.Verify(helper => helper.Save(imageMock.Object, @"c:\work\app_data", 1), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.Create(section), Times.Once());
            sectionRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once());
            Assert.That(section.Image.FileName, Is.EqualTo("/Images/Section/1.jpg"));
            Assert.That(section.Image.ImageMimeType, Is.EqualTo("file/jpg"));
        }

    }
}
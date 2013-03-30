using System.Collections.ObjectModel;
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
        private Mock<IRepository<Section>> repositoryMock;
        private Collection<Section> sections;
            
        [SetUp]
        public void SetUp()
        {
            sections = new Collection<Section>
                {
                    new Section {Title = "Sport"},
                    new Section {Title = "Life"},
                    new Section {Title = "Programming"}
                };
            repositoryMock = new Mock<IRepository<Section>>();
            repositoryMock.Setup(repo => repo.Entities).Returns(sections);
            unitOfWork = new SectionUnitOfWork(repositoryMock.Object);
        }

        [Test]
        public void SectionsTest()
        {
            var actualSections = unitOfWork.Sections;

            repositoryMock.Verify(repo => repo.Entities, Times.Once());
            Assert.That(actualSections, Is.EquivalentTo(sections));
        }

    }
}
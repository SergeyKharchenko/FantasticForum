using System.Collections.Generic;
using System.Collections.ObjectModel;
using Models;
using Moq;
using Mvc.Controllers;
using Mvc.Infrastructure.Abstract;
using Mvc.ViewModels;
using NUnit.Framework;

namespace Tests.Controllers
{
    [TestFixture]
    public class SectionTests
    {
        private SectionController controller;
        private Mock<ISectionUnitOfWork> unitOfWorkMock;
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
            unitOfWorkMock = new Mock<ISectionUnitOfWork>();
            unitOfWorkMock.Setup(unit => unit.Section).Returns(sections);
            controller = new SectionController(unitOfWorkMock.Object);
        }

        [Test]
        public void ListTest()
        {
            var view = controller.List();
            var actualSections = view.Model as List<SectionListVM>;

            unitOfWorkMock.Verify(unit => unit.Section, Times.Once());
            Assert.That(actualSections, Is.Not.Null);
            Assert.That(actualSections, Is.EquivalentTo(actualSections));
        }
    }
}
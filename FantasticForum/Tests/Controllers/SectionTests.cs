using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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

        [Test]
        public void CreateTest()
        {
            var httpContextMock = new Mock<HttpContextBase>();
            var serverMock = new Mock<HttpServerUtilityBase>();
            const string virtualPath = "~/Images/Section";
            serverMock.Setup(x => x.MapPath(virtualPath)).Returns(@"c:\work\app_data");
            httpContextMock.Setup(x => x.Server).Returns(serverMock.Object);
            controller.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), controller);

            var httpPostedFileBaseMock = new Mock<HttpPostedFileBase>();

            var section = new Section();

            var view = controller.Create(section, httpPostedFileBaseMock.Object);
            var redirectResult = view as RedirectToRouteResult;

            unitOfWorkMock.Verify(unit => unit.Create(section, httpPostedFileBaseMock.Object, @"c:\work\app_data", virtualPath.Substring(1)));
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("List"));
            
        }
    }
}
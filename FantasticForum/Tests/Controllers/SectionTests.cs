using Models;
using Moq;
using Mvc.Controllers;
using Mvc.Infrastructure;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.Concrete;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using Mvc.UtilityModels;
using Mvc.ViewModels;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tests.Controllers
{    
    [TestFixture]
    public class SectionTests
    {
        private SectionController controller;
        private Mock<AbstractSectionUnitOfWork> unitOfWorkMock;
        private Mock<IFileAssistant> fileHelperMock;
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
            unitOfWorkMock = new Mock<AbstractSectionUnitOfWork>(null, null);
            unitOfWorkMock.Setup(unit => unit.Entities).Returns(sections);

            fileHelperMock = new Mock<IFileAssistant>();

            controller = new SectionController(unitOfWorkMock.Object, fileHelperMock.Object, new CommonMapper());
        }

        [Test]
        public void ListTest()
        {
            var view = controller.List();
            var actualSections = view.Model as IEnumerable<SectionViewModel>;

            unitOfWorkMock.Verify(unit => unit.Entities, Times.Once());
            Assert.That(actualSections, Is.Not.Null);
        }
        
        [Test]
        public void CreateTest()
        {
            var section = new Section();

            var httpPostedFileBaseMock = new Mock<HttpPostedFileBase>();
            unitOfWorkMock.Setup(unit => unit.CreateOrUpdateSection(section, httpPostedFileBaseMock.Object))
                .Returns(new CrudUtilityModel<Section>(true, section));            

            var view = controller.Edit(section, httpPostedFileBaseMock.Object);
            var redirectResult = view as RedirectToRouteResult;

            unitOfWorkMock.Verify(unit => unit.CreateOrUpdateSection(section, httpPostedFileBaseMock.Object));
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("List"));
        }
        
        [Test]
        public void UpdateWithConcurrencyExceptionTest()
        {
            var httpPostedFileBaseMock = new Mock<HttpPostedFileBase>();
            var section = new Section();
            unitOfWorkMock.Setup(unit => unit.CreateOrUpdateSection(section, httpPostedFileBaseMock.Object))
                          .Returns(new CrudUtilityModel<Section>(false, section));

            var view = controller.Edit(section, httpPostedFileBaseMock.Object);
            var redirectResult = view as ViewResult;

            unitOfWorkMock.Verify(unit => unit.CreateOrUpdateSection(section, httpPostedFileBaseMock.Object));
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Model, Is.EqualTo(section));
        }

        
        [Test]
        public void ShowEditPageTest()
        {
            unitOfWorkMock.Setup(unit => unit.Read(1))
                .Returns(new Section { Id = 1, Title = "Games" });

            var view = controller.Edit(1);
            var section = view.Model as Section;

            unitOfWorkMock.Verify(unit => unit.Read(1), Times.Once());
            Assert.That(section, Is.Not.Null);
            Assert.That(section.Id, Is.EqualTo(1));
            Assert.That(section.Title, Is.EqualTo("Games"));
        }
        
        [Test]
        public void GetAvatarSuccessTest()
        {
            var avatarData = new byte[] {1, 2, 3};
            unitOfWorkMock.Setup(unit => unit.GetAvatar(1)).Returns(new ImageUtilityModel(true, avatarData, "file"));

            var avatar = controller.GetAvatar(1);

            Assert.That(avatar.FileContents, Is.EquivalentTo(avatarData));
            Assert.That(avatar.ContentType, Is.EquivalentTo("file"));
        }
        
        [Test]
        public void GetAvatarUnsuccessTest()
        {
            var httpContextMock = new Mock<HttpContextBase>();
            var serverMock = new Mock<HttpServerUtilityBase>();
            const string virtualPath = "~/Images/Section/section-without-avatar.png";
            serverMock.Setup(x => x.MapPath(virtualPath)).Returns(@"c:\work\app_data");
            httpContextMock.Setup(x => x.Server).Returns(serverMock.Object);
            controller.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), controller);
            var avatarData = new byte[] {1, 2, 4};
            fileHelperMock.Setup(helper => helper.FileToByteArray(@"c:\work\app_data")).Returns(avatarData);
            unitOfWorkMock.Setup(unit => unit.GetAvatar(1)).Returns(new ImageUtilityModel(false));

            var avatar = controller.GetAvatar(1);

            Assert.That(avatar.FileContents, Is.EquivalentTo(avatarData));
            Assert.That(avatar.ContentType, Is.EquivalentTo("image/png"));
        }
        
        [Test]
        public void ShowRemovePageTest()
        {
            unitOfWorkMock.Setup(unit => unit.Read(1))
                .Returns(new Section{Id = 1, Title = "Games"});

            var view = controller.Remove(1, null);
            var section = view.Model as Section;

            unitOfWorkMock.Verify(unit => unit.Read(1), Times.Once());
            Assert.That(section, Is.Not.Null);
            Assert.That(section.Id, Is.EqualTo(1));
            Assert.That(section.Title, Is.EqualTo("Games"));
        }

        [Test]
        public void RemoveTest()
        {
            var section = new Section { Id = 2 };
            unitOfWorkMock.Setup(unit => unit.Delete(section)).Returns(new CrudUtilityModel<Section>(true, section));

            var redirectToRouteResult = controller.Remove(section);

            Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo("List"));
        }

        [Test]
        public void RemoveWithConcurrencyTest()
        {
            var section = new Section { Id = 2 };
            unitOfWorkMock.Setup(unit => unit.Delete(section)).Returns(new CrudUtilityModel<Section>(false, section));

            var redirectToRouteResult = controller.Remove(section);

            Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo("Remove"));
            Assert.That(redirectToRouteResult.RouteValues["id"], Is.EqualTo(2));
            Assert.That(redirectToRouteResult.RouteValues["concurrencyError"], Is.EqualTo(true));
        }
    }
    
}
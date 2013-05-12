using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Models;
using Moq;
using Mvc.Controllers;
using Mvc.Filters;
using Mvc.Infrastructure.Concrete;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using Mvc.ViewModels;
using NUnit.Framework;
using PagedList;

namespace Tests.Controllers
{
    [TestFixture]
    public class RecordTests
    {
        private RecordController controller;
        private Mock<ISqlCrudUnitOfWork<Record>> unitOfWorkMock;
            
        [SetUp]
        public void SetUp()
        {
            unitOfWorkMock = new Mock<ISqlCrudUnitOfWork<Record>>();
            controller = new RecordController(unitOfWorkMock.Object, new CommonMapper());
        }

        [Test]
        public void ListTest()
        {
            var view = controller.List(1, 2, null);

            Assert.That(view.Model, Is.TypeOf<RecordsViewModel>());
            Assert.That((view.Model as RecordsViewModel).SectionId, Is.EqualTo(1));
            Assert.That((view.Model as RecordsViewModel).TopicId, Is.EqualTo(2));
        }

        [Test]
        public void AddTest()
        {
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(context => context.User).Returns(new UserIndentity {User = new User {Id = 42}});
            controller.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), controller);

            var view = controller.Add(1, 2, "Hello");

            unitOfWorkMock.Verify(
                unit =>
                unit.Create(It.Is<Record>(r => r.Text == "Hello" && r.CreationDate <= DateTime.Now && r.UserId == 42)));
            Assert.That(view.RouteValues["action"], Is.EqualTo("List"));
            Assert.That(view.RouteValues["sectionId"], Is.EqualTo(1));
            Assert.That(view.RouteValues["topicId"], Is.EqualTo(2));
        }
    }
}
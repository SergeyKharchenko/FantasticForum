using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Models;
using Moq;
using Mvc.Controllers;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using Mvc.UtilityModels;
using Mvc.ViewModels;
using NUnit.Framework;

namespace Tests.Controllers
{
    [TestFixture]
    public class TopicTests
    {
        private TopicController controller;
        private Mock<ISqlCrudUnitOfWork<Topic>> unitOfWorkMock;
            
        [SetUp]
        public void SetUp()
        {
            unitOfWorkMock = new Mock<ISqlCrudUnitOfWork<Topic>>();
            controller = new TopicController(unitOfWorkMock.Object, new CommonMapper());
        }

        [Test]
        public void ListTest()
        {
            var expression = (Expression<Func<Topic, bool>>) (topic => topic.SectionId == 1);
            unitOfWorkMock.Setup(unit => unit.Read(expression, ""))
                          .Returns(new List<Topic>());

            var view = controller.List(1);
            var actualTopics = view.Model as IEnumerable<TopicViewModel>;            

            Assert.That(actualTopics, Is.Not.Null);
            Assert.That(view.ViewData["SectionId"], Is.EqualTo(1));
        }

        [Test]
        public void ShowCreatePageTest()
        {
            var view = controller.Create(1);
            var model = view.Model as Topic;

            Assert.That(model, Is.Not.Null);
            Assert.That(model.SectionId, Is.EqualTo(1));
        }

        [Test]
        public void CreateTest()
        {
            var topic = new Topic {SectionId = 1};

            var view = controller.Edit(topic);
            var redirectResult = view as RedirectToRouteResult;

            unitOfWorkMock.Verify(unit => unit.Create(topic), Times.Once());
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("List"));
            Assert.That(redirectResult.RouteValues["sectionId"], Is.EqualTo(1));
        }
    }
}
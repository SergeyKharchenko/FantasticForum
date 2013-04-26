using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Web.Mvc;
using Models;
using Moq;
using Mvc.Controllers;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using Mvc.UtilityModels;
using Mvc.ViewModels;
using NUnit.Framework;
using System.Linq;

namespace Tests.Controllers
{
    [TestFixture]
    public class TopicTests
    {
        private TopicController controller;
        private Mock<AbstractTopicUnitOfWork> unitOfWorkMock;
            
        [SetUp]
        public void SetUp()
        {
            unitOfWorkMock = new Mock<AbstractTopicUnitOfWork>(null, null);
            controller = new TopicController(unitOfWorkMock.Object, new CommonMapper());
        }

        [Test]
        public void ListTest()
        {
            const int sectionId = 1;
            unitOfWorkMock.Setup(unit => unit.Read(It.IsAny<Expression<Func<Topic, bool>>>(), ""))
                          .Returns(new List<Topic>
                              {
                                  new Topic {Records = new Collection<Record> {new Record()}}
                              });

            var view = controller.List(sectionId);
            var actualTopics = view.Model as IEnumerable<TopicViewModel>;            

            Assert.That(actualTopics, Is.Not.Null);
            Assert.That(actualTopics.Count(), Is.EqualTo(1));
            Assert.That(actualTopics.First().RecordCount, Is.EqualTo(1));
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

            var view = controller.Create(topic);
            var redirectResult = view as RedirectToRouteResult;

            unitOfWorkMock.Verify(unit => unit.Create(topic), Times.Once());
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("List"));
            Assert.That(redirectResult.RouteValues["sectionId"], Is.EqualTo(1));
        }

        [Test]
        public void ShowRemovePageTest()
        {
            unitOfWorkMock.Setup(unit => unit.Read(1)).Returns(new Topic {Id = 2, SectionId = 3});

            var view = controller.Remove(1, false);
            var model = view.Model as TopicViewModel;

            Assert.That(model, Is.Not.Null);
            Assert.That(model.Id, Is.EqualTo(2));
            Assert.That(view.ViewData["sectionId"], Is.EqualTo(3));
        }

        [Test]
        public void RemoveTest()
        {
            var topic = new Topic { Id = 2, SectionId = 3 };
            unitOfWorkMock.Setup(unit => unit.Delete(topic)).Returns(new CrudUtilityModel<Topic>(true, topic));

            var redirectToRouteResult = controller.Remove(topic);

            Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo("List"));
            Assert.That(redirectToRouteResult.RouteValues["sectionId"], Is.EqualTo(3));
        }

        [Test]
        public void RemoveWithConcurrencyTest()
        {
            var topic = new Topic {Id = 2};
            unitOfWorkMock.Setup(unit => unit.Delete(topic)).Returns(new CrudUtilityModel<Topic>(false, topic));

            var redirectToRouteResult = controller.Remove(topic);

            Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo("Remove"));
            Assert.That(redirectToRouteResult.RouteValues["id"], Is.EqualTo(2));
            Assert.That(redirectToRouteResult.RouteValues["concurrencyError"], Is.EqualTo(true));
        }
    }
}
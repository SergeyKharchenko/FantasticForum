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
            unitOfWorkMock = new Mock<AbstractTopicUnitOfWork>(null);
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
    }
}
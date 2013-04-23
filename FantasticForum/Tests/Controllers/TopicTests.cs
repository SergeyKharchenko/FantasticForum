using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Models;
using Moq;
using Mvc.Controllers;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
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
            unitOfWorkMock.Setup(unit => unit.Read(expression))
                          .Returns(new List<Topic>());

            var view = controller.List(1);
            var actualTopics = view.Model as IEnumerable<TopicViewModel>;

            Assert.That(actualTopics, Is.Not.Null);
        }
    }
}
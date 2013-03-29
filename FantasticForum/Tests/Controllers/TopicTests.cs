using System.Collections.ObjectModel;
using Models;
using Moq;
using Mvc.Controllers;
using Mvc.Infrastructure.Abstract;
using NUnit.Framework;

namespace Tests.Controllers
{
    [TestFixture]
    public class TopicTests
    {
        private TopicController controller;
        private Mock<IEntityUnitOfWork> unitOfWorkMock;
        private Collection<Topic> topics;

        [SetUp]
        public void SetUp()
        {
            topics = new Collection<Topic>
                {
                    new Topic {Title = "Sport"},
                    new Topic {Title = "Life"},
                    new Topic {Title = "Programming"}
                };
            unitOfWorkMock = new Mock<IEntityUnitOfWork>();
            unitOfWorkMock.Setup(unit => unit.Topics).Returns(topics);
            controller = new TopicController(unitOfWorkMock.Object);
        }

        [Test]
        public void ListTest()
        {
            var view = controller.List();
            var expectedTopics = view.Model as Collection<Topic>;

            unitOfWorkMock.Verify(unit => unit.Topics, Times.Once());
            Assert.That(expectedTopics, Is.Not.Null);
            Assert.That(expectedTopics, Is.EquivalentTo(expectedTopics));
        }
    }
}
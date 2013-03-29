using System.Collections.ObjectModel;
using Models;
using Moq;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using NUnit.Framework;

namespace Tests.UnitOfWork
{
    [TestFixture]
    public class TopicTests
    {
        private TopicUnitOfWork unitOfWork;
        private Mock<IRepository<Topic>> repositoryMock;
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
            repositoryMock = new Mock<IRepository<Topic>>();
            repositoryMock.Setup(repo => repo.Entities).Returns(topics);
            unitOfWork = new TopicUnitOfWork(repositoryMock.Object);
        }

        [Test]
        public void TopicsTest()
        {
            var expectedTopics = unitOfWork.Topics;

            repositoryMock.Verify(repo => repo.Entities, Times.Once());
            Assert.That(expectedTopics, Is.EquivalentTo(topics));
        }

    }
}
using System.Collections.Generic;
using Models;
using Moq;
using Mvc.Controllers;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using NUnit.Framework;

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
            controller = new RecordController(unitOfWorkMock.Object);
        }

        [Test]
        public void ListTest()
        {
            var view = controller.List(1, 2);

            Assert.That(view.Model, Is.TypeOf<List<Record>>());
            Assert.That(view.ViewBag.SectionId, Is.EqualTo(1));
            Assert.That(view.ViewBag.TopicId, Is.EqualTo(2));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Models;
using Moq;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.DAL.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Concrete;
using NUnit.Framework;
using System.Web;

namespace Tests.UnitsOfWork
{
    [TestFixture]
    public class UserTests
    {
        private UserUnitOfWork unitOfWork;
        private Mock<IRepository<User>> userRepositoryMock;
        private Mock<IEntityWithImageAssistant<User>> imageAssistantMock;

        [SetUp]
        public void SetUp()
        {
            userRepositoryMock = new Mock<IRepository<User>>();
            imageAssistantMock = new Mock<IEntityWithImageAssistant<User>>();

            unitOfWork = new UserUnitOfWork(null, userRepositoryMock.Object, imageAssistantMock.Object);
        }

        [Test]
        public void IsUserExistTest()
        {            
            var user = new User {Id = 42};
            userRepositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), ""))
                              .Returns(new List<User>());

            var isUserExist = unitOfWork.IsUserExist(user);

            userRepositoryMock.Verify(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), ""), Times.Once());
            Assert.That(isUserExist, Is.False);

        }

        [Test]
        public void RegisterTest()
        {            
            var user = new User {Id = 42};
            userRepositoryMock.Setup(repo => repo.Create(user)).Returns(user);
            var imageMock = new Mock<HttpPostedFileBase>();
            imageAssistantMock.Setup(assistant => assistant.CreateImage(imageMock.Object)).Returns("123");

            var createdUser = unitOfWork.RegisterUser(user, imageMock.Object);

            imageAssistantMock.Verify(assistant => assistant.CreateImage(imageMock.Object), Times.Once());
            userRepositoryMock.Verify(repo => repo.Create(user), Times.Once());
            Assert.That(createdUser.ImageId, Is.EqualTo("123"));
        }
    }
}
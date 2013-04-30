using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Models;
using Moq;
using Mvc.Controllers;
using Mvc.Infrastructure;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using Mvc.ViewModels;
using NUnit.Framework;
using System.Web;
using System.Web.Mvc;

namespace Tests.Controllers
{
    [TestFixture]
    public class AccountTests
    {
        private AccountController controller;
        private Mock<AbstractUserUnitOfWork> unitOfWorkMock;
        private Mock<IAuthorizationAssistant> authorizationAssistantMock;
        
        [SetUp]
        public void SetUp()
        {
            unitOfWorkMock = new Mock<AbstractUserUnitOfWork>(null, null);
            authorizationAssistantMock = new Mock<IAuthorizationAssistant>();
            controller = new AccountController(unitOfWorkMock.Object, authorizationAssistantMock.Object, new CommonMapper());
        }

        [Test]
        public void RegisterTest()
        {
            var user = new User {Id = 42};
            var imageMock = new Mock<HttpPostedFileBase>();
            unitOfWorkMock.Setup(unit => unit.RegisterUser(user, imageMock.Object)).Returns(user);
            
            var view = controller.Register(user, imageMock.Object);
            var redirectToRouteResult = view as RedirectToRouteResult;

            unitOfWorkMock.Verify(unit => unit.RegisterUser(user, imageMock.Object), Times.Once());
            authorizationAssistantMock.Verify(assistant => assistant.WriteAuthInfoInSession(controller.Session, 42));
            Assert.That(redirectToRouteResult, Is.Not.Null);
        }

        [Test]
        public void LoginTest()
        {
            var loginViewModel = new LoginViewModel {Email = "a@ew.com", Password = "123"};
            unitOfWorkMock.Setup(unit => unit.Read(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string>()))
                .Returns(new List<User> { new User { Id = 42} });

            controller.Login(loginViewModel);

            unitOfWorkMock.Verify(unit => unit.Read(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string>()), Times.Once());
            authorizationAssistantMock.Verify(assistant => assistant.WriteAuthInfoInSession(controller.Session, 42), Times.Once());
        }

        [Test]
        public void LoginFailTest()
        {
            var loginViewModel = new LoginViewModel {Email = "a@ew.com", Password = "123"};
            unitOfWorkMock.Setup(unit => unit.Read(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string>()))
                .Returns(new List<User> ());

            controller.Login(loginViewModel);

            unitOfWorkMock.Verify(unit => unit.Read(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string>()), Times.Once());
            authorizationAssistantMock.Verify(assistant => assistant.WriteAuthInfoInSession(controller.Session, 42), Times.Never());
        }
    }
}
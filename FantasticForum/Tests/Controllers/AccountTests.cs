using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Models;
using Moq;
using Mvc.Controllers;
using Mvc.Infrastructure;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.Concrete;
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
            controller = new AccountController(unitOfWorkMock.Object, authorizationAssistantMock.Object, null,
                                               new CommonMapper());
        }

        [Test]
        public void RegisterTest()
        {
            var registeredUser = new RegisterViewModel();
            var imageMock = new Mock<HttpPostedFileBase>();
            var user = new User {Id = 42};
            unitOfWorkMock.Setup(unit => unit.RegisterUser(It.IsAny<User>(), imageMock.Object))
                          .Returns(user);
            controller.TempData["returnUrl"] = "/abs";

            var view = controller.Register(registeredUser, imageMock.Object);
            var redirectResult = view as RedirectResult;

            Assert.That(redirectResult, Is.Not.Null);
            unitOfWorkMock.Verify(unit => unit.RegisterUser(It.IsAny<User>(), imageMock.Object), Times.Once());
            authorizationAssistantMock.Verify(assistant => assistant.WriteAuthInfoInSession(controller.Session, user));
        }

        [Test]
        public void LoginTest()
        {
            var loginViewModel = new LoginViewModel {Email = "a@ew.com", Password = "123"};
            var user = new User { Id = 42 };
            unitOfWorkMock.Setup(unit => unit.Read(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string>()))
                .Returns(new List<User> { user });
            controller.TempData["returnUrl"] = "/abs";

            var view = controller.Login(loginViewModel);
            var redirectResult = view as RedirectResult;

            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.EqualTo("/abs"));
            unitOfWorkMock.Verify(unit => unit.Read(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string>()), Times.Once());
            authorizationAssistantMock.Verify(assistant => assistant.WriteAuthInfoInSession(controller.Session, user), Times.Once());
        }

        [Test]
        public void LoginFailTest()
        {
            var loginViewModel = new LoginViewModel {Email = "a@ew.com", Password = "123"};
            unitOfWorkMock.Setup(unit => unit.Read(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string>()))
                .Returns(new List<User> ());
            controller.TempData["returnUrl"] = "/abs";

            var view = controller.Login(loginViewModel);
            var viewResult = view as ViewResult;

            Assert.That(viewResult, Is.Not.Null);
            unitOfWorkMock.Verify(unit => unit.Read(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string>()), Times.Once());
            authorizationAssistantMock.Verify(assistant => assistant.WriteAuthInfoInSession(controller.Session, It.IsAny<User>()), Times.Never());
        }

        [Test]
        public void LogoutTest()
        {
            var view = controller.Logout("/abs");
            var redirectResult = view as RedirectResult;

            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.EqualTo("/abs"));
            authorizationAssistantMock.Verify(assistant => assistant.RemoveAuthInfoFromSession(controller.Session),
                                              Times.Once());
        }
    }
}
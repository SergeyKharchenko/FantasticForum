using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Routing;
using Models;
using Moq;
using Mvc.App_Start;
using Mvc.Controllers;
using Mvc.Infrastructure;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.Concrete;
using Mvc.Infrastructure.Mailers;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using Mvc.Mailer;
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
        private Mock<IUserMailer> userMailertMock;
        private Mock<IUrlAssistant> urlAssistantMock;
        
        [SetUp]
        public void SetUp()
        {
            unitOfWorkMock = new Mock<AbstractUserUnitOfWork>(null, null);
            authorizationAssistantMock = new Mock<IAuthorizationAssistant>();
            userMailertMock = new Mock<IUserMailer>();
            urlAssistantMock = new Mock<IUrlAssistant>();
            controller = new AccountController(unitOfWorkMock.Object,
                                               authorizationAssistantMock.Object,
                                               null,
                                               userMailertMock.Object,
                                               urlAssistantMock.Object,
                                               new CommonMapper());
        }

        [Test]
        public void RegisterTest()
        {
            var registeredUser = new RegisterViewModel();
            var imageMock = new Mock<HttpPostedFileBase>();
            var user = new User {Id = 42, Email = "a@b.com", Guid = ""};
            unitOfWorkMock.Setup(unit => unit.IsUserExist(It.IsAny<User>()))
                          .Returns(false);
            unitOfWorkMock.Setup(unit => unit.RegisterUser(It.IsAny<User>(), imageMock.Object))
                          .Returns(user);
            urlAssistantMock.Setup(
                assistant =>
                assistant.GenerateAbsoluteUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>(), controller.Url))
                            .Returns("Hello");

            var messageMock = new Mock<MvcMailMessage>();
            userMailertMock.Setup(mailer => mailer.Register("a@b.com", It.IsAny<string>()))
                           .Returns(messageMock.Object);

            controller.Register(registeredUser, imageMock.Object);
            
            unitOfWorkMock.Verify(unit => unit.IsUserExist(It.IsAny<User>()), Times.Once());
            unitOfWorkMock.Verify(unit => unit.RegisterUser(It.IsAny<User>(), imageMock.Object), Times.Once());
            userMailertMock.Verify(mailer => mailer.Register("a@b.com", "Hello"), Times.Once());
            messageMock.Verify(message => message.Send(null), Times.Once());
        }

        [Test]
        public void RegistrationConfirmationTest()
        {
            var user = new User();
            unitOfWorkMock.Setup(unit => unit.Read(It.IsAny<Expression<Func<User, bool>>>(), ""))
                          .Returns(new List<User> { user });

            var view = controller.RegistrationConfirmation("Hello");

            Assert.That(view.Model, Is.EqualTo(true));
            unitOfWorkMock.Verify(unit => unit.Update(It.Is<User>(u => u.IsConfirmed)), Times.Once());
            unitOfWorkMock.Verify(unit => unit.Read(It.IsAny<Expression<Func<User, bool>>>(), ""), Times.Once());
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
            var view = controller.Logout();

            Assert.That(view.RouteValues["controller"], Is.EqualTo("Section"));
            Assert.That(view.RouteValues["action"], Is.EqualTo("List"));
            authorizationAssistantMock.Verify(assistant => assistant.RemoveAuthInfoFromSession(controller.Session),
                                              Times.Once());
        }
    }
}
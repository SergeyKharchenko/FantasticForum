using Models;
using Moq;
using Mvc.Controllers;
using Mvc.Infrastructure;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
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
    }
}
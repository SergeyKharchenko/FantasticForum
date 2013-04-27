using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Models;
using Moq;
using Mvc.Controllers;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using NUnit.Framework;

namespace Tests.Controllers
{
    [TestFixture]
    public class AccountTests
    {
        private AccountController controller;
        private Mock<AbstractUserUnitOfWork> unitOfWorkMock;
        
        [SetUp]
        public void SetUp()
        {
            unitOfWorkMock = new Mock<AbstractUserUnitOfWork>(null, null);
            controller = new AccountController(unitOfWorkMock.Object, new CommonMapper());
        }

        [Test]
        public void RegisterTest()
        {
            var user = new User();
            var imageMock = new Mock<HttpPostedFileBase>();
            var httpContext = new Mock<HttpContextBase>();
            var response = new Mock<HttpResponseBase>();
            var cookie = new HttpCookieCollection();
            response.Setup(res => res.Cookies).Returns(cookie);
            httpContext.Setup(context => context.Response).Returns(response.Object);
            controller.ControllerContext = new ControllerContext(httpContext.Object, new RouteData(), controller);
            unitOfWorkMock.Setup(unit => unit.RegisterUser(user, imageMock.Object)).Returns("123");
            
            var view = controller.Register(user, imageMock.Object);

            unitOfWorkMock.Verify(unit => unit.RegisterUser(user, imageMock.Object), Times.Once());
            Assert.That(cookie.AllKeys, Has.Member(ConfigurationManager.AppSettings.Get("Auth")));
            var authCookie = cookie.Get(ConfigurationManager.AppSettings.Get("Auth"));
            Assert.That(authCookie.Value, Is.EqualTo("123"));
        }
    }
}
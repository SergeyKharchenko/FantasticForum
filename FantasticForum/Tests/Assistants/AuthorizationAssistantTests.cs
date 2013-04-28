using System.Configuration;
using System.Web;
using System.Web.Security;
using Moq;
using Mvc.Infrastructure.Assistants.Concrete;
using NUnit.Framework;

namespace Tests.Assistants
{
    [TestFixture]
    public class AuthorizationAssistantTests
    {
        private AuthorizationAssistant assistant;

        [SetUp]
        public void SetUp()
        {
            assistant = new AuthorizationAssistant();
        }

        [Test]
        public void PlaceAuthInfoInCookieTest()
        {
            var response = new Mock<HttpResponseBase>();
            var cookie = new HttpCookieCollection();
            response.Setup(res => res.Cookies).Returns(cookie);

            assistant.PlaceAuthInfoInCookie(response.Object, 42);

            Assert.That(cookie.AllKeys, Has.Member(ConfigurationManager.AppSettings.Get("Auth")));
            var ticket = cookie.Get(ConfigurationManager.AppSettings.Get("Auth"));
            Assert.That(FormsAuthentication.Decrypt(ticket.Value).Name, Is.EqualTo("42"));
        }
    }
}
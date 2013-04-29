using Moq;
using Mvc.Infrastructure.Assistants.Concrete;
using Mvc.UtilityModels;
using NUnit.Framework;
using System.Configuration;
using System.Web;
using System.Web.Security;

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

            assistant.WriteAuthInfoInCookie(response.Object, 42);

            response.Verify(res => res.Cookies, Times.Once());
            Assert.That(cookie.AllKeys, Has.Member(ConfigurationManager.AppSettings.Get("Auth")));
            var ticket = cookie.Get(ConfigurationManager.AppSettings.Get("Auth"));
            Assert.That(FormsAuthentication.Decrypt(ticket.Value).Name, Is.EqualTo("42"));
        }

        private readonly object[] readAuthInfoFromCookieData = new object[]
            {
                new object[]
                    {
                        new FormsAuthenticationTicket("42",
                                                      false,
                                                      (int) FormsAuthentication.Timeout.TotalMinutes),
                        new AuthorizeUtilityModel {IsAuthorized = true, UserId = 42}
                    },
                new object[]
                    {
                        new FormsAuthenticationTicket("",
                                                      false,
                                                      (int) FormsAuthentication.Timeout.TotalMinutes),
                        new AuthorizeUtilityModel {IsAuthorized = false}
                    },
                new object[]
                    {
                        null, new AuthorizeUtilityModel {IsAuthorized = false}
                    }
            };

        [Test, TestCaseSource("readAuthInfoFromCookieData")]
        public void ReadAuthInfoFromCookieTest(FormsAuthenticationTicket ticket, AuthorizeUtilityModel authorizeUtilityModel)
        {
            var request = new Mock<HttpRequestBase>();
            var cookie = new HttpCookieCollection
                {
                    new HttpCookie(ConfigurationManager.AppSettings.Get("Auth"),
                                   ticket == null ? null : FormsAuthentication.Encrypt(ticket))
                };
            request.Setup(res => res.Cookies).Returns(cookie);

            var actuAlauthorizeUtilityModel = assistant.ReadAuthInfoFromCookie(request.Object);

            request.Verify(res => res.Cookies, Times.Once());
            Assert.That(actuAlauthorizeUtilityModel.IsAuthorized, Is.EqualTo(authorizeUtilityModel.IsAuthorized));
            if (actuAlauthorizeUtilityModel.IsAuthorized)
                Assert.That(actuAlauthorizeUtilityModel.UserId, Is.EqualTo(authorizeUtilityModel.UserId));
        }
    }
}
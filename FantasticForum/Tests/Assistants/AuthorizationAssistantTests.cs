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
            var session = new Mock<HttpSessionStateBase>();

            assistant.WriteAuthInfoInSession(session.Object, 42);

            session.Verify(s => s.Add(ConfigurationManager.AppSettings.Get("Auth"), 42), Times.Once());
        }

        private readonly object[] readAuthInfoFromCookieData = new object[]
            {
                new object[]
                    {
                        42, new AuthorizeUtilityModel {IsAuthorized = true, UserId = 42}
                    },
                new object[]
                    {
                        null, new AuthorizeUtilityModel {IsAuthorized = false}
                    },
            };

        [Test, TestCaseSource("readAuthInfoFromCookieData")]
        public void ReadAuthInfoFromCookieTest(object dataFromSession, AuthorizeUtilityModel authorizeUtilityModel)
        {
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(s => s[ConfigurationManager.AppSettings.Get("Auth")]).Returns(dataFromSession);

            var actuAlauthorizeUtilityModel = assistant.ReadAuthInfoFromSession(session.Object);

            session.Verify(s => s[ConfigurationManager.AppSettings.Get("Auth")], Times.Once());
            Assert.That(actuAlauthorizeUtilityModel.IsAuthorized, Is.EqualTo(authorizeUtilityModel.IsAuthorized));
            if (actuAlauthorizeUtilityModel.IsAuthorized)
                Assert.That(actuAlauthorizeUtilityModel.UserId, Is.EqualTo(authorizeUtilityModel.UserId));
        }
    }
}
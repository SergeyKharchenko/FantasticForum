using Models;
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
        public void PlaceAuthInfoInSessionTest()
        {
            var session = new Mock<HttpSessionStateBase>();

            var user = new User {Id = 42};
            assistant.WriteAuthInfoInSession(session.Object, user);

            session.Verify(s => s.Add(ConfigurationManager.AppSettings.Get("Auth"), user), Times.Once());
        }

        private readonly object[] readAuthInfoFromSessionData = new object[]
            {
                new User {Id = 42}, null
            };

        [Test, TestCaseSource("readAuthInfoFromSessionData")]
        public void ReadAuthInfoFromSessionTest(object dataFromSession)
        {
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(s => s[ConfigurationManager.AppSettings.Get("Auth")]).Returns(dataFromSession);

            var user = assistant.ReadAuthInfoFromSession(session.Object);

            session.Verify(s => s[ConfigurationManager.AppSettings.Get("Auth")], Times.Once());
            Assert.That(user, Is.EqualTo(dataFromSession));
        }
    }
}
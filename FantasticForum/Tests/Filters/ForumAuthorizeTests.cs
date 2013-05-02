using System.Web;
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Mvc.Filters;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.DAL.Abstract;
using Mvc.UtilityModels;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Tests.Filters
{
    [TestFixture]
    public class ForumAuthorizeTests
    {
        private ForumAuthorizeAttribute attribute;
        private Mock<IAuthorizationAssistant> assistantMock;

        [SetUp]
        public void SetUp()
        {
            assistantMock = new Mock<IAuthorizationAssistant>();
            attribute = new ForumAuthorizeAttribute
                {
                    Assistant = assistantMock.Object
                };
        }

        private readonly object[] authorizeCoreData = new object[]
            {
                new object[]
                    {
                        new User {Id = 42}
                    },
                new object[]
                    {
                        null
                    }
            };

        [Test, TestCaseSource("authorizeCoreData")]
        public void AuthorizeCoreTest(User user)
        {
            var session = new Mock<HttpSessionStateBase>();
            var contextBase = new Mock<HttpContextBase>();
            contextBase.Setup(context => context.Session)
                         .Returns(session.Object);
            assistantMock.Setup(assistant => assistant.ReadAuthInfoFromSession(session.Object))
                         .Returns(user);            

            var privateAttribute = new PrivateObject(attribute);
            var isAuthorized = (bool) privateAttribute.Invoke("AuthorizeCore", contextBase.Object);

            assistantMock.Verify(assistant => assistant.ReadAuthInfoFromSession(session.Object),
                                 Times.Once());
            Assert.That(isAuthorized, Is.EqualTo(user != null));
        }
    }
}
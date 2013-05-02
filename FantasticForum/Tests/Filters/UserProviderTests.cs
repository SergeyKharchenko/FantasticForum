using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Mvc.Filters;
using Mvc.Infrastructure.Assistants.Abstract;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Tests.Filters
{
    [TestFixture]
    public class UserProviderTests
    {
        private UserProvider attribute;
        private Mock<IAuthorizationAssistant> assistantMock;

        [SetUp]
        public void SetUp()
        {
            assistantMock = new Mock<IAuthorizationAssistant>();
            attribute = new UserProvider
            {
                Assistant = assistantMock.Object
            };
        }

        [Test]
        public void AuthorizeCoreTest()
        {
            var session = new Mock<HttpSessionStateBase>();
            var contextBase = new Mock<HttpContextBase>();
            contextBase.Setup(context => context.Session)
                         .Returns(session.Object);
            var user = new User();
            assistantMock.Setup(assistant => assistant.ICollection(session.Object))
                         .Returns(user);

            attribute.OnActionExecuting(new ActionExecutingContext { HttpContext = contextBase .Object});

            assistantMock.Verify(assistant => assistant.ICollection(session.Object),
                                 Times.Once());
            contextBase.VerifySet(
                context =>
                context.User =
                It.Is<UserIndentity>(identity => (identity.Identity as UserIndentity).User == user));
        }
    }
}
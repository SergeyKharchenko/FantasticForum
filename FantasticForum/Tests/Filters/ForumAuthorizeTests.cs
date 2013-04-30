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
        private Mock<IRepository<User>> repositoryMock;

        [SetUp]
        public void SetUp()
        {
            assistantMock = new Mock<IAuthorizationAssistant>();
            repositoryMock = new Mock<IRepository<User>>();
            attribute = new ForumAuthorizeAttribute
                {
                    assistant = assistantMock.Object,
                    repository = repositoryMock.Object
                };
        }

        private readonly object[] authorizeCoreData = new object[]
            {
                new object[]
                    {
                        new AuthorizeUtilityModel {IsAuthorized = true, UserId = 42}
                    },
                new object[]
                    {
                        new AuthorizeUtilityModel {IsAuthorized = false}
                    }
            };

        [Test, TestCaseSource("authorizeCoreData")]
        public void AuthorizeCoreTest(AuthorizeUtilityModel authorizeUtilityModel)
        {
            var session = new Mock<HttpSessionStateBase>();
            var contextBase = new Mock<HttpContextBase>();
            contextBase.Setup(context => context.Session)
                         .Returns(session.Object);         
            assistantMock.Setup(assistant => assistant.ReadAuthInfoFromSession(session.Object))
                         .Returns(authorizeUtilityModel);            
            var user = new User();
            repositoryMock.Setup(repo => repo.GetById(authorizeUtilityModel.UserId))
                          .Returns(user);

            var privateAttribute = new PrivateObject(attribute);
            var isAuthorized = (bool) privateAttribute.Invoke("AuthorizeCore", contextBase.Object);

            assistantMock.Verify(assistant => assistant.ReadAuthInfoFromSession(session.Object),
                                 Times.Once());
            Assert.That(isAuthorized, Is.EqualTo(authorizeUtilityModel.IsAuthorized));

            if (!authorizeUtilityModel.IsAuthorized) 
                return;
            repositoryMock.Verify(repo => repo.GetById(authorizeUtilityModel.UserId), Times.Once());
            contextBase.VerifySet(
                context =>
                context.User =
                It.Is<UserIndentity>(identity => (identity.Identity as UserIndentity).User == user));
        }
    }
}
using Moq;
using Mvc.App_Start;
using NUnit.Framework;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tests
{
    [TestFixture]
    public class RouteTests
    {
        [Test]
        public void TestIncomingRoutes()
        {
            #if !DEBUG
            return;
            #endif
            IncomingRouteMatchTest("~/", "List", "Section");
            IncomingRouteMatchTest("~/Section/Create/2", "Create", "Section", new { id = 2 });
            IncomingRouteMatchTest("~/Section/1/", "List", "Topic", new {sectionId = 1});
            IncomingRouteMatchTest("~/Section/1/Topic/Remove/2", "Remove", "Topic", new { sectionId = 1, id = 2 });
        }

        [Test]
        public void TestOutgoingRoutes()
        {
            #if !DEBUG
            return;
            #endif
            OutgoingRouteMatchTest("List", "Section", null, "/");
            OutgoingRouteMatchTest("Create", "Section", new RouteValueDictionary(new { id = 2 }), "/Section/Create/2");
            OutgoingRouteMatchTest("List", "Topic", new RouteValueDictionary(new { sectionId = 1 }), "/Section/1");
            OutgoingRouteMatchTest("Remove", "Topic", new RouteValueDictionary(new { sectionId = 1, id = 2 }), "/Section/1/Topic/Remove/2");
        }

        private static void IncomingRouteMatchTest(string url, string action, string controller, object routeProperties = null, string httpMethod = "GET")
        {
            // Arrange
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act - process the route
            var result = routes.GetRouteData(CreateHttpContext(url, httpMethod));

            // Assert
            TestIncomingRouteResult(result, controller, action, routeProperties);
        }

        private static void OutgoingRouteMatchTest(string action, string controller,
                                                   RouteValueDictionary routeProperties, string url)
        {
            // Arrange
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var context = new RequestContext(CreateHttpContext(), new RouteData());

            // Act - generate the URL
            var result = UrlHelper.GenerateUrl(null, action, controller, routeProperties, routes, context, true);

            // Assert
            Assert.That(url, Is.EqualTo(result));
        }

        private static HttpContextBase CreateHttpContext(string targetUrl = null, string httpMethod = "GET")
        {
            // create the mock request
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath)
                       .Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);
            // create the mock response
            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>()))
                        .Returns<string>(s => s);
            // create the mock context, using the request and response
            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);
            // return the mocked context
            return mockContext.Object;
        }

        private static void TestIncomingRouteResult(RouteData routeResult,
                                                    string controller, string action, object propertySet = null)
        {
            Assert.That(routeResult.Values["controller"],
                        Is.EqualTo(controller).IgnoreCase, "Controller is mismatch");

            Assert.That(routeResult.Values["action"],
                        Is.EqualTo(action).IgnoreCase, "Action is mismatch");

            if (propertySet == null)
                return;
            var propInfo = propertySet.GetType().GetProperties();
            foreach (var propertyInfo in propInfo)
            {
                Assert.That(routeResult.Values.Keys, Has.Member(propertyInfo.Name));

                Assert.That(routeResult.Values[propertyInfo.Name],
                            Is.EqualTo(propertyInfo.GetValue(propertySet).ToString()).IgnoreCase,
                            string.Format("Property is mismatch: {0}", propertyInfo.Name));
            }
        }
    }
}
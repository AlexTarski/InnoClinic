using InnoClinic.Authorization.API.Filters;
using InnoClinic.Authorization.Business.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Polly.CircuitBreaker;
using Polly.RateLimiting;
using Polly.Timeout;

namespace InnoClinic.Authorization.Tests
{
    [TestFixture]
    [Category("Unit")]
    public class ResilienceExceptionFilterUnitTests
    {
        private ResilienceExceptionFilter _filter;
        private ILogger<ResilienceExceptionFilter> _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new NullLogger<ResilienceExceptionFilter>();
            _filter = new ResilienceExceptionFilter(_logger);
        }

        [TestCase(typeof(TimeoutRejectedException), "Timeout", "The request took too long")]
        [TestCase(typeof(BrokenCircuitException), "Service Unavailable", "The service is temporarily unavailable")]
        [TestCase(typeof(RateLimiterRejectedException), "Too Many Requests", "You’ve hit the request limit")]
        public void OnException_WhenPollyExceptionThrown_ReturnsMessageView(Type exceptionType, string expectedTitle, string expectedHeader)
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = (Exception)Activator.CreateInstance(exceptionType)!
            };

            // Act
            _filter.OnException(exceptionContext);

            // Assert
            Assert.That(exceptionContext.ExceptionHandled, Is.True);
            var result = exceptionContext.Result as ViewResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.ViewName, Is.EqualTo("Message"));

            var model = result.ViewData.Model as MessageViewModel;
            using (Assert.EnterMultipleScope())
            {
                Assert.That(model!.Title, Is.EqualTo(expectedTitle));
                Assert.That(model.Header, Is.EqualTo(expectedHeader));
            }
        }
    }
}
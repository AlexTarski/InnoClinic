using InnoClinic.Authorization.API;
using InnoClinic.Authorization.Business.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InnoClinic.Authorization.Tests
{
    [TestFixture]
    [Category("Integration")]
    public class ResilienceExceptionFilterIntegrationTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll<IRazorViewEngine>();
                        services.AddSingleton<IRazorViewEngine, FakeRazorViewEngine>();

                        services.AddControllersWithViews()
                            .PartManager.ApplicationParts.Add(
                                new AssemblyPart(typeof(FakeExceptionController).Assembly));
                    });
                });

            _client = _factory.CreateClient();
        }

        [TearDown]
        public void CleanUp()
        {
            try
            {
                _factory?.Dispose();
                _client?.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        [TestCase("timeout", "The request took too long")]
        [TestCase("circuit", "The service is temporarily unavailable")]
        [TestCase("ratelimit", "You’ve hit the request limit")]
        public async Task Filter_HandlesExceptions_ReturnsMessageView(string exceptionEndpoint, string viewMessage)
        {
            var response = await _client.GetAsync($"/FakeException/{exceptionEndpoint}");

            var responseMessage = await response.Content.ReadAsStringAsync();
            Assert.That(responseMessage, Does.Contain($"{viewMessage}"));
        }
    }

    [Route("FakeException")]
    public class FakeExceptionController : Controller
    {
        [HttpGet("timeout")]
        public IActionResult ThrowTimeout()
        {
            throw new Polly.Timeout.TimeoutRejectedException();
        }

        [HttpGet("circuit")]
        public IActionResult ThrowCircuit()
        {
            throw new Polly.CircuitBreaker.BrokenCircuitException();
        }

        [HttpGet("ratelimit")]
        public IActionResult ThrowRateLimit()
        {
            throw new Polly.RateLimiting.RateLimiterRejectedException();
        }
    }

    /// <summary>
    /// Fake IRazorViewEngine that bypasses .cshtml lookup and always returns a dummy view.
    /// Useful for integration tests of filters without real Razor files.
    /// </summary>
    public class FakeRazorViewEngine : IRazorViewEngine
    {
        public RazorPageResult FindPage(ActionContext context, string pageName)
        {
            // Always return a dummy page
            return new RazorPageResult(pageName, new[] { "Fake page engine" });
        }

        public RazorPageResult GetPage(string executingFilePath, string pagePath)
        {
            // Always return a dummy page
            return new RazorPageResult(pagePath, new[] { "Fake page engine" });
        }

        public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
        {
            return ViewEngineResult.Found(viewName, new FakeView());
        }

        public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
        {
            return ViewEngineResult.Found(viewPath, new FakeView());
        }

        public string? GetAbsolutePath(string? executingFilePath, string? pagePath)
        {
            throw new NotImplementedException();
        }

        private class FakeView : IView
        {
            public string Path => "FakeRazorView";

            public async Task RenderAsync(ViewContext context)
            {
                if (context.ViewData.Model is MessageViewModel model)
                {
                    await context.Writer.WriteAsync(
                        $"[FAKE RAZOR] Title: {model.Title} | Header: {model.Header} | Message: {model.Message}");
                }
                else
                {
                    await context.Writer.WriteAsync("[FAKE RAZOR] No model provided");
                }
            }
        }
    }
}
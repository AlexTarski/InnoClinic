using Duende.IdentityServer.EntityFramework.DbContexts;

using InnoClinic.Authorization.API;
using InnoClinic.Authorization.Business.Models;
using InnoClinic.Authorization.Infrastructure;
using InnoClinic.Shared;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InnoClinic.Authorization.Tests
{
    public static class TestingConstants
    {
        public const string exceptionController = "FakeException";
        public const string timeoutEndpoint = "timeout";
        public const string brokenCircuitEndpoint = "circuit";
        public const string rateLimiterEndpoint = "ratelimit";
        public const string sqliteConnectionString = "Filename=:memory:";
    }

    [TestFixture]
    [Category("Integration")]
    public class ResilienceExceptionFilterIntegrationTests
    {
        private const string timeoutViewMessage = "The request took too long";
        private const string brokenCircuitViewMessage = "The service is temporarily unavailable";
        private const string rateLimiterViewMessage = "You’ve hit the request limit";
        private SqliteConnection? _authConnection;
        private SqliteConnection? _grantsConnection;
        private SqliteConnection? _keysConnection;

        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment(Environments.Testing);
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll<DbContextOptions<AuthorizationContext>>();
                        services.RemoveAll<AuthorizationContext>();
                        services.RemoveAll<DbContextOptions<PersistedGrantDbContext>>();
                        services.RemoveAll<PersistedGrantDbContext>();
                        services.RemoveAll<DbContextOptions<DataProtectionKeysContext>>();
                        services.RemoveAll<DataProtectionKeysContext>();
                        services.RemoveAll<IRazorViewEngine>();

                        //using sqlite helps to avoide collisions with EFCore context registration
                        //EFCore in-memory does not work with SQL commands (like migrate)
                        _authConnection = new SqliteConnection(TestingConstants.sqliteConnectionString);
                        _authConnection.Open();

                        _grantsConnection = new SqliteConnection(TestingConstants.sqliteConnectionString);
                        _grantsConnection.Open();

                        _keysConnection = new SqliteConnection(TestingConstants.sqliteConnectionString);
                        _keysConnection.Open();

                        services.AddDbContext<AuthorizationContext>(options =>
                            options.UseSqlite(_authConnection));

                        services.AddDbContext<PersistedGrantDbContext>(options =>
                            options.UseSqlite(_grantsConnection));

                        services.AddDbContext<DataProtectionKeysContext>(options =>
                            options.UseSqlite(_keysConnection));

                        services.AddDataProtection()
                            .PersistKeysToDbContext<DataProtectionKeysContext>()
                            .SetApplicationName("InnoClinicAuthTest");

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

                _authConnection!.Close();
                _keysConnection!.Close();
                _grantsConnection!.Close();

                _authConnection.Dispose();
                _keysConnection.Dispose();
                _grantsConnection.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        [TestCase(TestingConstants.timeoutEndpoint, timeoutViewMessage)]
        [TestCase(TestingConstants.brokenCircuitEndpoint, brokenCircuitViewMessage)]
        [TestCase(TestingConstants.rateLimiterEndpoint, rateLimiterViewMessage)]
        public async Task Filter_HandlesExceptions_ReturnsMessageView(string exceptionEndpoint, string viewMessage)
        {
            var response = await _client.GetAsync($"/{TestingConstants.exceptionController}/{exceptionEndpoint}");

            var responseMessage = await response.Content.ReadAsStringAsync();
            Assert.That(responseMessage, Does.Contain($"{viewMessage}"));
        }
    }

    [Route(TestingConstants.exceptionController)]
    public class FakeExceptionController : Controller
    {
        [HttpGet(TestingConstants.timeoutEndpoint)]
        public IActionResult ThrowTimeout()
        {
            throw new Polly.Timeout.TimeoutRejectedException();
        }

        [HttpGet(TestingConstants.brokenCircuitEndpoint)]
        public IActionResult ThrowCircuit()
        {
            throw new Polly.CircuitBreaker.BrokenCircuitException();
        }

        [HttpGet(TestingConstants.rateLimiterEndpoint)]
        public IActionResult ThrowRateLimit()
        {
            throw new Polly.RateLimiting.RateLimiterRejectedException();
        }
    }

    /// <summary>
    /// Fake IRazorViewEngine that bypasses .cshtml lookup and always returns a dummy view.
    /// For integration tests of filters without real Razor files.
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
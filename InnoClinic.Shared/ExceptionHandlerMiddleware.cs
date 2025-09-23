using System.Net.Mail;
using System.Text.Json;

using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Shared
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DiNullReferenceException ex)
            {
                Logger.CriticalDiNullReference(_logger, ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new {error = ex.Message}));
            }
            catch (HttpRequestException ex)
            {
                Logger.Error(_logger, ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer
                    .Serialize(new { error = "Service is unavailable. Please try again later." }));
            }
            catch(TaskCanceledException ex)
            {
                Logger.Error(_logger, ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status504GatewayTimeout;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer
                    .Serialize(new { error = "The request timed out or canceled. Please try again later." }));
            }
            catch (SmtpFailedRecipientException ex)
            {
                Logger.Warning(_logger, ex.Message);
                context.Response.StatusCode = StatusCodes.Status502BadGateway;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer
                    .Serialize(new { error = "Failed to deliver email to the recipient. Please check the email address and try again." }));
            }
            catch (SmtpException ex)
            {
                Logger.Warning(_logger, ex.Message);
                context.Response.StatusCode = StatusCodes.Status502BadGateway;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer
                    .Serialize(new { error = "Email service is currently unavailable. Please try again later." }));
            }
            catch (Exception ex)
            {
                Logger.Error(_logger, ex, "Unhandled exception occurred.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer
                    .Serialize(new { error = "An unexpected error occurred. Please try again later." }));
            }
        }
    }
}
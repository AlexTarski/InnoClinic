using InnoClinic.Authorization.Business.Models;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Polly.CircuitBreaker;
using Polly.RateLimiting;
using Polly.Timeout;

namespace InnoClinic.Authorization.API.Filters
{
    public class ResilienceExceptionFilter : IExceptionFilter
    {
        private const string _messagePageName = "Message";
        private readonly ILogger<ResilienceExceptionFilter> _logger;

        public ResilienceExceptionFilter(ILogger<ResilienceExceptionFilter> logger)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
        }

        public void OnException(ExceptionContext context)
        {
            MessageViewModel errorMessage;
            string exceptionMessage;

            switch (context.Exception)
            {
                case TimeoutRejectedException ex:
                    Logger.Error(_logger, ex, "Timeout");
                    errorMessage = new MessageViewModel
                    {
                        Title = "Timeout",
                        Header = "The request took too long",
                        Message = "Please try again later."
                    };

                    exceptionMessage = ex.Message;
                    break;

                case BrokenCircuitException ex:
                    Logger.Error(_logger, ex, "Circuit breaker is open");
                    errorMessage = new MessageViewModel
                    {
                        Title = "Service Unavailable",
                        Header = "The service is temporarily unavailable",
                        Message = "Please try again later."
                    };

                    exceptionMessage = ex.Message;
                    break;

                case RateLimiterRejectedException ex:
                    Logger.Error(_logger, ex, "Rate limit exceeded");
                    errorMessage = new MessageViewModel
                    {
                        Title = "Too Many Requests",
                        Header = "You’ve hit the request limit",
                        Message = "Please slow down and try again later."
                    };

                    exceptionMessage = ex.Message;
                    break;

                case HttpRequestException ex:
                    Logger.Error(_logger, ex, ex.Message);
                    errorMessage = new MessageViewModel
                    {
                        Title = "Request Error",
                        Header = "Unexpected request error",
                        Message = "Try again later or contact administrator for more information."
                    };

                    exceptionMessage = ex.Message;
                    break;

                default:
                    return;
            }

            Logger.InfoSendInfoPageToClient(_logger, $"{errorMessage.Header} : {exceptionMessage}");

            context.Result = new ViewResult
            {
                ViewName = _messagePageName,
                ViewData = new ViewDataDictionary<MessageViewModel>(
                    new EmptyModelMetadataProvider(), context.ModelState)
                {
                    Model = errorMessage
                }
            };

            context.ExceptionHandled = true;
        }
    }
}
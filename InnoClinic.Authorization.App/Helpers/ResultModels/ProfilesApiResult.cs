using System.Net;

namespace InnoClinic.Authorization.Business.Helpers.ResultModels
{
    /// <summary>
    /// Represents a standardized result returned from the Profiles API.
    /// Wraps both the parsed result payload and metadata about the HTTP response.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the result payload returned by the API call.
    /// </typeparam>
    /// <param name="IsSuccess">
    /// Indicates whether the API call was successful (HTTP 2xx).
    /// </param>
    /// <param name="Result">
    /// Represents result of the API call. May be <c>null</c> if the call failed.
    /// </param>
    /// <param name="StatusCode">
    /// The HTTP status code returned by the API call.
    /// </param>
    /// <param name="Message">
    /// Optional message or raw response content for diagnostics and logging.
    /// </param>
    public record ProfilesApiResult<T>(
        bool IsSuccess,
        T Result,
        HttpStatusCode StatusCode,
        string? Message
    );
}
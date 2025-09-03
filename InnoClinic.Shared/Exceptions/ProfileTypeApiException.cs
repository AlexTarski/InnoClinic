namespace InnoClinic.Shared.Exceptions
{
    /// <summary>
    /// Thrown when an attempt to retrieve a profile type from an external API fails due to an internal server error (HTTP 500)
    /// or when the API returns an invalid profile type.
    /// </summary>
    public class ProfileTypeApiException : Exception
    {
        /// <summary>
        /// Thrown when an attempt to retrieve a profile type from an external API fails due to an internal server error (HTTP 500)
        /// or when the API returns an invalid profile type.
        /// </summary>
        public ProfileTypeApiException() : 
            base("Failed to retrieve a valid profile type: API responded with an internal server error (HTTP 500) or returned an invalid profile type.") { }
    }
}
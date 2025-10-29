namespace InnoClinic.Shared.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a page-related argument
    /// (such as page number or page size) falls outside the valid range.
    /// Inherits from <see cref="ArgumentOutOfRangeException"/> to provide
    /// parameter-specific error details.
    /// </summary>
    public class PageOutOfRangeException : ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageOutOfRangeException"/> class
        /// with a default error message indicating that one or more page arguments are invalid.
        /// </summary>
        public PageOutOfRangeException()
            : base("One or more page arguments are invalid.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageOutOfRangeException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public PageOutOfRangeException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageOutOfRangeException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="ex">The exception that is the cause of the current exception.</param>
        public PageOutOfRangeException(string message, Exception ex)
            : base(message, ex) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageOutOfRangeException"/> class
        /// with a specified parameter name and error message.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public PageOutOfRangeException(string paramName, string message)
            : base(paramName, message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageOutOfRangeException"/> class
        /// with a specified parameter name, error message, and a reference to the inner exception
        /// that is the cause of this exception.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="ex">The exception that is the cause of the current exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public PageOutOfRangeException(string paramName, Exception ex, string message)
            : base(paramName, ex, message) { }
    }
}
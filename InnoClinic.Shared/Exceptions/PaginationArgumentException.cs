namespace InnoClinic.Shared.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when one or more pagination arguments are invalid.
    /// Inherits from <see cref="ArgumentException"/> to provide parameter-specific error details.
    /// </summary>
    public class PaginationArgumentException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationArgumentException"/> class
        /// with a default error message.
        /// </summary>
        public PaginationArgumentException()
            : base("One or more pagination arguments are invalid.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationArgumentException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public PaginationArgumentException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationArgumentException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public PaginationArgumentException(string? message, Exception? innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationArgumentException"/> class
        /// with a specified error message and the name of the parameter that caused this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        public PaginationArgumentException(string? message, string? paramName)
            : base(message, paramName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationArgumentException"/> class
        /// with a specified error message, the name of the parameter that caused this exception,
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public PaginationArgumentException(string? message, string? paramName, Exception? innerException)
            : base(message, paramName, innerException) { }
    }
}
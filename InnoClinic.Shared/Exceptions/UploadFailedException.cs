namespace InnoClinic.Shared.Exceptions
{
    /// <summary>
    /// Thrown when an attempt to upload a file to the remote storage fails.
    /// </summary>
    public class UploadFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadFailedException"/> class
        /// with a default error message indicating that the file upload failed.
        /// </summary>
        public UploadFailedException()
            : base("File upload to the remote storage has failed.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadFailedException"/> class
        /// with a custom error message describing the reason for the failure.
        /// </summary>
        /// <param name="message">Custom message describing the reason for the failure.</param>
        public UploadFailedException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadFailedException"/> class
        /// with a custom error message and an inner exception that caused the failure.
        /// </summary>
        /// <param name="message">Custom message describing the reason for the failure.</param>
        /// <param name="inner">The inner exception that caused the upload operation to fail.</param>
        public UploadFailedException(string message, Exception inner)
            : base(message, inner) { }
    }
}
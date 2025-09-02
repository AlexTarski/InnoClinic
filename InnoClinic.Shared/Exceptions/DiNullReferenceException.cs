namespace InnoClinic.Shared.Exceptions
{
    /// <summary>
    /// The exception that is thrown when dependency injection fails to provide a required service instance, resulting in a null reference.
    /// </summary>
    public class DiNullReferenceException : NullReferenceException
    {
        /// <summary>
        /// The exception that is thrown when dependency injection fails to provide a required service instance, resulting in a null reference.
        /// </summary>
        public DiNullReferenceException()
            : base("Dependency Injection failed to provide service instance. Service must not be null.")
        {
        }

        /// <summary>
        /// The exception that is thrown when dependency injection fails to provide a required service instance, resulting in a null reference.
        /// </summary>
        /// <param name="paramName">The name of the parameter that was expected to be injected.</param>
        public DiNullReferenceException(string paramName)
            : base($"Dependency Injection failed to provide {paramName} service instance. {paramName} must not be null.")
        {
        }

        /// <summary>
        /// The exception that is thrown when dependency injection fails to provide a required service instance, resulting in a null reference.
        /// </summary>
        /// <param name="paramName">The name of the parameter that was expected to be injected.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public DiNullReferenceException(string paramName, Exception innerException)
            : base($"Dependency Injection failed to provide {paramName} service instance. {paramName} must not be null.", innerException)
        {
        }
    }
}
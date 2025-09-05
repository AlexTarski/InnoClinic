using Microsoft.Extensions.Logging;

namespace InnoClinic.Shared
{
    public static partial class Logger
    {
        /// <summary>
        /// Logs an informational message indicating that an information page is being sent to the client.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message.</param>
        /// <param name="message">The name or description of the information page being sent to the client.</param>
        [LoggerMessage(
            EventId = 1000,
            Level = LogLevel.Information,
            Message = "Sending `{Message}` information page to client")]
        public static partial void InfoSendInfoPageToClient(ILogger logger, string message);

        /// <summary>
        /// Logs a warning message indicating an invalid page access attempt.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message.</param>
        /// <param name="pageName">The name of the page that was accessed invalidly.</param>
        [LoggerMessage(
            EventId = 1001,
            Level = LogLevel.Warning,
            Message = "Invalid `{PageName}` page access")]
        public static partial void WarningInvalidPageAccess(ILogger logger, string pageName);

        /// <summary>
        /// Logs a debug message indicating that the application is inside a specific method.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message.</param>
        /// <param name="methodName">The name of the method being processed.</param>
        [LoggerMessage(
            EventId = 1900,
            Level = LogLevel.Debug,
            Message = "Inside `{MethodName}` method")]
        public static partial void DebugStartProcessingMethod(ILogger logger, string methodName);

        /// <summary>
        /// Logs a debug message indicating that the application is preparing to enter a specific method.
        /// Use this only when you cannot call <see cref="DebugStartProcessingMethod(ILogger, string)"/> directly.
        /// For example, within system or third-party library methods.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message.</param>
        /// <param name="methodName">The name of the method being prepared to enter.</param>
        [LoggerMessage(
            EventId = 1901,
            Level = LogLevel.Debug,
            Message = "Entering `{MethodName}` method...")]
        public static partial void DebugPrepareToEnter(ILogger logger, string methodName);

        /// <summary>
        /// Logs a debug message indicating that the application is exiting a specific method.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message.</param>
        /// <param name="methodName">The name of the method being exited.</param>
        [LoggerMessage(
            EventId = 1902,
            Level = LogLevel.Debug,
            Message = "Exiting `{MethodName}` method...")]
        public static partial void DebugExitingMethod(ILogger logger,string methodName);

        /// <summary>
        /// Logs a debug message indicating that the application is attempting to retrieve a user by email address.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message.</param>
        /// <param name="email">The email address of the user being retrieved.</param>
        [LoggerMessage(
            EventId = 1903,
            Level = LogLevel.Debug,
            Message = "Getting user by email `{Email}`")]
        public static partial void DebugAuthGettingUserByEmail(ILogger logger, string email);

        /// <summary>
        /// Logs a critical error message indicating that a dependency injection failure resulted in a null reference.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message.</param>
        /// <param name="ex">The <see cref="CriticalDiNullReference(ILogger, Exception, string)"/> that was thrown due to the null reference.</param>
        /// <param name="errorMessage">A message describing the error</param>
        [LoggerMessage(
            EventId = 1951,
            Level = LogLevel.Critical,
            Message = "`{ErrorMessage}`")]
        public static partial void CriticalDiNullReference(ILogger logger, Exception ex, string errorMessage);

        /// <summary>
        /// Logs a warning message indicating that a specified action has failed.
        /// Try to use the name of the method as the action name when calling this logger.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message.</param>
        /// <param name="actionName">The name of the action that failed. This value is included in the log message.</param>
                [LoggerMessage(
            EventId = 1952,
            Level = LogLevel.Warning,
            Message = "Failed to `{ActionName}`")]
        public static partial void WarningFailedDoAction(ILogger logger, string actionName);

        /// <summary>
        /// Logs an informational message indicating the successful completion of the specified action.
        /// Try to use the name of the method as the action name when calling this logger.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message.</param>
        /// <param name="actionName">The name of the action that was successfully completed. This value is included in the log message.</param>
        [LoggerMessage(
            EventId = 1953,
            Level = LogLevel.Information,
            Message = "`{ActionName}` success")]
        public static partial void InfoSuccess(ILogger logger, string actionName);

        /// <summary>
        /// Logs an informational message indicating an attempt to perform the specified action.
        /// Prefer using the method name as the action name when calling this logger.
        /// This method should be distinguished from <see cref="DebugPrepareToEnter(ILogger, string)"/>.
        /// While <see cref="DebugPrepareToEnter(ILogger, string)"/> is intended specifically for method entry points,
        /// this logger is more general and can be used in any context where an action is being attempted.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message.</param>
        /// <param name="actionName">The name of the action being attempted. This value is included in the log message.</param>
        [LoggerMessage(
            EventId = 1954,
            Level = LogLevel.Information,
            Message = "Trying to `{ActionName}`...")]
        public static partial void InfoTryDoAction(ILogger logger, string actionName);

        /// <summary>
        /// Logs an informational message to the specified logger.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message. Cannot be <see langword="null"/>.</param>
        /// <param name="message">The informational message to log. Cannot be <see langword="null"/> or empty.</param>
        [LoggerMessage(
            EventId = 1955,
            Level = LogLevel.Information,
            Message = "{Message}")]
        public static partial void Information(ILogger logger, string message);

        /// <summary>
        /// Logs a warning message to the specified logger.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message. Cannot be <see langword="null"/>.</param>
        /// <param name="message">The warning message to log. Cannot be <see langword="null"/> or empty.</param>
        [LoggerMessage(
            EventId = 1956,
            Level = LogLevel.Warning,
            Message = "{Message}")]
        public static partial void Warning(ILogger logger, string message);

        /// <summary>
        /// Logs a warning message to the specified logger.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message. Cannot be <see langword="null"/>.</param>
        /// <param name="message">The warning message to log. Cannot be <see langword="null"/> or empty.</param>
        ///<param name="ex">The <see cref="Exception"/> that represents the warning to log. Cannot be <see langword="null"/>.</param>
        [LoggerMessage(
            EventId = 1960,
            Level = LogLevel.Warning,
            Message = "{Message}")]
        public static partial void Warning(ILogger logger, Exception ex, string message);

        /// <summary>
        /// Logs an error message along with the associated exception details.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the error. Cannot be <see langword="null"/>.</param>
        /// <param name="ex">The <see cref="Exception"/> that represents the error to log. Cannot be <see langword="null"/>.</param>
        /// <param name="message">The error message to log. Cannot be <see langword="null"/> or empty.</param>
        [LoggerMessage(
            EventId = 1957,
            Level = LogLevel.Error,
            Message = "{Message}")]
        public static partial void Error(ILogger logger, Exception ex, string message);

        /// <summary>
        /// Logs a critical error message along with the associated exception.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to log the message. Cannot be <see langword="null"/>.</ param>
        /// <param name="ex">The exception associated with the critical error. Cannot be <see langword="null"/>.</param>
        /// <param name="message">The message describing the critical error. Cannot be <see langword="null"/> or empty.</param>
        [LoggerMessage(
            EventId = 1958,
            Level = LogLevel.Critical,
            Message = "{Message}")]
        public static partial void Critical(ILogger logger, Exception ex, string message);

        /// <summary>
        /// Logs an informational message indicating the result of a method execution.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance used to write the log message.</param>
        /// <param name="method">The name of the method whose result is being logged.</param>
        /// <param name="result">The result of the method execution, represented as a string.</param>
        [LoggerMessage(
            EventId = 1959,
            Level = LogLevel.Information,
            Message = "{Method}: `{Result}`")]
        public static partial void InfoBoolResult(ILogger logger, string method, string result);
    }
}
using Microsoft.Extensions.Logging;

namespace InnoClinic.Shared
{
    public static partial class Logger
    {
        [LoggerMessage(
            EventId = 1951,
            Level = LogLevel.Critical,
            Message = "`{ErrorMessage}`")]
        public static partial void CriticalDiNullReference(
            ILogger logger,
            Exception ex,
            string errorMessage);

        [LoggerMessage(
            EventId = 1900,
            Level = LogLevel.Debug,
            Message = "Inside `{MethodName}` method")]
        public static partial void DebugStartProcessingMethod(
            ILogger logger,
            string methodName);

        [LoggerMessage(
            EventId = 1901,
            Level = LogLevel.Debug,
            Message = "Entering `{MethodName}` method...")]
        public static partial void DebugExecutingMethod(
            ILogger logger,
            string methodName);

        [LoggerMessage(
            EventId = 1902,
            Level = LogLevel.Debug,
            Message = "Exiting `{MethodName}` method...")]
        public static partial void DebugExitingMethod(
            ILogger logger,
            string methodName);

        [LoggerMessage(
            EventId = 1903,
            Level = LogLevel.Debug,
            Message = "Getting user by email `{Email}`")]
        public static partial void DebugAuthGettingUserByEmail(
            ILogger logger,
            string email);

        [LoggerMessage(
            EventId = 1000,
            Level = LogLevel.Information,
            Message = "Sending `{Message}` information page to client")]
        public static partial void InfoSendInfoPageToClient(
            ILogger logger,
            string message);


        [LoggerMessage(
            EventId = 1001,
            Level = LogLevel.Warning,
            Message = "Invalid login page access")]
        public static partial void WarningInvalidLoginPageAccess(ILogger logger);

        [LoggerMessage(
            EventId = 1002,
            Level = LogLevel.Warning,
            Message = "Failed to sign in")]
        public static partial void WarningFailedToSignIn(ILogger logger);

        [LoggerMessage(
            EventId = 1003,
            Level = LogLevel.Information,
            Message = "User signed in successfully")]
        public static partial void InfoSignInSuccess(ILogger logger);

        [LoggerMessage(
            EventId = 1004,
            Level = LogLevel.Information,
            Message = "Trying to sign in...")]
        public static partial void InfoTrySignIn(ILogger logger);

        [LoggerMessage(
            EventId = 1005,
            Level = LogLevel.Warning,
            Message = "Invalid register page access")]
        public static partial void WarningInvalidRegisterPageAccess(ILogger logger);

        [LoggerMessage(
            EventId = 1006,
            Level = LogLevel.Warning,
            Message = "Failed to sign up")]
        public static partial void WarningFailedToSignUp(ILogger logger);

        [LoggerMessage(
            EventId = 1007,
            Level = LogLevel.Information,
            Message = "User signed up successfully")]
        public static partial void InfoSignUpSuccess(ILogger logger);

        [LoggerMessage(
            EventId = 1008,
            Level = LogLevel.Information,
            Message = "Trying to sign up...")]
        public static partial void InfoTrySignUp(ILogger logger);

        [LoggerMessage(
            EventId = 1009,
            Level = LogLevel.Information,
            Message = "Trying to sign out...")]
        public static partial void InfoTrySignOut(ILogger logger);

        [LoggerMessage(
            EventId = 1010,
            Level = LogLevel.Information,
            Message = "User signed out successfully")]
        public static partial void InfoSignOutSuccess(ILogger logger);

        [LoggerMessage(
            EventId = 1011,
            Level = LogLevel.Information,
            Message = "Email verified successfully")]
        public static partial void InfoEmailVerificationSuccess(ILogger logger);

        [LoggerMessage(
            EventId = 1012,
            Level = LogLevel.Warning,
            Message = "Failed to verify email")]
        public static partial void WarningEmailVerificationFailed(ILogger logger);

    }
}
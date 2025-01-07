using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace BoffToolkit.Logging {

    /// <summary>
    /// Provides shared resources and configurations for logging across the application.
    /// </summary>
    public static class LoggerResources {
        /// <summary>
        /// The global <see cref="ILoggerFactory"/> instance used to create loggers.
        /// </summary>
        public static readonly ILoggerFactory LoggerFactory = CreateLoggerFactory();

        /// <summary>
        /// The default error message used when no specific error message is provided.
        /// </summary>
        public const string DefaultErrorMessage = "Unknown error message";

        /// <summary>
        /// A global lock object for thread-safe operations.
        /// </summary>
        private static readonly object Lock = new();

        /// <summary>
        /// A dictionary to store loggers for different types.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, object> Loggers = new();

        /// <summary>
        /// Indicates whether logging is enabled across the application.
        /// </summary>
        public static bool IsLoggingEnabled { get; set; } = false;

        /// <summary>
        /// Creates the global <see cref="ILoggerFactory"/> instance with default configuration.
        /// </summary>
        /// <returns>An instance of <see cref="ILoggerFactory"/>.</returns>
        private static ILoggerFactory CreateLoggerFactory() {
            return Microsoft.Extensions.Logging.LoggerFactory.Create(builder => {
                builder.AddConsole();
                // Add additional logging providers or configurations here.
            });
        }

        /// <summary>
        /// Gets the global lock object for thread-safe operations.
        /// </summary>
        /// <returns>The global lock object.</returns>
        public static object GetLock() {
            return Lock;
        }

        /// <summary>
        /// Gets an <see cref="ILogger{T}"/> instance for the specified type.
        /// </summary>
        /// <typeparam name="T">The type for which to get the logger.</typeparam>
        /// <returns>An <see cref="ILogger{T}"/> instance.</returns>
        public static ILogger<T> GetLogger<T>() {
            return (ILogger<T>)Loggers.GetOrAdd(typeof(T), _ => LoggerFactory.CreateLogger<T>());
        }
    }

    /// <summary>
    /// Provides centralized logging functionality for a specific type.
    /// </summary>
    /// <typeparam name="T">The type for which logging is performed.</typeparam>
    public static class CentralLogger<T> {

        /// <summary>
        /// Retrieves the singleton instance of <see cref="ILogger{T}"/>.
        /// </summary>
        /// <returns>An instance of <see cref="ILogger{T}"/> if logging is enabled; otherwise, <c>null</c>.</returns>
        private static ILogger<T>? GetLogger() {
            if (!LoggerResources.IsLoggingEnabled) {
                return null;
            }

            return LoggerResources.GetLogger<T>();
        }

        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The arguments for the message.</param>
        public static void LogInformation(string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogInformation("LogInformation: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The arguments for the message.</param>
        public static void LogError(string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogError("LogError: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Logs an error message with an exception and a custom message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The custom message.</param>
        /// <param name="args">The arguments for the message.</param>
        public static void LogError(Exception exception, string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogError(exception, "LogError: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The arguments for the message.</param>
        public static void LogDebug(string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogDebug("LogDebug: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Logs a critical or fatal message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The arguments for the message.</param>
        public static void LogCritical(string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogCritical("LogCritical: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Logs an exception with a custom message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The custom message.</param>
        /// <param name="args">The arguments for the message.</param>
        public static void LogException(Exception exception, string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogError(exception, "LogException: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Logs a trace message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The arguments for the message.</param>
        public static void LogTrace(string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogTrace("LogTrace: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Logs a warning message with an exception and a custom message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The custom message.</param>
        /// <param name="args">The arguments for the message.</param>
        public static void LogWarning(Exception exception, string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogWarning(exception, "LogWarning: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">The arguments for the message.</param>
        public static void LogWarning(string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogWarning("LogWarning: {Message}, Args: {Args}", logMessage, args);
        }
    }
}
using Microsoft.Extensions.Logging;

namespace BoffToolkit.Exceptions {
    /// <summary>
    /// Provides functionality for logging exceptions using a configured logger.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExceptionLogger"/> class.
    /// </remarks>
    /// <param name="logger">The logger to use for logging exceptions.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="logger"/> is <c>null</c>.</exception>
    public class ExceptionLogger(ILogger logger) {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");

        /// <summary>
        /// Logs an exception using the configured logger.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="ex"/> is <c>null</c>.</exception>
        public void LogException(Exception ex) {
            if (ex == null) {
                throw new ArgumentNullException(nameof(ex), "Exception cannot be null.");
            }

            // Extract information from the exception and any inner exception
            var errorMessage = ex.Message;
            var exceptionType = ex.GetType().FullName;
            var stackTrace = ex.StackTrace;
            var innerMessage = ex.InnerException?.Message;
            var innerStackTrace = ex.InnerException?.StackTrace;

            // Log the exception information using the logger without string interpolation
            _logger.LogError("Exception Type: {ExceptionType}\nMessage: {ErrorMessage}\nStackTrace: {StackTrace}\nInnerException Message: {InnerMessage}\nInnerException StackTrace: {InnerStackTrace}",
                             exceptionType, errorMessage, stackTrace, innerMessage, innerStackTrace);
        }
    }
}
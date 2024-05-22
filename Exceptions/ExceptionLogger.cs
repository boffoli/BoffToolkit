using Microsoft.Extensions.Logging;

namespace BoffToolkit.Exceptions {
    /// <summary>
    /// Fornisce funzionalità per registrare eccezioni utilizzando un logger.
    /// </summary>
    public class ExceptionLogger {
        private readonly ILogger _logger;

        /// <summary>
        /// Inizializza una nuova istanza della classe ExceptionLogger.
        /// </summary>
        /// <param name="logger">Il logger da utilizzare per registrare le eccezioni.</param>
        public ExceptionLogger(ILogger logger) {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Il logger non può essere null.");
        }

        /// <summary>
        /// Registra un'eccezione utilizzando il logger configurato.
        /// </summary>
        /// <param name="ex">L'eccezione da registrare.</param>
        public void LogException(Exception ex) {
            if (ex == null) {
                throw new ArgumentNullException(nameof(ex), "L'eccezione non può essere null.");
            }

            // Estrae le informazioni dall'eccezione e dalle eventuali eccezioni interne
            var errorMessage = ex.Message;
            var exceptionType = ex.GetType().FullName;
            var stackTrace = ex.StackTrace;
            var innerMessage = ex.InnerException?.Message;
            var innerStackTrace = ex.InnerException?.StackTrace;

            // Registra le informazioni dell'eccezione utilizzando il logger senza usare l'interpolazione di stringhe
            _logger.LogError("Exception Type: {ExceptionType}\nMessage: {ErrorMessage}\nStackTrace: {StackTrace}\nInnerException Message: {InnerMessage}\nInnerException StackTrace: {InnerStackTrace}",
                             exceptionType, errorMessage, stackTrace, innerMessage, innerStackTrace);
        }
    }
}
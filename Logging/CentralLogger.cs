using Microsoft.Extensions.Logging;

namespace BoffToolkit.Logging {
    /// <summary>
    /// Fornisce funzionalità di logging centralizzate per un tipo specifico.
    /// </summary>
    /// <typeparam name="T">Il tipo per il quale si desidera effettuare il logging.</typeparam>
    public static class CentralLogger<T> {
        private static readonly ILoggerFactory _loggerFactory = CreateLoggerFactory();
        private static ILogger<T>? _instance;
        private static readonly object _lock = new object();
        private const string DefaultErrorMessage = "Messaggio di errore sconosciuto";

        // Classe interna per mantenere lo stato globale di logging
        private static class LoggingState {
            public static bool IsLoggingEnabled = false;
        }

        /// <summary>
        /// Ottiene o imposta se il logging è abilitato o disabilitato.
        /// Questa proprietà influisce su tutti i tipi T utilizzati con CentralLogger.
        /// </summary>
        public static bool IsOn {
            get => LoggingState.IsLoggingEnabled;
            set => LoggingState.IsLoggingEnabled = value;
        }

        /// <summary>
        /// Restituisce l'istanza singleton di ILogger<T>.
        /// </summary>
        /// <returns>Un'istanza di ILogger<T> se il logging è abilitato, altrimenti null.</returns>
        private static ILogger<T>? GetInstance() {
            if (!IsOn)
                return null;

            lock (_lock) {
                _instance ??= _loggerFactory.CreateLogger<T>();
            }

            return _instance;
        }

        /// <summary>
        /// Registra un messaggio di informazione.
        /// </summary>
        /// <param name="message">Il messaggio da registrare.</param>
        public static void LogInformation(string message) {
            GetInstance()?.LogInformation(message ?? DefaultErrorMessage);
        }

        /// <summary>
        /// Registra un messaggio di errore.
        /// </summary>
        /// <param name="message">Il messaggio da registrare.</param>
        public static void LogError(string message) {
            GetInstance()?.LogError(message ?? DefaultErrorMessage);
        }

        /// <summary>
        /// Registra un messaggio di debug.
        /// </summary>
        /// <param name="message">Il messaggio da registrare.</param>
        public static void LogDebug(string message) {
            GetInstance()?.LogDebug(message ?? DefaultErrorMessage);
        }

        /// <summary>
        /// Registra un messaggio di avviso.
        /// </summary>
        /// <param name="message">Il messaggio da registrare.</param>
        public static void LogWarning(string message) {
            GetInstance()?.LogWarning(message ?? DefaultErrorMessage);
        }

        /// <summary>
        /// Registra un messaggio critico o fatale.
        /// </summary>
        /// <param name="message">Il messaggio da registrare.</param>
        public static void LogCritical(string message) {
            GetInstance()?.LogCritical(message ?? DefaultErrorMessage);
        }

        /// <summary>
        /// Registra un'eccezione con un messaggio personalizzato.
        /// </summary>
        /// <param name="exception">L'eccezione da registrare.</param>
        /// <param name="message">Il messaggio personalizzato.</param>
        public static void LogException(Exception exception, string message) {
            GetInstance()?.LogError(exception, message ?? DefaultErrorMessage);
        }

        /// <summary>
        /// Registra un messaggio di traccia dettagliata.
        /// </summary>
        /// <param name="message">Il messaggio da registrare.</param>
        public static void LogTrace(string message) {
            GetInstance()?.LogTrace(message ?? DefaultErrorMessage);
        }

        /// <summary>
        /// Registra un messaggio di avviso con eccezione e messaggio personalizzato.
        /// </summary>
        /// <param name="exception">L'eccezione da registrare.</param>
        /// <param name="message">Il messaggio personalizzato.</param>
        public static void LogWarning(Exception exception, string message) {
            GetInstance()?.LogWarning(exception, message ?? DefaultErrorMessage);
        }

        /// <summary>
        /// Registra un messaggio di errore con eccezione e messaggio personalizzato.
        /// </summary>
        /// <param name="exception">L'eccezione da registrare.</param>
        /// <param name="message">Il messaggio personalizzato.</param>
        public static void LogError(Exception exception, string message) {
            GetInstance()?.LogError(exception, message ?? DefaultErrorMessage);
        }

        /// <summary>
        /// Crea l'ILoggerFactory con i provider di logging desiderati.
        /// </summary>
        /// <returns>Un'istanza di ILoggerFactory configurata.</returns>
        private static ILoggerFactory CreateLoggerFactory() {
            return LoggerFactory.Create(builder => {
                builder.AddConsole();
                // Aggiungi qui altri provider di logging o configurazioni.
            });
        }
    }
}

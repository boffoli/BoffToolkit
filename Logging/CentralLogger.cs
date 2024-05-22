using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace BoffToolkit.Logging {

    // Classe non generica per mantenere lo stato globale di logging e le risorse condivise
    public static class LoggerResources {
        public static readonly ILoggerFactory LoggerFactory = CreateLoggerFactory();
        public const string DefaultErrorMessage = "Messaggio di errore sconosciuto";
        private static readonly object Lock = new();
        private static readonly ConcurrentDictionary<Type, object> Loggers = new();

        public static bool IsLoggingEnabled { get; set; } = false;

        private static ILoggerFactory CreateLoggerFactory() {
            return Microsoft.Extensions.Logging.LoggerFactory.Create(builder => {
                builder.AddConsole();
                // Aggiungi qui altri provider di logging o configurazioni.
            });
        }

        public static object GetLock() {
            return Lock;
        }

        public static ILogger<T> GetLogger<T>() {
            return (ILogger<T>)Loggers.GetOrAdd(typeof(T), _ => LoggerFactory.CreateLogger<T>());
        }
    }

    /// <summary>
    /// Fornisce funzionalità di logging centralizzate per un tipo specifico.
    /// </summary>
    /// <typeparam name="T">Il tipo per il quale si desidera effettuare il logging.</typeparam>
    public static class CentralLogger<T> {

        /// <summary>
        /// Restituisce l'istanza singleton di ILogger<T>.
        /// </summary>
        /// <returns>Un'istanza di ILogger<T> se il logging è abilitato, altrimenti null.</returns>
        private static ILogger<T>? GetLogger() {
            if (!LoggerResources.IsLoggingEnabled) {
                return null;
            }

            return LoggerResources.GetLogger<T>();
        }

        /// <summary>
        /// Registra un messaggio di informazione.
        /// </summary>
        /// <param name="message">Il messaggio da registrare.</param>
        /// <param name="args">Gli argomenti del messaggio.</param>
        public static void LogInformation(string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogInformation("LogInformation: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Registra un messaggio di errore.
        /// </summary>
        /// <param name="message">Il messaggio da registrare.</param>
        /// <param name="args">Gli argomenti del messaggio.</param>
        public static void LogError(string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogError("LogError: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Registra un messaggio di errore con eccezione e messaggio personalizzato.
        /// </summary>
        /// <param name="exception">L'eccezione da registrare.</param>
        /// <param name="message">Il messaggio personalizzato.</param>
        /// <param name="args">Gli argomenti del messaggio.</param>
        public static void LogError(Exception exception, string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogError(exception, "LogError: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Registra un messaggio di debug.
        /// </summary>
        /// <param name="message">Il messaggio da registrare.</param>
        /// <param name="args">Gli argomenti del messaggio.</param>
        public static void LogDebug(string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogDebug("LogDebug: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Registra un messaggio critico o fatale.
        /// </summary>
        /// <param name="message">Il messaggio da registrare.</param>
        /// <param name="args">Gli argomenti del messaggio.</param>
        public static void LogCritical(string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogCritical("LogCritical: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Registra un'eccezione con un messaggio personalizzato.
        /// </summary>
        /// <param name="exception">L'eccezione da registrare.</param>
        /// <param name="message">Il messaggio personalizzato.</param>
        /// <param name="args">Gli argomenti del messaggio.</param>
        public static void LogException(Exception exception, string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogError(exception, "LogException: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Registra un messaggio di traccia dettagliata.
        /// </summary>
        /// <param name="message">Il messaggio da registrare.</param>
        /// <param name="args">Gli argomenti del messaggio.</param>
        public static void LogTrace(string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogTrace("LogTrace: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Registra un messaggio di avviso con eccezione e messaggio personalizzato.
        /// </summary>
        /// <param name="exception">L'eccezione da registrare.</param>
        /// <param name="message">Il messaggio personalizzato.</param>
        /// <param name="args">Gli argomenti del messaggio.</param>
        public static void LogWarning(Exception exception, string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogWarning(exception, "LogWarning: {Message}, Args: {Args}", logMessage, args);
        }

        /// <summary>
        /// Registra un messaggio di avviso.
        /// </summary>
        /// <param name="message">Il messaggio da registrare.</param>
        /// <param name="args">Gli argomenti del messaggio.</param>
        public static void LogWarning(string message, params object?[] args) {
            var logMessage = message ?? LoggerResources.DefaultErrorMessage;
            GetLogger()?.LogWarning("LogWarning: {Message}, Args: {Args}", logMessage, args);
        }
    }
}
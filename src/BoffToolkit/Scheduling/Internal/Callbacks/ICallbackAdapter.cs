using System;
using System.Threading.Tasks;

namespace BoffToolkit.Scheduling.Internal.Callbacks {
    /// <summary>
    /// Classe per gli argomenti dell'evento di completamento del callback.
    /// </summary>
    /// <remarks>
    /// Inizializza una nuova istanza della classe <see cref="CallbackCompletedEventArgs"/>.
    /// </remarks>
    /// <param name="result">Il risultato del callback.</param>
    public class CallbackCompletedEventArgs(object? result) : EventArgs {

        /// <summary>
        /// Ottiene il risultato del callback, se disponibile.
        /// </summary>
        public object? Result { get; } = result;
    }

    /// <summary>
    /// Interfaccia per l'adattatore di callback che gestisce l'esecuzione di diversi tipi di callback.
    /// </summary>
    internal interface ICallbackAdapter {
        /// <summary>
        /// Evento che viene sollevato al completamento del callback.
        /// </summary>
        event EventHandler<CallbackCompletedEventArgs>? CallbackCompleted;

        /// <summary>
        /// Esegue il callback in modo asincrono.
        /// </summary>
        /// <returns>Un Task che rappresenta l'operazione asincrona.</returns>
        Task ExecuteAsync();
    }
}
using System;
using System.Threading.Tasks;

namespace BoffToolkit.Scheduling.Internal.Callbacks {
    /// <summary>
    /// Classe per gli argomenti dell'evento di completamento del callback.
    /// </summary>
    public class CallbackCompletedEventArgs : EventArgs {
        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="CallbackCompletedEventArgs"/>.
        /// </summary>
        /// <param name="result">Il risultato del callback.</param>
        public CallbackCompletedEventArgs(object? result) {
            Result = result;
        }

        /// <summary>
        /// Ottiene il risultato del callback, se disponibile.
        /// </summary>
        public object? Result { get; }
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
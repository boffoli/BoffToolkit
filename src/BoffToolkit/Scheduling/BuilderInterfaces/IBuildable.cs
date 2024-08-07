using System;
using BoffToolkit.Scheduling.Internal.Callbacks;

namespace BoffToolkit.Scheduling.BuilderInterfaces {
    /// <summary>
    /// Interfaccia per costruire l'istanza finale di <see cref="IJobScheduler"/>.
    /// </summary>
    public interface IBuildable {
        /// <summary>
        /// Imposta il gestore per l'evento di completamento del callback.
        /// </summary>
        /// <param name="handler">Il gestore dell'evento.</param>
        /// <returns>Un'istanza di <see cref="IBuildable"/>.</returns>
        IBuildable SetCallbackCompleted(EventHandler<CallbackCompletedEventArgs> handler);

        /// <summary>
        /// Costruisce l'istanza di <see cref="IJobScheduler"/> con le specifiche fornite.
        /// </summary>
        /// <returns>Un'istanza di <see cref="IJobScheduler"/>.</returns>
        IJobScheduler Build();
    }
}
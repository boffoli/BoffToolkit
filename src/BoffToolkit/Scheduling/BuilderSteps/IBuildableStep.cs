using System;
using BoffToolkit.Scheduling.Internal.Callbacks;

namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interface for building the final instance of <see cref="IJobScheduler"/>.
    /// </summary>
    public interface IBuildableStep {
        /// <summary>
        /// Sets the handler for the callback completion event.
        /// </summary>
        /// <param name="handler">The event handler for callback completion.</param>
        /// <returns>An instance of <see cref="IBuildableStep"/> for further configuration.</returns>
        IBuildableStep SetCallbackCompleted(EventHandler<CallbackCompletedEventArgs> handler);

        /// <summary>
        /// Constructs an instance of <see cref="IJobScheduler"/> with the specified configuration.
        /// </summary>
        /// <returns>An instance of <see cref="IJobScheduler"/>.</returns>
        IJobScheduler Build();
    }
}
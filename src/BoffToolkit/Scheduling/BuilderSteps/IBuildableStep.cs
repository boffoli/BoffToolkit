using BoffToolkit.Scheduling.Internal.Callbacks;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interface for building the final instance of <see cref="IJobScheduler{TPeriodRule}"/>.
    /// </summary>
    /// <typeparam name="TPeriodRule">The specific type of period rule used by the job scheduler.</typeparam>
    public interface IBuildableStep<TPeriodRule> where TPeriodRule : IPeriodRule {
        /// <summary>
        /// Sets the handler for the callback completion event.
        /// </summary>
        /// <param name="handler">
        /// The event handler that will be invoked when a scheduled callback is completed.
        /// </param>
        /// <returns>
        /// The current instance of <see cref="IBuildableStep{TPeriodRule}"/> to allow method chaining.
        /// </returns>
        IBuildableStep<TPeriodRule> SetCallbackCompleted(EventHandler<CallbackCompletedEventArgs> handler);

        /// <summary>
        /// Constructs an instance of <see cref="IJobScheduler{TPeriodRule}"/> with the specified configuration.
        /// </summary>
        /// <returns>
        /// A fully configured instance of <see cref="IJobScheduler{TPeriodRule}"/> ready for execution.
        /// </returns>
        IJobScheduler<TPeriodRule> Build();
    }
}
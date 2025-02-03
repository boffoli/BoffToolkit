using BoffToolkit.Scheduling.Internal.Callbacks;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Defines the final step in building a job scheduler instance, ensuring that all required
    /// configurations are set before constructing the scheduler.
    /// </summary>
    /// <typeparam name="TPeriodRule">
    /// The specific type of period rule that defines the scheduling logic for the job scheduler.
    /// </typeparam>
    public interface IBuildableStep<TPeriodRule>
        where TPeriodRule : IPeriodRule {

        /// <summary>
        /// Registers an event handler that will be triggered when a scheduled callback completes execution.
        /// </summary>
        /// <param name="handler">
        /// The event handler to invoke when the job scheduler completes a scheduled callback execution.
        /// </param>
        /// <returns>
        /// The current instance of <see cref="IBuildableStep{TPeriodRule}"/>,
        /// allowing for method chaining to continue the configuration process.
        /// </returns>
        IBuildableStep<TPeriodRule> SetCallbackCompleted(EventHandler<CallbackCompletedEventArgs> handler);

        /// <summary>
        /// Constructs a fully configured instance of the job scheduler, applying all previously defined settings.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="IJobScheduler{TPeriodRule}"/>, configured with the specified
        /// start time, end time, scheduling period, callback functions, and optional registry settings.
        /// </returns>
        IJobScheduler<TPeriodRule> Build();
    }

    /// <summary>
    /// Step interface for finalizing the configuration of a job scheduler with a TimeSpan-based period rule.
    /// </summary>
    public interface IBuildableStepTimeSpan : IBuildableStep<ITimeSpanPeriodRule> { }

    /// <summary>
    /// Step interface for finalizing the configuration of a job scheduler with a daily period rule.
    /// </summary>
    public interface IBuildableStepDaily : IBuildableStep<IDailyPeriodRule> { }

    /// <summary>
    /// Step interface for finalizing the configuration of a job scheduler with a weekly period rule.
    /// </summary>
    public interface IBuildableStepWeekly : IBuildableStep<IWeeklyPeriodRule> { }

    /// <summary>
    /// Step interface for finalizing the configuration of a job scheduler with a monthly period rule.
    /// </summary>
    public interface IBuildableStepMonthly : IBuildableStep<IMonthlyPeriodRule> { }

    /// <summary>
    /// Step interface for finalizing the configuration of a job scheduler with an annual period rule.
    /// </summary>
    public interface IBuildableStepAnnual : IBuildableStep<IAnnualPeriodRule> { }
}
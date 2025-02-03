using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.BuilderSteps {

    /// <summary>
    /// Generic interface for setting a specific period rule type.
    /// </summary>
    /// <typeparam name="TPeriodRule">The specific period rule type associated with the job scheduler.</typeparam>
    public interface IPeriodStep<TPeriodRule>
        where TPeriodRule : IPeriodRule {

        /// <summary>
        /// Sets the execution period rule of type <typeparamref name="TPeriodRule"/>.
        /// </summary>
        /// <param name="periodRule">The specific period rule to set.</param>
        /// <returns>
        /// An instance of <see cref="ICallbackStep{TPeriodRule}"/> for further configuration.
        /// </returns>
        ICallbackStep<TPeriodRule> SetPeriod(TPeriodRule periodRule);
    }

    /// <summary>
    /// Step interface for configuring a job scheduler with a TimeSpan-based period rule.
    /// </summary>
    public interface IPeriodStepTimeSpan : IPeriodStep<ITimeSpanPeriodRule> { }

    /// <summary>
    /// Step interface for configuring a job scheduler with a daily period rule.
    /// </summary>
    public interface IPeriodStepDaily : IPeriodStep<IDailyPeriodRule> { }

    /// <summary>
    /// Step interface for configuring a job scheduler with a weekly period rule.
    /// </summary>
    public interface IPeriodStepWeekly : IPeriodStep<IWeeklyPeriodRule> { }

    /// <summary>
    /// Step interface for configuring a job scheduler with a monthly period rule.
    /// </summary>
    public interface IPeriodStepMonthly : IPeriodStep<IMonthlyPeriodRule> { }

    /// <summary>
    /// Step interface for configuring a job scheduler with an annual period rule.
    /// </summary>
    public interface IPeriodStepAnnual : IPeriodStep<IAnnualPeriodRule> { }
}
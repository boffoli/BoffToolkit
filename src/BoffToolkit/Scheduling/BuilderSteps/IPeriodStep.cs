using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.BuilderSteps {

    /// <summary>
    /// Generic interface for setting a specific period rule type.
    /// </summary>
    public interface IPeriodStep<TPeriodRule> where TPeriodRule : IPeriodRule {
        /// <summary>
        /// Sets the execution period rule of type <typeparamref name="TPeriodRule"/>.
        /// </summary>
        /// <param name="periodRule">The specific period rule to set.</param>
        /// <returns>An instance of <see cref="ICallbackStep{TPeriodRule}"/> for further configuration.</returns>
        ICallbackStep<TPeriodRule> SetPeriod(TPeriodRule periodRule);
    }

    /// <summary>
    /// Step interface for configuring a TimeSpan-based period rule.
    /// </summary>
    public interface ITimeSpanPeriodStep : IPeriodStep<ITimeSpanPeriodRule> { }

    /// <summary>
    /// Step interface for configuring a daily period rule.
    /// </summary>
    public interface IDailyPeriodStep : IPeriodStep<IDailyPeriodRule> { }

    /// <summary>
    /// Step interface for configuring a weekly period rule.
    /// </summary>
    public interface IWeeklyPeriodStep : IPeriodStep<IWeeklyPeriodRule> { }

    /// <summary>
    /// Step interface for configuring a monthly period rule.
    /// </summary>
    public interface IMonthlyPeriodStep : IPeriodStep<IMonthlyPeriodRule> { }

    /// <summary>
    /// Step interface for configuring an annual period rule.
    /// </summary>
    public interface IAnnualPeriodStep : IPeriodStep<IAnnualPeriodRule> { }
}
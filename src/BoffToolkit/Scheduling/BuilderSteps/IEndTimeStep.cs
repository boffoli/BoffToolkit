using System;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interface for setting the end time of the job scheduler.
    /// </summary>
    /// <typeparam name="TPeriodRule">
    /// The specific period rule type associated with the job scheduler.
    /// </typeparam>
    public interface IEndTimeStep<TPeriodRule>
        where TPeriodRule : IPeriodRule {

        /// <summary>
        /// Sets the end time of the job scheduler.
        /// </summary>
        /// <param name="endTime">The end time to set.</param>
        /// <returns>
        /// An instance of <see cref="IPeriodStep{TPeriodRule}"/> to proceed with the configuration.
        /// </returns>
        IPeriodStep<TPeriodRule> SetEnd(DateTime endTime);

        /// <summary>
        /// Specifies that the job scheduler does not have an end time.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="IPeriodStep{TPeriodRule}"/> to proceed with the configuration.
        /// </returns>
        IPeriodStep<TPeriodRule> SetNoEnd();
    }

    /// <summary>
    /// Step interface for configuring the end time of a job scheduler with a TimeSpan-based period rule.
    /// </summary>
    public interface IEndTimeStepTimeSpan : IEndTimeStep<ITimeSpanPeriodRule> { }

    /// <summary>
    /// Step interface for configuring the end time of a job scheduler with a daily period rule.
    /// </summary>
    public interface IEndTimeStepDaily : IEndTimeStep<IDailyPeriodRule> { }

    /// <summary>
    /// Step interface for configuring the end time of a job scheduler with a weekly period rule.
    /// </summary>
    public interface IEndTimeStepWeekly : IEndTimeStep<IWeeklyPeriodRule> { }

    /// <summary>
    /// Step interface for configuring the end time of a job scheduler with a monthly period rule.
    /// </summary>
    public interface IEndTimeStepMonthly : IEndTimeStep<IMonthlyPeriodRule> { }

    /// <summary>
    /// Step interface for configuring the end time of a job scheduler with an annual period rule.
    /// </summary>
    public interface IEndTimeStepAnnual : IEndTimeStep<IAnnualPeriodRule> { }
}
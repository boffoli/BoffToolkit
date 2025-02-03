using System;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.BuilderSteps {

    /// <summary>
    /// Defines the step for setting the start time in the job scheduler configuration.
    /// </summary>
    /// <typeparam name="TPeriodRule">
    /// The specific period rule type associated with the job scheduler.
    /// </typeparam>
    public interface IStartTimeStep<TPeriodRule>
        where TPeriodRule : IPeriodRule {

        /// <summary>
        /// Specifies the start time of the job scheduler.
        /// </summary>
        /// <param name="startTime">
        /// The date and time when the job scheduler should start executing.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IEndTimeStep{TPeriodRule}"/> to proceed with the configuration.
        /// </returns>
        IEndTimeStep<TPeriodRule> SetStart(DateTime startTime);
    }

    /// <summary>
    /// Step interface for configuring the start time of a job scheduler with a TimeSpan-based period rule.
    /// </summary>
    public interface IStartTimeStepTimeSpan : IStartTimeStep<ITimeSpanPeriodRule> { }

    /// <summary>
    /// Step interface for configuring the start time of a job scheduler with a daily period rule.
    /// </summary>
    public interface IStartTimeStepDaily : IStartTimeStep<IDailyPeriodRule> { }

    /// <summary>
    /// Step interface for configuring the start time of a job scheduler with a weekly period rule.
    /// </summary>
    public interface IStartTimeStepWeekly : IStartTimeStep<IWeeklyPeriodRule> { }

    /// <summary>
    /// Step interface for configuring the start time of a job scheduler with a monthly period rule.
    /// </summary>
    public interface IStartTimeStepMonthly : IStartTimeStep<IMonthlyPeriodRule> { }

    /// <summary>
    /// Step interface for configuring the start time of a job scheduler with an annual period rule.
    /// </summary>
    public interface IStartTimeStepAnnual : IStartTimeStep<IAnnualPeriodRule> { }
}
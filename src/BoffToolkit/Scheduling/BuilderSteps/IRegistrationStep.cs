using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Represents the registration step in the job scheduler build process, allowing 
    /// the scheduler to be added to a global registry or explicitly skipped.
    /// </summary>
    /// <typeparam name="TPeriodRule">
    /// The specific type of period rule that defines the scheduling logic for the job scheduler.
    /// </typeparam>
    public interface IRegistrationStep<TPeriodRule>
        where TPeriodRule : IPeriodRule {

        /// <summary>
        /// Registers the job scheduler in the global registry using a unique identifier key.
        /// </summary>
        /// <param name="key">
        /// A unique identifier used to store and retrieve the job scheduler from the registry.
        /// </param>
        /// <param name="overwrite">
        /// If set to <c>true</c>, an existing job scheduler with the same key will be replaced.
        /// If set to <c>false</c> and the key already exists, an exception may be thrown.
        /// The default value is <c>false</c>.
        /// </param>
        /// <returns>
        /// The current instance of <see cref="IBuildableStep{TPeriodRule}"/>, 
        /// allowing the configuration process to continue.
        /// </returns>
        IBuildableStep<TPeriodRule> AddToRegistry(string key, bool overwrite = false);

        /// <summary>
        /// Skips adding the job scheduler to the global registry, allowing it to function
        /// without being globally tracked.
        /// </summary>
        /// <returns>
        /// The current instance of <see cref="IBuildableStep{TPeriodRule}"/>, 
        /// allowing the configuration process to continue.
        /// </returns>
        IBuildableStep<TPeriodRule> SkipRegistry();
    }

    /// <summary>
    /// Step interface for registering a job scheduler with a TimeSpan-based period rule.
    /// </summary>
    public interface IRegistrationStepTimeSpan : IRegistrationStep<ITimeSpanPeriodRule> { }

    /// <summary>
    /// Step interface for registering a job scheduler with a daily period rule.
    /// </summary>
    public interface IRegistrationStepDaily : IRegistrationStep<IDailyPeriodRule> { }

    /// <summary>
    /// Step interface for registering a job scheduler with a weekly period rule.
    /// </summary>
    public interface IRegistrationStepWeekly : IRegistrationStep<IWeeklyPeriodRule> { }

    /// <summary>
    /// Step interface for registering a job scheduler with a monthly period rule.
    /// </summary>
    public interface IRegistrationStepMonthly : IRegistrationStep<IMonthlyPeriodRule> { }

    /// <summary>
    /// Step interface for registering a job scheduler with an annual period rule.
    /// </summary>
    public interface IRegistrationStepAnnual : IRegistrationStep<IAnnualPeriodRule> { }
}
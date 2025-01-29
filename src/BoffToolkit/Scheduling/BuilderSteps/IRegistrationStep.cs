using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interface for the registration step in the JobScheduler build process.
    /// </summary>
    /// <typeparam name="TPeriodRule">The type of period rule used by the job scheduler.</typeparam>
    public interface IRegistrationStep<TPeriodRule> where TPeriodRule : IPeriodRule {
        /// <summary>
        /// Adds the job scheduler to the global registry with a unique key.
        /// </summary>
        /// <param name="key">The unique key to identify the job scheduler in the registry.</param>
        /// <param name="overwrite">
        /// If <c>true</c>, an existing scheduler with the same key will be overwritten.
        /// Defaults to <c>false</c>.
        /// </param>
        /// <returns>An instance of <see cref="IBuildableStep{TPeriodRule}"/> to continue the configuration process.</returns>
        IBuildableStep<TPeriodRule> AddToRegistry(string key, bool overwrite = false);

        /// <summary>
        /// Skips adding the job scheduler to the global registry.
        /// </summary>
        /// <returns>An instance of <see cref="IBuildableStep{TPeriodRule}"/> to continue the configuration process.</returns>
        IBuildableStep<TPeriodRule> SkipRegistry();
    }
}
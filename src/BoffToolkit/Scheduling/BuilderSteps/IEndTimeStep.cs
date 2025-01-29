using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interface for setting the end time of the job scheduler.
    /// </summary>
    public interface IEndTimeStep<TPeriodRule> where TPeriodRule : IPeriodRule {
        /// <summary>
        /// Sets the end time of the job scheduler.
        /// </summary>
        /// <param name="endTime">The end time to set.</param>
        /// <returns>An instance of <see cref="IPeriodStep{TPeriodRule}"/> to proceed with the configuration.</returns>
        IPeriodStep<TPeriodRule> SetEnd(DateTime endTime);

        /// <summary>
        /// Specifies that the job scheduler does not have an end time.
        /// </summary>
        /// <returns>An instance of <see cref="IPeriodStep{TPeriodRule}"/> to proceed with the configuration.</returns>
        IPeriodStep<TPeriodRule> SetNoEnd();
    }
}
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interface for configuring the execution period of a job.
    /// </summary>
    public interface IPeriodStep {
        /// <summary>
        /// Sets the execution period rule.
        /// </summary>
        /// <param name="periodRule">The period rule to set.</param>
        /// <returns>An instance of <see cref="ICallbackStep"/> for further configuration.</returns>
        ICallbackStep SetPeriod(IPeriodRule periodRule);
    }
}
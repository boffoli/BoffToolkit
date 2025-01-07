using System;

namespace BoffToolkit.Scheduling.PeriodRules {
    /// <summary>
    /// Interface that defines a rule for determining the next occurrence of a period.
    /// </summary>
    public interface IPeriodRule {
        /// <summary>
        /// Returns the next occurrence of the period starting from a specified time.
        /// </summary>
        /// <param name="fromTime">The starting point from which to calculate the next occurrence.</param>
        /// <returns>The next occurrence of the period.</returns>
        DateTime GetNextOccurrence(DateTime fromTime);
    }
}
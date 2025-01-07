using System;

namespace BoffToolkit.Scheduling.PeriodRules {
    /// <summary>
    /// Interface defining a daily period rule with a specific interval.
    /// </summary>
    public interface IDailyPeriodRule : IPeriodRule {
        /// <summary>
        /// Gets the time of day when the event occurs.
        /// </summary>
        TimeSpan TimeOfDay { get; }

        /// <summary>
        /// Gets the interval in days between occurrences.
        /// </summary>
        int DaysInterval { get; }
    }
}
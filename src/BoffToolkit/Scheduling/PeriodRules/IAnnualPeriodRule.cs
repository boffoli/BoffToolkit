using System;

namespace BoffToolkit.Scheduling.PeriodRules {
    /// <summary>
    /// Interface defining an annual period rule with a specific interval.
    /// </summary>
    public interface IAnnualPeriodRule : IPeriodRule {
        /// <summary>
        /// Gets the month of the year (1-12) when the event occurs.
        /// </summary>
        int Month { get; }

        /// <summary>
        /// Gets the day of the month (1-31) when the event occurs.
        /// </summary>
        int Day { get; }

        /// <summary>
        /// Gets the interval in years between occurrences.
        /// </summary>
        int YearsInterval { get; }
    }
}
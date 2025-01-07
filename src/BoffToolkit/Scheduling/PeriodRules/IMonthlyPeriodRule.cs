using System;

namespace BoffToolkit.Scheduling.PeriodRules {
    /// <summary>
    /// Interface defining a monthly period rule with a specific interval.
    /// </summary>
    public interface IMonthlyPeriodRule : IPeriodRule {
        /// <summary>
        /// Gets the day of the month (1-31) when the event occurs.
        /// </summary>
        int DayOfMonth { get; }

        /// <summary>
        /// Gets the interval in months between occurrences.
        /// </summary>
        int MonthsInterval { get; }
    }
}
using System;

namespace BoffToolkit.Scheduling.PeriodRules {
    /// <summary>
    /// Interface that defines a rule for scheduling weekly occurrences with a specific interval.
    /// </summary>
    public interface IWeeklyPeriodRule : IPeriodRule {
        /// <summary>
        /// Gets the day of the week on which the event occurs.
        /// </summary>
        DayOfWeek DayOfWeek { get; }

        /// <summary>
        /// Gets the number of weeks between consecutive occurrences.
        /// </summary>
        int WeeksInterval { get; }
    }
}
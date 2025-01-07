using System;

namespace BoffToolkit.Scheduling.PeriodRules {
    /// <summary>
    /// Interface that defines a time-based period rule using a specific TimeSpan interval.
    /// </summary>
    public interface ITimeSpanPeriodRule : IPeriodRule {
        /// <summary>
        /// Gets the time interval between one occurrence and the next.
        /// </summary>
        TimeSpan Interval { get; }
    }
}
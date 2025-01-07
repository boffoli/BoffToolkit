using System;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Internal.PeriodRules {
    /// <summary>
    /// Implements a period rule based on a specified interval of time using <see cref="TimeSpan"/>.
    /// </summary>
    internal class TimeSpanPeriodRule : ITimeSpanPeriodRule {
        private readonly TimeSpan _interval;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanPeriodRule"/> class with a specified interval.
        /// </summary>
        /// <param name="interval">The time interval between occurrences.</param>
        /// <exception cref="ArgumentException">Thrown if the interval is less than or equal to zero.</exception>
        public TimeSpanPeriodRule(TimeSpan interval) {
            if (interval <= TimeSpan.Zero) {
                throw new ArgumentException("The timespan interval must be greater than zero.", nameof(interval));
            }

            _interval = interval;
        }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            if (fromTime == default) {
                throw new ArgumentException("The start time cannot be the default value.", nameof(fromTime));
            }

            return fromTime.Add(_interval);
        }

        /// <inheritdoc />
        public TimeSpan Interval => _interval;
    }
}
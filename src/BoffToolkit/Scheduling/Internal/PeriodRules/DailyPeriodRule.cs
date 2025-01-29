using System;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Internal.PeriodRules {
    /// <summary>
    /// Implements a daily period rule with the option to specify an interval in days.
    /// </summary>
    /// <remarks>
    /// Represents an event that occurs every specified number of days at a given time of day.
    /// </remarks>
    /// <remarks>
    /// Creates a new instance of <see cref="DailyPeriodRule"/> for an event that occurs every n days.
    /// </remarks>
    /// <param name="timeOfDay">The time of day for the event as a <see cref="DateTime"/>.</param>
    /// <param name="daysInterval">The interval in days between occurrences. Default is 1 day.</param>
    internal class DailyPeriodRule(DateTime timeOfDay, int daysInterval = DailyPeriodRule.DefaultInterval) : IDailyPeriodRule {
        private const int DefaultInterval = 1;

        private readonly TimeSpan _timeOfDay = (timeOfDay.TimeOfDay >= TimeSpan.Zero && timeOfDay.TimeOfDay < TimeSpan.FromDays(1))
                ? timeOfDay.TimeOfDay
                : throw new ArgumentException("The time of day must be between 00:00 and 23:59.", nameof(timeOfDay));
        private readonly int _daysInterval = daysInterval > 0
                ? daysInterval
                : throw new ArgumentException("The daily interval must be greater than zero.", nameof(daysInterval));

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            var nextDate = fromTime.Date.Add(_timeOfDay);

            // Adjust the date if the calculated occurrence is in the past
            if (nextDate <= fromTime) {
                nextDate = nextDate.AddDays(_daysInterval);
            }

            return nextDate;
        }

        /// <inheritdoc />
        public TimeSpan TimeOfDay => _timeOfDay;

        /// <inheritdoc />
        public int DaysInterval => _daysInterval;
    }
}
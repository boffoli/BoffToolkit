using System;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Internal.PeriodRules {
    /// <summary>
    /// Implements a daily period rule with the option to specify an interval in days.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of <see cref="DailyPeriodRule"/> for an event that occurs every n days.
    /// </remarks>
    internal class DailyPeriodRule(TimeSpan timeOfDay, int daysInterval) : IDailyPeriodRule {
        private const int DefaultInterval = 1;
        private readonly TimeSpan _timeOfDay = (timeOfDay >= TimeSpan.Zero && timeOfDay < TimeSpan.FromDays(1))
                ? timeOfDay
                : throw new ArgumentException(InvalidTimeOfDayErrorMessage, nameof(timeOfDay));
        private readonly int _daysInterval = daysInterval > 0
                ? daysInterval
                : throw new ArgumentException(InvalidDaysIntervalErrorMessage, nameof(daysInterval));

        // Error message constants
        private const string InvalidTimeOfDayErrorMessage = "The time of day must be between 00:00 and 23:59.";
        private const string InvalidDaysIntervalErrorMessage = "The daily interval must be greater than zero.";
        private const string InvalidFromTimeErrorMessage = "The starting date must be a valid date.";

        /// <summary>
        /// Creates a new instance of <see cref="DailyPeriodRule"/> for a daily event.
        /// </summary>
        /// <param name="timeOfDay">The time of day for the event.</param>
        public DailyPeriodRule(DateTime timeOfDay)
            : this(timeOfDay.TimeOfDay, DefaultInterval) { }

        /// <summary>
        /// Creates a new instance of <see cref="DailyPeriodRule"/> for an event that occurs every n days.
        /// </summary>
        /// <param name="timeOfDay">The time of day for the event.</param>
        /// <param name="daysInterval">The interval in days between occurrences.</param>
        public DailyPeriodRule(DateTime timeOfDay, int daysInterval)
            : this(timeOfDay.TimeOfDay, daysInterval) { }

        /// <summary>
        /// Creates a new instance of <see cref="DailyPeriodRule"/> for a daily event.
        /// </summary>
        /// <param name="timeOfDay">The time of day for the event.</param>
        public DailyPeriodRule(TimeSpan timeOfDay)
            : this(timeOfDay, DefaultInterval) { }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            if (fromTime == default) {
                throw new ArgumentException(InvalidFromTimeErrorMessage, nameof(fromTime));
            }

            var nextDate = fromTime.Date.Add(_timeOfDay);

            // Case: Every n days (including the case where _daysInterval is 1 for daily events)
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
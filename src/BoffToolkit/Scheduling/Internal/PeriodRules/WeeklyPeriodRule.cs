using System;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Internal.PeriodRules {
    /// <summary>
    /// Implements a weekly period rule with the ability to specify an interval of weeks.
    /// </summary>
    internal class WeeklyPeriodRule : IWeeklyPeriodRule {
        private const int DefaultInterval = 1; // Default interval of 1 week
        private readonly DayOfWeek _dayOfWeek;
        private readonly int _weeksInterval;

        // Error message constants
        private const string InvalidDayOfWeekErrorMessage = "The specified day of the week is invalid.";
        private const string InvalidWeeksIntervalErrorMessage = "The weekly interval must be greater than zero.";
        private const string InvalidFromTimeErrorMessage = "The start time cannot be the default value.";

        /// <summary>
        /// Initializes a new instance of the <see cref="WeeklyPeriodRule"/> class for a weekly event with the default interval.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week when the event occurs.</param>
        public WeeklyPeriodRule(DayOfWeek dayOfWeek)
            : this(dayOfWeek, DefaultInterval) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeeklyPeriodRule"/> class for an event that occurs every n weeks.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week when the event occurs.</param>
        /// <param name="weeksInterval">The interval of weeks between occurrences.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="dayOfWeek"/> is invalid or <paramref name="weeksInterval"/> is less than or equal to zero.
        /// </exception>
        public WeeklyPeriodRule(DayOfWeek dayOfWeek, int weeksInterval) {
            if (!Enum.IsDefined(typeof(DayOfWeek), dayOfWeek)) {
                throw new ArgumentException(InvalidDayOfWeekErrorMessage, nameof(dayOfWeek));
            }

            if (weeksInterval <= 0) {
                throw new ArgumentException(InvalidWeeksIntervalErrorMessage, nameof(weeksInterval));
            }

            _dayOfWeek = dayOfWeek;
            _weeksInterval = weeksInterval;
        }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            if (fromTime == default) {
                throw new ArgumentException(InvalidFromTimeErrorMessage, nameof(fromTime));
            }

            var nextDate = fromTime.Date;

            // Find the next occurrence of the specified day of the week
            while (nextDate.DayOfWeek != _dayOfWeek) {
                nextDate = nextDate.AddDays(1);
            }

            // Add the weekly interval if the next date is less than or equal to `fromTime`
            if (nextDate <= fromTime) {
                nextDate = nextDate.AddDays(7 * _weeksInterval);
            }

            return nextDate;
        }

        /// <inheritdoc />
        public DayOfWeek DayOfWeek => _dayOfWeek;

        /// <inheritdoc />
        public int WeeksInterval => _weeksInterval;
    }
}
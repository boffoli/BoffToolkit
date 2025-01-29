using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Internal.PeriodRules {
    /// <summary>
    /// Implements a weekly period rule with the ability to specify an interval of weeks.
    /// </summary>
    internal class WeeklyPeriodRule : IWeeklyPeriodRule {
        private const int DefaultInterval = 1; // Default interval of 1 week

        private readonly DayOfWeek _dayOfWeek;
        private readonly int _weeksInterval;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeeklyPeriodRule"/> class for a weekly event.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week when the event occurs.</param>
        /// <param name="weeksInterval">The interval of weeks between occurrences. Default is 1 week.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="dayOfWeek"/> is invalid or <paramref name="weeksInterval"/> is less than or equal to zero.
        /// </exception>
        public WeeklyPeriodRule(DayOfWeek dayOfWeek, int weeksInterval = DefaultInterval) {
            if (!Enum.IsDefined(typeof(DayOfWeek), dayOfWeek)) {
                throw new ArgumentException("The specified day of the week is invalid.", nameof(dayOfWeek));
            }

            if (weeksInterval <= 0) {
                throw new ArgumentException("The weekly interval must be greater than zero.", nameof(weeksInterval));
            }

            _dayOfWeek = dayOfWeek;
            _weeksInterval = weeksInterval;
        }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
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
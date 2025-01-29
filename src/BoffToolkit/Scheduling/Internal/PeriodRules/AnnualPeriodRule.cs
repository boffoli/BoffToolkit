using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Internal.PeriodRules {
    /// <summary>
    /// Implements an annual period rule with the option to specify an interval in years.
    /// </summary>
    /// <remarks>
    /// Represents an event that occurs every specified number of years on a given month and day.
    /// </remarks>
    /// <remarks>
    /// Creates a new instance of <see cref="AnnualPeriodRule"/> for an annual event with a custom interval.
    /// </remarks>
    /// <param name="month">The month of the year (1-12).</param>
    /// <param name="day">The day of the month (1-31).</param>
    /// <param name="yearsInterval">The interval in years between occurrences. Default is 1 year.</param>
    internal class AnnualPeriodRule(int month, int day, int yearsInterval = AnnualPeriodRule.DefaultInterval) : IAnnualPeriodRule {
        private const int DefaultInterval = 1; // Default interval of 1 year

        private readonly int _month = (month >= 1 && month <= 12)
                ? month
                : throw new ArgumentException("The month must be a value between 1 and 12.", nameof(month));
        private readonly int _day = (day >= 1 && day <= 31)
                ? day
                : throw new ArgumentException("The day must be a value between 1 and 31.", nameof(day));
        private readonly int _yearsInterval = yearsInterval > 0
                ? yearsInterval
                : throw new ArgumentException("The years interval must be greater than zero.", nameof(yearsInterval));

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            // Create the next occurrence based on the specified month and day
            var nextDate = new DateTime(fromTime.Year, _month, _day, 0, 0, 0, fromTime.Kind);

            // If the created date is equal to or earlier than the start date, add the years interval
            if (nextDate <= fromTime) {
                nextDate = nextDate.AddYears(_yearsInterval);
            }

            return nextDate;
        }

        /// <inheritdoc />
        public int Month => _month;

        /// <inheritdoc />
        public int Day => _day;

        /// <inheritdoc />
        public int YearsInterval => _yearsInterval;
    }
}
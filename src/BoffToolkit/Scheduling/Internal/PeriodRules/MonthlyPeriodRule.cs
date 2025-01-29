using System;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Internal.PeriodRules {
    /// <summary>
    /// Implements a monthly period rule, allowing for a specified interval in months.
    /// </summary>
    /// <remarks>
    /// Represents an event that occurs every specified number of months on a given day of the month.
    /// </remarks>
    /// <remarks>
    /// Creates a new instance of <see cref="MonthlyPeriodRule"/> for a monthly event.
    /// </remarks>
    /// <param name="dayOfMonth">The day of the month (1-31).</param>
    /// <param name="monthsInterval">The interval in months between occurrences. Default is 1 month.</param>
    internal class MonthlyPeriodRule(int dayOfMonth, int monthsInterval = MonthlyPeriodRule.DefaultInterval) : IMonthlyPeriodRule {
        private const int DefaultInterval = 1;

        private readonly int _dayOfMonth = (dayOfMonth >= 1 && dayOfMonth <= 31)
                ? dayOfMonth
                : throw new ArgumentException("The day of the month must be between 1 and 31.", nameof(dayOfMonth));
        private readonly int _monthsInterval = monthsInterval > 0
                ? monthsInterval
                : throw new ArgumentException("The interval in months must be greater than zero.", nameof(monthsInterval));

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            var nextDate = CalculateNextDate(fromTime);

            // If the next date is less than or equal to fromTime, advance by the interval of months
            if (nextDate <= fromTime) {
                nextDate = nextDate.AddMonths(_monthsInterval);
                nextDate = CalculateNextDate(nextDate);
            }

            return nextDate;
        }

        /// <summary>
        /// Calculates the next valid date while preserving the DateTimeKind.
        /// </summary>
        /// <param name="fromTime">The reference date.</param>
        /// <returns>A valid next occurrence date.</returns>
        private DateTime CalculateNextDate(DateTime fromTime) {
            try {
                var day = ValidateDayInMonth(fromTime.Year, fromTime.Month, _dayOfMonth);
                return new DateTime(fromTime.Year, fromTime.Month, day, 0, 0, 0, fromTime.Kind);
            }
            catch (ArgumentOutOfRangeException ex) {
                throw new InvalidOperationException("Error calculating the next date: the combination of year, month, and day is invalid.", ex);
            }
        }

        /// <summary>
        /// Validates and adjusts the provided day to the maximum number of days in the specified month.
        /// </summary>
        /// <param name="year">The year of the date.</param>
        /// <param name="month">The month of the date.</param>
        /// <param name="day">The requested day.</param>
        /// <returns>A valid day for the specified month.</returns>
        private static int ValidateDayInMonth(int year, int month, int day) {
            var maxDaysInMonth = DateTime.DaysInMonth(year, month);
            return day <= maxDaysInMonth ? day : maxDaysInMonth;
        }

        /// <inheritdoc />
        public int DayOfMonth => _dayOfMonth;

        /// <inheritdoc />
        public int MonthsInterval => _monthsInterval;
    }
}
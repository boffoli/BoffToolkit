using System;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Internal.PeriodRules {
    /// <summary>
    /// Implements a monthly period rule, allowing for a specified interval in months.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of <see cref="MonthlyPeriodRule"/> for an event that occurs every n months.
    /// </remarks>
    internal class MonthlyPeriodRule(int dayOfMonth, int monthsInterval) : IMonthlyPeriodRule {
        private const int DefaultInterval = 1;

        private readonly int _dayOfMonth = (dayOfMonth >= 1 && dayOfMonth <= 31)
                ? dayOfMonth
                : throw new ArgumentException(InvalidDayOfMonthErrorMessage, nameof(dayOfMonth));
        private readonly int _monthsInterval = (monthsInterval > 0)
                ? monthsInterval
                : throw new ArgumentException(InvalidMonthsIntervalErrorMessage, nameof(monthsInterval));

        // Error message constants
        private const string InvalidDayOfMonthErrorMessage = "The day of the month must be between 1 and 31.";
        private const string InvalidMonthsIntervalErrorMessage = "The interval in months must be greater than zero.";
        private const string InvalidFromTimeErrorMessage = "The starting date must be a valid date.";
        private const string CalculateNextDateErrorMessage = "Error calculating the next date: the combination of year, month, and day is invalid.";

        /// <summary>
        /// Creates a new instance of <see cref="MonthlyPeriodRule"/> for a monthly event.
        /// </summary>
        /// <param name="dayOfMonth">The day of the month (1-31).</param>
        public MonthlyPeriodRule(int dayOfMonth)
            : this(dayOfMonth, DefaultInterval) { }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            if (fromTime == default) {
                throw new ArgumentException(InvalidFromTimeErrorMessage, nameof(fromTime));
            }

            var nextDate = CalculateNextDate(fromTime);

            // If the next date is less than or equal to fromTime, advance by the interval of months
            if (nextDate <= fromTime) {
                nextDate = nextDate.AddMonths(_monthsInterval);
                nextDate = CalculateNextDate(nextDate);
            }

            return nextDate;
        }

        // Method to calculate the next date while preserving the DateTimeKind
        private DateTime CalculateNextDate(DateTime fromTime) {
            try {
                var day = ValidateDayInMonth(fromTime.Year, fromTime.Month, _dayOfMonth);
                return new DateTime(fromTime.Year, fromTime.Month, day, 0, 0, 0, fromTime.Kind);
            }
            catch (ArgumentOutOfRangeException ex) {
                throw new InvalidOperationException(CalculateNextDateErrorMessage, ex);
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

            // If the day exceeds the month's maximum, return the valid maximum
            return day <= maxDaysInMonth ? day : maxDaysInMonth;
        }

        /// <inheritdoc />
        public int DayOfMonth => _dayOfMonth;

        /// <inheritdoc />
        public int MonthsInterval => _monthsInterval;
    }
}
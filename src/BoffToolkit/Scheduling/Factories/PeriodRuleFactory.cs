using System;
using BoffToolkit.Scheduling.Internal.PeriodRules;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Factories {
    /// <summary>
    /// Factory for creating instances of period rules.
    /// </summary>
    public static class PeriodRuleFactory {
        /// <summary>
        /// Creates an annual period rule.
        /// </summary>
        /// <param name="month">The month of the year (1-12).</param>
        /// <param name="day">The day of the month (1-31).</param>
        /// <param name="yearsInterval">The interval in years between occurrences. Default value: 1.</param>
        /// <returns>An instance of <see cref="IAnnualPeriodRule"/>.</returns>
        public static IAnnualPeriodRule CreateAnnualPeriodRule(int month, int day, int yearsInterval = 1) {
            return new AnnualPeriodRule(month, day, yearsInterval);
        }

        /// <summary>
        /// Creates a daily period rule.
        /// </summary>
        /// <param name="timeOfDay">The time of the day.</param>
        /// <param name="daysInterval">The interval in days between occurrences. Default value: 1.</param>
        /// <returns>An instance of <see cref="IDailyPeriodRule"/>.</returns>
        public static IDailyPeriodRule CreateDailyPeriodRule(TimeSpan timeOfDay, int daysInterval = 1) {
            return new DailyPeriodRule(timeOfDay, daysInterval);
        }

        /// <summary>
        /// Creates a weekly period rule.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week.</param>
        /// <param name="weeksInterval">The interval in weeks between occurrences. Default value: 1.</param>
        /// <returns>An instance of <see cref="IWeeklyPeriodRule"/>.</returns>
        public static IWeeklyPeriodRule CreateWeeklyPeriodRule(DayOfWeek dayOfWeek, int weeksInterval = 1) {
            return new WeeklyPeriodRule(dayOfWeek, weeksInterval);
        }

        /// <summary>
        /// Creates a monthly period rule.
        /// </summary>
        /// <param name="dayOfMonth">The day of the month (1-31).</param>
        /// <param name="monthsInterval">The interval in months between occurrences. Default value: 1.</param>
        /// <returns>An instance of <see cref="IMonthlyPeriodRule"/>.</returns>
        public static IMonthlyPeriodRule CreateMonthlyPeriodRule(int dayOfMonth, int monthsInterval = 1) {
            return new MonthlyPeriodRule(dayOfMonth, monthsInterval);
        }

        /// <summary>
        /// Creates a period rule based on a time span.
        /// </summary>
        /// <param name="interval">The time span interval between occurrences.</param>
        /// <returns>An instance of <see cref="ITimeSpanPeriodRule"/>.</returns>
        public static ITimeSpanPeriodRule CreateTimeSpanPeriodRule(TimeSpan interval) {
            return new TimeSpanPeriodRule(interval);
        }
    }
}
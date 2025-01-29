using BoffToolkit.Scheduling.Internal.PeriodRules;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Factories {
    /// <summary>
    /// Factory for creating instances of period rules.
    /// </summary>
    public static class PeriodRuleFactory {
        /// <summary>
        /// Creates an annual period rule with a default interval of 1 year.
        /// </summary>
        /// <param name="month">The month of the year (1-12).</param>
        /// <param name="day">The day of the month (1-31).</param>
        /// <returns>An instance of <see cref="IAnnualPeriodRule"/>.</returns>
        public static IAnnualPeriodRule CreateAnnualPeriodRule(int month, int day) {
            return new AnnualPeriodRule(month, day);
        }

        /// <summary>
        /// Creates an annual period rule with a custom interval in years.
        /// </summary>
        /// <param name="month">The month of the year (1-12).</param>
        /// <param name="day">The day of the month (1-31).</param>
        /// <param name="yearsInterval">The interval in years between occurrences.</param>
        /// <returns>An instance of <see cref="IAnnualPeriodRule"/>.</returns>
        public static IAnnualPeriodRule CreateAnnualPeriodRule(int month, int day, int yearsInterval) {
            return new AnnualPeriodRule(month, day, yearsInterval);
        }

        /// <summary>
        /// Creates a daily period rule using a <see cref="DateTime"/> object for the time of day with a default interval of 1 day.
        /// </summary>
        /// <param name="timeOfDay">The time of day for the event.</param>
        /// <returns>An instance of <see cref="IDailyPeriodRule"/>.</returns>
        public static IDailyPeriodRule CreateDailyPeriodRule(DateTime timeOfDay) {
            return new DailyPeriodRule(timeOfDay);
        }

        /// <summary>
        /// Creates a daily period rule using a <see cref="DateTime"/> object for the time of day and a custom days interval.
        /// </summary>
        /// <param name="timeOfDay">The time of day for the event.</param>
        /// <param name="daysInterval">The interval in days between occurrences.</param>
        /// <returns>An instance of <see cref="IDailyPeriodRule"/>.</returns>
        public static IDailyPeriodRule CreateDailyPeriodRule(DateTime timeOfDay, int daysInterval) {
            return new DailyPeriodRule(timeOfDay, daysInterval);
        }

        /// <summary>
        /// Creates a weekly period rule with a default interval of 1 week.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week.</param>
        /// <returns>An instance of <see cref="IWeeklyPeriodRule"/>.</returns>
        public static IWeeklyPeriodRule CreateWeeklyPeriodRule(DayOfWeek dayOfWeek) {
            return new WeeklyPeriodRule(dayOfWeek);
        }

        /// <summary>
        /// Creates a weekly period rule with a custom interval in weeks.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week.</param>
        /// <param name="weeksInterval">The interval in weeks between occurrences.</param>
        /// <returns>An instance of <see cref="IWeeklyPeriodRule"/>.</returns>
        public static IWeeklyPeriodRule CreateWeeklyPeriodRule(DayOfWeek dayOfWeek, int weeksInterval) {
            return new WeeklyPeriodRule(dayOfWeek, weeksInterval);
        }

        /// <summary>
        /// Creates a monthly period rule with a default interval of 1 month.
        /// </summary>
        /// <param name="dayOfMonth">The day of the month (1-31).</param>
        /// <returns>An instance of <see cref="IMonthlyPeriodRule"/>.</returns>
        public static IMonthlyPeriodRule CreateMonthlyPeriodRule(int dayOfMonth) {
            return new MonthlyPeriodRule(dayOfMonth);
        }

        /// <summary>
        /// Creates a monthly period rule with a custom interval in months.
        /// </summary>
        /// <param name="dayOfMonth">The day of the month (1-31).</param>
        /// <param name="monthsInterval">The interval in months between occurrences.</param>
        /// <returns>An instance of <see cref="IMonthlyPeriodRule"/>.</returns>
        public static IMonthlyPeriodRule CreateMonthlyPeriodRule(int dayOfMonth, int monthsInterval) {
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
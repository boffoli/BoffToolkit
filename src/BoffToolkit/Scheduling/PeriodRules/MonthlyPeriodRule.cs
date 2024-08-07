using System;

namespace BoffToolkit.Scheduling.PeriodRules {
    /// <summary>
    /// Implementa una regola di periodo mensile, con possibilità di specificare un intervallo di mesi.
    /// </summary>
    public class MonthlyPeriodRule : IPeriodRule {
        private readonly int _dayOfMonth;
        private readonly int? _monthsInterval;

        /// <summary>
        /// Crea una nuova istanza di <see cref="MonthlyPeriodRule"/> per un evento mensile.
        /// </summary>
        /// <param name="dayOfMonth">Il giorno del mese (1-31).</param>
        public MonthlyPeriodRule(int dayOfMonth)
            : this(dayOfMonth, null) { }

        /// <summary>
        /// Crea una nuova istanza di <see cref="MonthlyPeriodRule"/> per un evento che si verifica ogni n mesi.
        /// </summary>
        /// <param name="dayOfMonth">Il giorno del mese (1-31).</param>
        /// <param name="monthsInterval">L'intervallo di mesi tra le occorrenze.</param>
        public MonthlyPeriodRule(int dayOfMonth, int monthsInterval)
            : this(dayOfMonth, (int?)monthsInterval) { }

        // Costruttore privato per gestire l'inizializzazione.
        private MonthlyPeriodRule(int dayOfMonth, int? monthsInterval) {
            if (dayOfMonth < 1 || dayOfMonth > 31)
                throw new ArgumentException("Il giorno del mese deve essere tra 1 e 31.", nameof(dayOfMonth));
            if (monthsInterval.HasValue && monthsInterval.Value <= 0)
                throw new ArgumentException("L'intervallo deve essere maggiore di zero.", nameof(monthsInterval));

            _dayOfMonth = dayOfMonth;
            _monthsInterval = monthsInterval;
        }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            if (fromTime == default)
                throw new ArgumentException("La data di partenza deve essere una data valida.", nameof(fromTime));

            DateTime nextDate = CalculateNextDate(fromTime);

            // Se la prossima data è minore o uguale a fromTime, avanzare di un intervallo di mesi
            if (nextDate <= fromTime) {
                nextDate = nextDate.AddMonths(_monthsInterval ?? 1);
                nextDate = CalculateNextDate(nextDate);
            }

            return nextDate;
        }

        // Metodo per calcolare la data successiva
        private DateTime CalculateNextDate(DateTime fromTime) {
            DateTime nextDate = new DateTime(fromTime.Year, fromTime.Month, _dayOfMonth);

            // Se il giorno specificato non esiste nel mese successivo, usa l'ultimo giorno del mese
            if (nextDate.Month != fromTime.Month) {
                nextDate = new DateTime(nextDate.Year, nextDate.Month, 1).AddMonths(1).AddDays(-1);
            }

            // Gestisci il caso in cui il giorno del mese specificato è oltre l'ultimo giorno del mese successivo
            if (nextDate.Day > DateTime.DaysInMonth(nextDate.Year, nextDate.Month)) {
                nextDate = new DateTime(nextDate.Year, nextDate.Month, DateTime.DaysInMonth(nextDate.Year, nextDate.Month));
            }

            return nextDate;
        }
    }
}
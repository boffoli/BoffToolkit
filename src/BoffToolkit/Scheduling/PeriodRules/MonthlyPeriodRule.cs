using System;

namespace BoffToolkit.Scheduling.PeriodRules {
    /// <summary>
    /// Implementa una regola di periodo mensile, con possibilità di specificare un intervallo di mesi.
    /// </summary>
    /// <remarks>
    /// Crea una nuova istanza di <see cref="MonthlyPeriodRule"/> per un evento che si verifica ogni n mesi.
    /// </remarks>
    /// <param name="dayOfMonth">Il giorno del mese (1-31).</param>
    /// <param name="monthsInterval">L'intervallo di mesi tra le occorrenze.</param>
    public class MonthlyPeriodRule(int dayOfMonth, int monthsInterval) : IPeriodRule {
        private const int DefaultInterval = 1;

        private readonly int _dayOfMonth = (dayOfMonth >= 1 && dayOfMonth <= 31)
                ? dayOfMonth
                : throw new ArgumentException(InvalidDayOfMonthErrorMessage, nameof(dayOfMonth));
        private readonly int _monthsInterval = (monthsInterval > 0)
                ? monthsInterval
                : throw new ArgumentException(InvalidMonthsIntervalErrorMessage, nameof(monthsInterval));

        // Definizione delle costanti per i messaggi di errore
        private const string InvalidDayOfMonthErrorMessage = "Il giorno del mese deve essere tra 1 e 31.";
        private const string InvalidMonthsIntervalErrorMessage = "L'intervallo dei mesi deve essere maggiore di zero.";
        private const string InvalidFromTimeErrorMessage = "La data di partenza deve essere una data valida.";
        private const string CalculateNextDateErrorMessage = "Errore nel calcolo della data successiva: la combinazione di anno, mese e giorno non è valida.";

        /// <summary>
        /// Crea una nuova istanza di <see cref="MonthlyPeriodRule"/> per un evento mensile.
        /// </summary>
        /// <param name="dayOfMonth">Il giorno del mese (1-31).</param>
        public MonthlyPeriodRule(int dayOfMonth)
            : this(dayOfMonth, DefaultInterval) { }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            if (fromTime == default) {
                throw new ArgumentException(InvalidFromTimeErrorMessage, nameof(fromTime));
            }

            var nextDate = CalculateNextDate(fromTime);

            // Se la prossima data è minore o uguale a fromTime, avanzare di un intervallo di mesi
            if (nextDate <= fromTime) {
                nextDate = nextDate.AddMonths(_monthsInterval);
                nextDate = CalculateNextDate(nextDate);
            }

            return nextDate;
        }

        // Metodo per calcolare la data successiva, mantenendo il DateTimeKind
        private DateTime CalculateNextDate(DateTime fromTime) {
            // Calcola il numero massimo di giorni nel mese corrente
            var maxDaysInMonth = DateTime.DaysInMonth(fromTime.Year, fromTime.Month);

            // Usa Math.Min per assicurarsi che il giorno non superi il numero massimo di giorni nel mese
            var day = Math.Min(_dayOfMonth, maxDaysInMonth);

            // Preserva il DateTimeKind e gestisce eventuali ArgumentOutOfRangeException
            try {
                var nextDate = new DateTime(fromTime.Year, fromTime.Month, day, 0, 0, 0, fromTime.Kind);
                return nextDate;
            }
            catch (ArgumentOutOfRangeException ex) {
                throw new InvalidOperationException(CalculateNextDateErrorMessage, ex);
            }
        }
    }
}
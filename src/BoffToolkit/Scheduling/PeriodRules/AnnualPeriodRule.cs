using System;

namespace BoffToolkit.Scheduling.PeriodRules {
    /// <summary>
    /// Implementa una regola di periodo annuale, con possibilità di specificare un intervallo di anni.
    /// </summary>
    public class AnnualPeriodRule : IPeriodRule {
        private readonly int _month;
        private readonly int _day;
        private readonly int? _yearsInterval;

        // Definizione delle costanti per i messaggi di errore
        private const string InvalidMonthErrorMessage = "Il mese deve essere un valore compreso tra 1 e 12.";
        private const string InvalidDayErrorMessage = "Il giorno deve essere un valore compreso tra 1 e 31.";
        private const string InvalidYearsIntervalErrorMessage = "L'intervallo di anni deve essere maggiore di zero.";
        private const string InvalidFromTimeErrorMessage = "La data di partenza deve essere una data valida.";

        /// <summary>
        /// Crea una nuova istanza di <see cref="AnnualPeriodRule"/> per un evento annuale.
        /// </summary>
        /// <param name="month">Il mese dell'anno (1-12).</param>
        /// <param name="day">Il giorno del mese (1-31).</param>
        public AnnualPeriodRule(int month, int day)
            : this(month, day, null) { }

        /// <summary>
        /// Crea una nuova istanza di <see cref="AnnualPeriodRule"/> per un evento che si verifica ogni n anni.
        /// </summary>
        /// <param name="month">Il mese dell'anno (1-12).</param>
        /// <param name="day">Il giorno del mese (1-31).</param>
        /// <param name="yearsInterval">L'intervallo di anni tra le occorrenze.</param>
        public AnnualPeriodRule(int month, int day, int yearsInterval)
            : this(month, day, (int?)yearsInterval) { }

        // Costruttore privato per gestire l'inizializzazione.
        private AnnualPeriodRule(int month, int day, int? yearsInterval) {
            _month = (month >= 1 && month <= 12) ? month : throw new ArgumentException(InvalidMonthErrorMessage, nameof(month));
            _day = (day >= 1 && day <= 31) ? day : throw new ArgumentException(InvalidDayErrorMessage, nameof(day));
            _yearsInterval = (yearsInterval.HasValue && yearsInterval.Value > 0) ? yearsInterval : throw new ArgumentException(InvalidYearsIntervalErrorMessage, nameof(yearsInterval));
        }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            if (fromTime == default) {
                throw new ArgumentException(InvalidFromTimeErrorMessage, nameof(fromTime));
            }

            // Usa il costruttore di DateTime con il parametro DateTimeKind
            var nextDate = new DateTime(fromTime.Year, _month, _day, 0, 0, 0, fromTime.Kind);

            if (nextDate <= fromTime) {
                // Caso: Ogni n anni
                nextDate = _yearsInterval.HasValue ? nextDate.AddYears(_yearsInterval.Value) : nextDate.AddYears(1);
            }

            return nextDate;
        }
    }
}
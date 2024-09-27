using System;

namespace BoffToolkit.Scheduling.PeriodRules {
    /// <summary>
    /// Implementa una regola di periodo annuale, con possibilità di specificare un intervallo di anni.
    /// </summary>
    /// <remarks>
    /// Crea una nuova istanza di <see cref="AnnualPeriodRule"/> per un evento che si verifica ogni n anni.
    /// </remarks>
    /// <param name="month">Il mese dell'anno (1-12).</param>
    /// <param name="day">Il giorno del mese (1-31).</param>
    /// <param name="yearsInterval">L'intervallo di anni tra le occorrenze.</param>
    public class AnnualPeriodRule(int month, int day, int yearsInterval) : IPeriodRule {
        private const int DefaultInterval = 1; // Intervallo annuale di default
        private readonly int _month = (month >= 1 && month <= 12) ? month : throw new ArgumentException(InvalidMonthErrorMessage, nameof(month));
        private readonly int _day = (day >= 1 && day <= 31) ? day : throw new ArgumentException(InvalidDayErrorMessage, nameof(day));
        private readonly int _yearsInterval = (yearsInterval > 0) ? yearsInterval : throw new ArgumentException(InvalidYearsIntervalErrorMessage, nameof(yearsInterval));

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
            : this(month, day, DefaultInterval) { }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            if (fromTime == default) {
                throw new ArgumentException(InvalidFromTimeErrorMessage, nameof(fromTime));
            }

            // Crea la prossima occorrenza basata sul mese e giorno specificato
            var nextDate = new DateTime(fromTime.Year, _month, _day, 0, 0, 0, fromTime.Kind);

            // Se la data creata è uguale o precedente alla data di partenza, aggiungi l'intervallo di anni
            if (nextDate <= fromTime) {
                nextDate = nextDate.AddYears(_yearsInterval);
            }

            return nextDate;
        }
    }
}
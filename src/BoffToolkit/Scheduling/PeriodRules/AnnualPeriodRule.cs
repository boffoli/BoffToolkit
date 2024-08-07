using System;

namespace BoffToolkit.Scheduling.PeriodRules
{
    /// <summary>
    /// Implementa una regola di periodo annuale, con possibilità di specificare un intervallo di anni.
    /// </summary>
    public class AnnualPeriodRule : IPeriodRule
    {
        private readonly int _month;
        private readonly int _day;
        private readonly int? _yearsInterval;

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
        private AnnualPeriodRule(int month, int day, int? yearsInterval)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Il mese deve essere un valore compreso tra 1 e 12.", nameof(month));
            if (day < 1 || day > 31)
                throw new ArgumentException("Il giorno deve essere un valore compreso tra 1 e 31.", nameof(day));
            if (yearsInterval.HasValue && yearsInterval.Value <= 0)
                throw new ArgumentException("L'intervallo di anni deve essere maggiore di zero.", nameof(yearsInterval));

            _month = month;
            _day = day;
            _yearsInterval = yearsInterval;
        }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime)
        {
            if (fromTime == default)
                throw new ArgumentException("La data di partenza deve essere una data valida.", nameof(fromTime));

            DateTime nextDate = new DateTime(fromTime.Year, _month, _day);

            if (nextDate <= fromTime)
            {
                // Caso: Ogni n anni
                if (_yearsInterval.HasValue)
                {
                    nextDate = nextDate.AddYears(_yearsInterval.Value);
                }
                else
                {
                    nextDate = nextDate.AddYears(1);
                }
            }

            return nextDate;
        }
    }
}
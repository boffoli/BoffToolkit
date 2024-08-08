using System;

namespace BoffToolkit.Scheduling.PeriodRules
{
    /// <summary>
    /// Implementa una regola di periodo giornaliero, con possibilità di specificare un intervallo di giorni.
    /// </summary>
    public class DailyPeriodRule : IPeriodRule
    {
        private readonly TimeSpan _timeOfDay;
        private readonly int? _daysInterval;

        /// <summary>
        /// Crea una nuova istanza di <see cref="DailyPeriodRule"/> per un evento giornaliero.
        /// </summary>
        /// <param name="timeOfDay">L'ora del giorno.</param>
        public DailyPeriodRule(DateTime timeOfDay)
            : this(timeOfDay.TimeOfDay, null) { }

        /// <summary>
        /// Crea una nuova istanza di <see cref="DailyPeriodRule"/> per un evento che si verifica ogni n giorni.
        /// </summary>
        /// <param name="timeOfDay">L'ora del giorno.</param>
        /// <param name="daysInterval">L'intervallo di giorni tra le occorrenze.</param>
        public DailyPeriodRule(DateTime timeOfDay, int daysInterval)
            : this(timeOfDay.TimeOfDay, daysInterval) { }

        /// <summary>
        /// Crea una nuova istanza di <see cref="DailyPeriodRule"/> per un evento giornaliero.
        /// </summary>
        /// <param name="timeOfDay">L'orario del giorno.</param>
        public DailyPeriodRule(TimeSpan timeOfDay)
            : this(timeOfDay, null) { }

        /// <summary>
        /// Crea una nuova istanza di <see cref="DailyPeriodRule"/> per un evento che si verifica ogni n giorni.
        /// </summary>
        /// <param name="timeOfDay">L'orario del giorno.</param>
        /// <param name="daysInterval">L'intervallo di giorni tra le occorrenze.</param>
        public DailyPeriodRule(TimeSpan timeOfDay, int daysInterval)
            : this(timeOfDay, (int?)daysInterval) { }

        // Costruttore privato per gestire l'inizializzazione.
        private DailyPeriodRule(TimeSpan timeOfDay, int? daysInterval)
        {
            if (timeOfDay < TimeSpan.Zero || timeOfDay >= TimeSpan.FromDays(1))
                throw new ArgumentException("L'orario deve essere compreso tra 00:00 e 23:59.", nameof(timeOfDay));
            if (daysInterval.HasValue && daysInterval.Value <= 0)
                throw new ArgumentException("L'intervallo deve essere maggiore di zero.", nameof(daysInterval));

            _timeOfDay = timeOfDay;
            _daysInterval = daysInterval;
        }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime)
        {
            if (fromTime == default)
                throw new ArgumentException("La data di partenza deve essere una data valida.", nameof(fromTime));

            DateTime nextDate = fromTime.Date.Add(_timeOfDay);

            if (_daysInterval.HasValue)
            {
                // Caso: Ogni n giorni
                if (nextDate <= fromTime)
                {
                    nextDate = nextDate.AddDays(_daysInterval.Value);
                }
            }
            else
            {
                // Caso: Ogni giorno
                if (nextDate <= fromTime)
                {
                    nextDate = nextDate.AddDays(1);
                }
            }

            return nextDate;
        }
    }
}
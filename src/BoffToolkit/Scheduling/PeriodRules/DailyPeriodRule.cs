using System;

namespace BoffToolkit.Scheduling.PeriodRules {
    /// <summary>
    /// Implementa una regola di periodo giornaliero, con possibilità di specificare un intervallo di giorni.
    /// </summary>
    public class DailyPeriodRule : IPeriodRule {
        private const int DefaultInterval = 1;
        private readonly TimeSpan _timeOfDay;
        private readonly int _daysInterval; // evento giornaliero di default

        // Definizione delle costanti per i messaggi di errore
        private const string InvalidTimeOfDayErrorMessage = "L'orario deve essere compreso tra 00:00 e 23:59.";
        private const string InvalidDaysIntervalErrorMessage = "L'intervallo giornaliero deve essere maggiore di zero.";
        private const string InvalidFromTimeErrorMessage = "La data di partenza deve essere una data valida.";

        /// <summary>
        /// Crea una nuova istanza di <see cref="DailyPeriodRule"/> per un evento giornaliero.
        /// </summary>
        /// <param name="timeOfDay">L'ora del giorno.</param>
        public DailyPeriodRule(DateTime timeOfDay)
            : this(timeOfDay.TimeOfDay, DefaultInterval) { }

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
            : this(timeOfDay, DefaultInterval) { }

        /// <summary>
        /// Crea una nuova istanza di <see cref="DailyPeriodRule"/> per un evento che si verifica ogni n giorni.
        /// </summary>
        /// <param name="timeOfDay">L'orario del giorno.</param>
        /// <param name="daysInterval">L'intervallo di giorni tra le occorrenze.</param>
        public DailyPeriodRule(TimeSpan timeOfDay, int daysInterval) {
            _timeOfDay = (timeOfDay >= TimeSpan.Zero && timeOfDay < TimeSpan.FromDays(1)) ? timeOfDay : throw new ArgumentException(InvalidTimeOfDayErrorMessage, nameof(timeOfDay));
            if (daysInterval > 0) {
                _daysInterval = daysInterval;
            }
            else {
                throw new ArgumentException(InvalidDaysIntervalErrorMessage, nameof(daysInterval));
            }
        }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            if (fromTime == default) {
                throw new ArgumentException(InvalidFromTimeErrorMessage, nameof(fromTime));
            }

            var nextDate = fromTime.Date.Add(_timeOfDay);

            // Caso: Ogni n giorni (compreso il caso in cui _daysInterval è 1 per giornaliero)
            nextDate = (nextDate <= fromTime) ? nextDate.AddDays(_daysInterval) : nextDate;

            return nextDate;
        }
    }
}
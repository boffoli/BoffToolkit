using System;

namespace BoffToolkit.Scheduling.PeriodRules {
    /// <summary>
    /// Implementa una regola di periodo settimanale, con possibilità di specificare un intervallo di settimane.
    /// </summary>
    public class WeeklyPeriodRule : IPeriodRule {
        private const int DefaultInterval = 1; // Intervallo predefinito di 1 settimana
        private readonly DayOfWeek _dayOfWeek;
        private readonly int _weeksInterval;

        // Costanti per i messaggi di errore
        private const string InvalidDayOfWeekErrorMessage = "Il giorno della settimana non è valido.";
        private const string InvalidWeeksIntervalErrorMessage = "L'intervallo settimanale deve essere maggiore di zero.";
        private const string InvalidFromTimeErrorMessage = "L'orario di partenza non può essere il valore predefinito.";

        /// <summary>
        /// Crea una nuova istanza di <see cref="WeeklyPeriodRule"/> per un evento settimanale con intervallo predefinito.
        /// </summary>
        /// <param name="dayOfWeek">Il giorno della settimana.</param>
        public WeeklyPeriodRule(DayOfWeek dayOfWeek)
            : this(dayOfWeek, DefaultInterval) { }

        /// <summary>
        /// Crea una nuova istanza di <see cref="WeeklyPeriodRule"/> per un evento che si verifica ogni n settimane.
        /// </summary>
        /// <param name="dayOfWeek">Il giorno della settimana.</param>
        /// <param name="weeksInterval">L'intervallo di settimane tra le occorrenze.</param>
        public WeeklyPeriodRule(DayOfWeek dayOfWeek, int weeksInterval) {
            if (!Enum.IsDefined(typeof(DayOfWeek), dayOfWeek)) {
                throw new ArgumentException(InvalidDayOfWeekErrorMessage, nameof(dayOfWeek));
            }

            if (weeksInterval <= 0) {
                throw new ArgumentException(InvalidWeeksIntervalErrorMessage, nameof(weeksInterval));
            }

            _dayOfWeek = dayOfWeek;
            _weeksInterval = weeksInterval;
        }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            if (fromTime == default) {
                throw new ArgumentException(InvalidFromTimeErrorMessage, nameof(fromTime));
            }

            var nextDate = fromTime.Date;

            // Trova il prossimo giorno della settimana specificato
            while (nextDate.DayOfWeek != _dayOfWeek) {
                nextDate = nextDate.AddDays(1);
            }

            // Aggiungi l'intervallo di settimane se la data successiva è uguale o precedente a `fromTime`
            if (nextDate <= fromTime) {
                nextDate = nextDate.AddDays(7 * _weeksInterval);
            }

            return nextDate;
        }
    }
}
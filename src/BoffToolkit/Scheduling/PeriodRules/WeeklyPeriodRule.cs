using System;

namespace BoffToolkit.Scheduling.PeriodRules
{
    /// <summary>
    /// Implementa una regola di periodo settimanale, con possibilità di specificare un intervallo di settimane.
    /// </summary>
    public class WeeklyPeriodRule : IPeriodRule
    {
        private readonly DayOfWeek _dayOfWeek;
        private readonly int? _weeksInterval;

        /// <summary>
        /// Crea una nuova istanza di <see cref="WeeklyPeriodRule"/> per un evento settimanale.
        /// </summary>
        /// <param name="dayOfWeek">Il giorno della settimana.</param>
        public WeeklyPeriodRule(DayOfWeek dayOfWeek)
            : this(dayOfWeek, null) { }

        /// <summary>
        /// Crea una nuova istanza di <see cref="WeeklyPeriodRule"/> per un evento che si verifica ogni n settimane.
        /// </summary>
        /// <param name="dayOfWeek">Il giorno della settimana.</param>
        /// <param name="weeksInterval">L'intervallo di settimane tra le occorrenze.</param>
        public WeeklyPeriodRule(DayOfWeek dayOfWeek, int weeksInterval)
            : this(dayOfWeek, (int?)weeksInterval) { }

        // Costruttore privato per gestire l'inizializzazione.
        private WeeklyPeriodRule(DayOfWeek dayOfWeek, int? weeksInterval)
        {
            if (!Enum.IsDefined(typeof(DayOfWeek), dayOfWeek))
                throw new ArgumentException("Il giorno della settimana non è valido.", nameof(dayOfWeek));
            if (weeksInterval.HasValue && weeksInterval.Value <= 0)
                throw new ArgumentException("L'intervallo deve essere maggiore di zero.", nameof(weeksInterval));

            _dayOfWeek = dayOfWeek;
            _weeksInterval = weeksInterval;
        }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime)
        {
            if (fromTime == default)
                throw new ArgumentException("L'orario di partenza non può essere il valore predefinito.", nameof(fromTime));

            DateTime nextDate = fromTime.Date;
            while (nextDate.DayOfWeek != _dayOfWeek)
            {
                nextDate = nextDate.AddDays(1);
            }

            if (_weeksInterval.HasValue)
            {
                // Caso: Ogni n settimane
                if (nextDate <= fromTime)
                {
                    nextDate = nextDate.AddDays(7 * _weeksInterval.Value);
                }
            }
            else
            {
                // Caso: Ogni settimana
                if (nextDate <= fromTime)
                {
                    nextDate = nextDate.AddDays(7);
                }
            }

            return nextDate;
        }
    }
}
using System;

namespace BoffToolkit.Scheduling.PeriodRules {
    /// <summary>
    /// Implementa una regola di periodo basata su un intervallo di tempo specificato in TimeSpan.
    /// </summary>
    public class TimeSpanPeriodRule : IPeriodRule {
        private readonly TimeSpan _interval;

        /// <summary>
        /// Crea una nuova istanza di <see cref="TimeSpanPeriodRule"/> con un intervallo specificato.
        /// </summary>
        /// <param name="interval">L'intervallo di tempo tra le occorrenze.</param>
        public TimeSpanPeriodRule(TimeSpan interval) {
            if (interval <= TimeSpan.Zero) {
                throw new ArgumentException("L'intervallo deve essere maggiore di zero.", nameof(interval));
            }

            _interval = interval;
        }

        /// <inheritdoc />
        public DateTime GetNextOccurrence(DateTime fromTime) {
            if (fromTime == default) {
                throw new ArgumentException("L'orario di partenza non può essere il valore predefinito.", nameof(fromTime));
            }

            return fromTime.Add(_interval);
        }
    }
}
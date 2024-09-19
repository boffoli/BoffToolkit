using System;
using BoffToolkit.Scheduling.PeriodRules;
using BoffToolkit.Scheduling.Internal.Callbacks;

namespace BoffToolkit.Scheduling.Internal {
    /// <summary>
    /// Contesto del JobScheduler che mantiene le informazioni necessarie per la schedulazione.
    /// </summary>
    internal class JobSchedulerContext {
        /// <summary>
        /// Tempo di inizio del job.
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        /// Regola del periodo per il job.
        /// </summary>
        public IPeriodRule PeriodRule { get; }

        /// <summary>
        /// Adattatore di callback per eseguire le azioni schedulate.
        /// </summary>
        public ICallbackAdapter CallbackAdapter { get; }

        /// <summary>
        /// Indica se il job deve essere eseguito in background.
        /// </summary>
        public bool IsBackground { get; }

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="JobSchedulerContext "/>.
        /// </summary>
        /// <param name="startTime">Il tempo di inizio del job.</param>
        /// <param name="periodRule">La regola del periodo per il job.</param>
        /// <param name="callbackAdapter">L'adattatore di callback.</param>
        /// <param name="isBackground">Indica se il job deve essere eseguito in background.</param>
        public JobSchedulerContext(DateTime startTime, IPeriodRule periodRule, ICallbackAdapter callbackAdapter, bool isBackground) {
            if (startTime == default) {
                throw new ArgumentException("Il tempo di inizio deve essere una data valida.", nameof(startTime));
            }

            PeriodRule = periodRule ?? throw new ArgumentNullException(nameof(periodRule), "La regola del periodo non può essere null.");
            CallbackAdapter = callbackAdapter ?? throw new ArgumentNullException(nameof(callbackAdapter), "L'adattatore di callback non può essere null.");
            StartTime = startTime;
            IsBackground = isBackground;
        }
    }
}
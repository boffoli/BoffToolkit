using BoffToolkit.Scheduling.Internal.Callbacks;
using BoffToolkit.Scheduling.Internal.States;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Internal {
    /// <summary>
    /// Rappresenta uno scheduler per l'esecuzione di job periodici.
    /// </summary>
    internal class JobScheduler : IJobScheduler, IDisposable {
        private readonly StateContext _context;
        private bool _disposed;

        public string CurrentStateName => _context.StateName;

        /// <summary>
        /// Evento sollevato quando un callback viene completato.
        /// </summary>
        public event EventHandler<CallbackCompletedEventArgs>? OnCallbackCompleted;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="JobScheduler"/>.
        /// </summary>
        /// <param name="startTime">Il tempo di inizio del job.</param>
        /// <param name="periodRule">La regola del periodo per il job.</param>
        /// <param name="callbackAdapter">L'adattatore di callback.</param>
        /// <param name="isBackground">Indica se il job deve essere eseguito in background.</param>
        internal JobScheduler(DateTime startTime, IPeriodRule periodRule, ICallbackAdapter callbackAdapter, bool isBackground) {
            _ = startTime == default ? throw new ArgumentException("Il tempo di inizio deve essere una data valida.", nameof(startTime)) : startTime;
            _ = periodRule ?? throw new ArgumentNullException(nameof(periodRule), "La regola del periodo non può essere null.");
            _ = callbackAdapter ?? throw new ArgumentNullException(nameof(callbackAdapter), "L'adattatore di callback non può essere null.");

            // Crea un contesto per lo scheduler con le configurazioni iniziali
            var jobSchedulerContext = new JobSchedulerContext(startTime, periodRule, callbackAdapter, isBackground);

            // Iscrizione all'evento di completamento del callback
            jobSchedulerContext.CallbackAdapter.CallbackCompleted += (sender, args) => {
                // Solleva l'evento OnCallbackCompleted quando un callback è completato
                OnCallbackCompleted?.Invoke(this, args);
            };

            // Inizializza le operazioni dello scheduler e lo stato iniziale
            var taskManager = new JobSchedulerTaskManager(jobSchedulerContext);
            _context = new StateContext(new StoppedState(taskManager));
        }

        /// <inheritdoc/>
        public void Start() {
            _context.Start();
        }

        /// <inheritdoc/>
        public void Stop() {
            _context.Stop();
        }

        /// <inheritdoc/>
        public void Pause() {
            _context.Pause();
        }

        /// <inheritdoc/>
        public void Resume() {
            _context.Resume();
        }

        /// <inheritdoc/>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Metodo per la gestione del rilascio delle risorse.
        /// </summary>
        /// <param name="disposing">Indica se il metodo è chiamato da Dispose.</param>
        protected virtual void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }

            if (disposing) {
                _context.Stop();
            }

            _disposed = true;
        }

        /// <summary>
        /// Finalizzatore per rilasciare le risorse non gestite.
        /// </summary>
        ~JobScheduler() {
            Dispose(false);
        }
    }
}
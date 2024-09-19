namespace BoffToolkit.Scheduling.Internal {
    /// <summary>
    /// Gestisce il Task di scheduling, inclusi avvio, arresto, pausa e ripresa delle operazioni.
    /// </summary>
    /// <remarks>
    /// Inizializza una nuova istanza della classe <see cref="JobSchedulerTaskManager"/>.
    /// </remarks>
    /// <param name="context">Il contesto del JobScheduler che contiene le configurazioni e lo stato corrente.</param>
    internal class JobSchedulerTaskManager(JobSchedulerContext context) {
        private readonly JobSchedulerContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _isPaused;

        /// <summary>
        /// Avvia l'esecuzione del Task di scheduling.
        /// </summary>
        public void Start() {
            // Se esiste già un CancellationTokenSource, lo eliminiamo
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose(); // Dispose del precedente CancellationTokenSource

            // Creiamo un nuovo CancellationTokenSource
            _cancellationTokenSource = new CancellationTokenSource();
            _isPaused = false;

            // Avvia il task di scheduling
            RunSchedulingTask();
        }

        /// <summary>
        /// Arresta l'esecuzione del Task di scheduling.
        /// </summary>
        public void Stop() {
            _cancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// Mette in pausa l'esecuzione del Task di scheduling.
        /// </summary>
        public void Pause() {
            _isPaused = true;
        }

        /// <summary>
        /// Riprende l'esecuzione del Task di scheduling.
        /// </summary>
        public void Resume() {
            _isPaused = false;
        }

        /// <summary>
        /// Metodo privato che gestisce l'esecuzione del Task di scheduling.
        /// </summary>
        private void RunSchedulingTask() {
            var token = _cancellationTokenSource?.Token ?? CancellationToken.None;

            Task.Factory.StartNew(async () => {
                while (!token.IsCancellationRequested) {
                    if (_isPaused) {
                        // Attende finché non viene ripreso
                        await Task.Delay(100, token);
                        continue;
                    }

                    var now = DateTime.Now;
                    var nextExecution = _context.PeriodRule.GetNextOccurrence(now);
                    var delayToNextExecution = nextExecution - now;

                    if (delayToNextExecution > TimeSpan.Zero) {
                        await Task.Delay(delayToNextExecution, token);
                    }

                    if (!token.IsCancellationRequested) {
                        // Esegue il callback
                        await _context.CallbackAdapter.ExecuteAsync();
                    }
                }
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }
}
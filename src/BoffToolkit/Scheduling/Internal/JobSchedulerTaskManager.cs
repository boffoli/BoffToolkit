using System;
using System.Threading;
using System.Threading.Tasks;
using BoffToolkit.Scheduling.Internal.Callbacks;
using BoffToolkit.Scheduling.Internal.States;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Internal {
    /// <summary>
    /// Gestisce il Task di scheduling, inclusi avvio, arresto, pausa e ripresa delle operazioni.
    /// </summary>
    internal class JobSchedulerTaskManager {
        private readonly JobSchedulerContext _context;
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _schedulingTask;
        private bool _isPaused;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="JobSchedulerTaskManager"/>.
        /// </summary>
        /// <param name="context">Il contesto del JobScheduler che contiene le configurazioni e lo stato corrente.</param>
        public JobSchedulerTaskManager(JobSchedulerContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Avvia l'esecuzione del Task di scheduling.
        /// </summary>
        public void Start() {
            _cancellationTokenSource = new CancellationTokenSource();
            _isPaused = false;
            RunSchedulingTask();
        }

        /// <summary>
        /// Arresta l'esecuzione del Task di scheduling.
        /// </summary>
        public void Stop() {
            _cancellationTokenSource?.Cancel();
            _schedulingTask = null;
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

            _schedulingTask = Task.Factory.StartNew(async () => {
                while (!token.IsCancellationRequested) {
                    if (_isPaused) {
                        // Attende finché non viene ripreso
                        await Task.Delay(100, token);
                        continue;
                    }

                    DateTime now = DateTime.Now;
                    DateTime nextExecution = _context.PeriodRule.GetNextOccurrence(now);
                    TimeSpan delayToNextExecution = nextExecution - now;

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
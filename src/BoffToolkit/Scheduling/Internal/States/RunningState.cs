using BoffToolkit.Logging;

namespace BoffToolkit.Scheduling.Internal.States {
    /// <summary>
    /// Rappresenta lo stato di esecuzione del JobScheduler.
    /// </summary>
    /// <remarks>
    /// Inizializza una nuova istanza della classe <see cref="RunningState"/>.
    /// </remarks>
    /// <param name="taskManager">Il gestore dei task del JobScheduler.</param>
    internal class RunningState(JobSchedulerTaskManager taskManager) : IJobSchedulerState {
        private readonly JobSchedulerTaskManager _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager), TaskManagerNullErrorMessage);

        // Costanti per i messaggi
        private const string TaskManagerNullErrorMessage = "Il gestore dei task non può essere null.";
        private const string ContextNullErrorMessage = "Il contesto non può essere null.";
        private const string TaskAlreadyRunningWarning = "Il task è già in esecuzione.";
        private const string TaskStoppingInfo = "Arresto del task in esecuzione.";
        private const string TaskPausingInfo = "Messa in pausa del task in esecuzione.";

        /// <inheritdoc/>
        public string Name => "Running";

        /// <inheritdoc/>
        public void ApplyState() {
            // Avvia il task quando lo stato diventa Running
            _taskManager.Start();
        }

        /// <inheritdoc/>
        public void Start(StateContext context) {
            HandleAlreadyRunning(context);
        }

        /// <inheritdoc/>
        public void Resume(StateContext context) {
            HandleAlreadyRunning(context);
        }

        /// <inheritdoc/>
        public void Stop(StateContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context), ContextNullErrorMessage);
            }

            CentralLogger<RunningState>.LogInformation(TaskStoppingInfo);
            _taskManager.Stop();
            context.SetState(new StoppedState(_taskManager));
        }

        /// <inheritdoc/>
        public void Pause(StateContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context), ContextNullErrorMessage);
            }

            CentralLogger<RunningState>.LogInformation(TaskPausingInfo);
            _taskManager.Pause();
            context.SetState(new PausedState(_taskManager));
        }

        /// <summary>
        /// Gestisce il caso in cui il task sia già in esecuzione.
        /// </summary>
        /// <param name="context">Il contesto del job scheduler.</param>
        private static void HandleAlreadyRunning(StateContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context), ContextNullErrorMessage);
            }

            CentralLogger<RunningState>.LogWarning(TaskAlreadyRunningWarning);
        }
    }
}
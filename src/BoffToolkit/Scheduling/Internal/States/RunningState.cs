using BoffToolkit.Logging;

namespace BoffToolkit.Scheduling.Internal.States {
    /// <summary>
    /// Rappresenta lo stato di esecuzione del JobScheduler.
    /// </summary>
    internal class RunningState : IJobSchedulerState {
        private readonly JobSchedulerTaskManager _taskManager;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="RunningState"/>.
        /// </summary>
        /// <param name="taskManager">Il gestore dei task del JobScheduler.</param>
        public RunningState(JobSchedulerTaskManager taskManager) {
            _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager), "Il gestore dei task non può essere null.");
        }

        /// <inheritdoc/>
        public void ApplyState() {
            // Avvia il task quando lo stato diventa Running
            _taskManager.Start();
        }

        /// <inheritdoc/>
        public void Start(StateContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context), "Il contesto non può essere null.");
            CentralLogger<RunningState>.LogWarning("Il task è già in esecuzione.");
        }

        /// <inheritdoc/>
        public void Stop(StateContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context), "Il contesto non può essere null.");
            CentralLogger<RunningState>.LogInformation("Arresto del task in esecuzione.");
            _taskManager.Stop();
            context.SetState(new StoppedState(_taskManager));
        }

        /// <inheritdoc/>
        public void Pause(StateContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context), "Il contesto non può essere null.");
            CentralLogger<RunningState>.LogInformation("Messa in pausa del task in esecuzione.");
            _taskManager.Pause();
            context.SetState(new PausedState(_taskManager));
        }

        /// <inheritdoc/>
        public void Resume(StateContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context), "Il contesto non può essere null.");
            CentralLogger<RunningState>.LogWarning("Il task è già in esecuzione.");
        }
    }
}
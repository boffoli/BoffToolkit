using System;
using BoffToolkit.Logging;

namespace BoffToolkit.Scheduling.Internal.States {
    /// <summary>
    /// Rappresenta lo stato di pausa per il task scheduler.
    /// </summary>
    internal class PausedState : IJobSchedulerState {
        private readonly JobSchedulerTaskManager _taskManager;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="PausedState"/> con le operazioni specificate.
        /// </summary>
        /// <param name="taskManager">Il gestore dei task del job scheduler.</param>
        public PausedState(JobSchedulerTaskManager taskManager) {
            _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager), "Il gestore dei task non può essere null.");
        }

        /// <inheritdoc/>
        public void ApplyState() {
            // Pausa il task quando lo stato diventa Paused
            _taskManager.Pause();
        }

        /// <inheritdoc/>
        public void Start(StateContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context), "Il contesto non può essere null.");
            CentralLogger<PausedState>.LogWarning("Il task è già in pausa. Non è possibile avviarlo.");
        }

        /// <inheritdoc/>
        public void Pause(StateContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context), "Il contesto non può essere null.");
            CentralLogger<PausedState>.LogWarning("Il task è già in pausa.");
        }

        /// <inheritdoc/>
        public void Resume(StateContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context), "Il contesto non può essere null.");
            CentralLogger<PausedState>.LogInformation("Ripresa del task dal stato di pausa.");
            _taskManager.Resume();
            context.SetState(new RunningState(_taskManager));
        }

        /// <inheritdoc/>
        public void Stop(StateContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context), "Il contesto non può essere null.");
            CentralLogger<PausedState>.LogInformation("Arresto del task dal stato di pausa.");
            _taskManager.Stop();
            context.SetState(new StoppedState(_taskManager));
        }
    }
}
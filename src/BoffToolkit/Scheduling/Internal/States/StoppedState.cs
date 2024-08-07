using System;
using BoffToolkit.Logging;

namespace BoffToolkit.Scheduling.Internal.States {
    /// <summary>
    /// Rappresenta lo stato di arresto per il task scheduler.
    /// </summary>
    internal class StoppedState : IJobSchedulerState {
        private readonly JobSchedulerTaskManager _taskManager;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="StoppedState"/> con le operazioni specificate.
        /// </summary>
        /// <param name="taskManager">Il gestore dei task del job scheduler.</param>
        public StoppedState(JobSchedulerTaskManager taskManager) {
            _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager), "Il gestore dei task non può essere null.");
        }

        /// <inheritdoc/>
        public void ApplyState() {
            // Ferma il task quando lo stato diventa Stopped
            _taskManager.Stop();
        }

        /// <inheritdoc/>
        public void Start(StateContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context), "Il contesto non può essere null.");
            CentralLogger<StoppedState>.LogInformation("Avvio del task dallo stato di arresto.");
            _taskManager.Start();
            context.SetState(new RunningState(_taskManager));
        }

        /// <inheritdoc/>
        public void Pause(StateContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context), "Il contesto non può essere null.");
            CentralLogger<StoppedState>.LogWarning("Tentativo di pausa durante l'arresto. Operazione non valida.");
        }

        /// <inheritdoc/>
        public void Resume(StateContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context), "Il contesto non può essere null.");
            CentralLogger<StoppedState>.LogWarning("Tentativo di ripresa durante l'arresto. Operazione non valida.");
        }

        /// <inheritdoc/>
        public void Stop(StateContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context), "Il contesto non può essere null.");
            CentralLogger<StoppedState>.LogWarning("Il task è già fermo.");
        }
    }
}
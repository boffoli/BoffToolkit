using System;
using BoffToolkit.Logging;

namespace BoffToolkit.Scheduling.Internal.States {
    /// <summary>
    /// Rappresenta lo stato di arresto per il task scheduler.
    /// </summary>
    /// <remarks>
    /// Inizializza una nuova istanza della classe <see cref="StoppedState"/> con le operazioni specificate.
    /// </remarks>
    /// <param name="taskManager">Il gestore dei task del job scheduler.</param>
    internal class StoppedState(JobSchedulerTaskManager taskManager) : IJobSchedulerState {
        private readonly JobSchedulerTaskManager _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager), TaskManagerNullErrorMessage);

        // Costanti per i messaggi di errore e di log
        private const string TaskManagerNullErrorMessage = "Il gestore dei task non può essere null.";
        private const string ContextNullErrorMessage = "Il contesto non può essere null.";
        private const string StartFromStoppedStateMessage = "Avvio del task dallo stato di arresto.";
        private const string InvalidPauseWarning = "Tentativo di pausa durante l'arresto. Operazione non valida.";
        private const string InvalidResumeWarning = "Tentativo di ripresa durante l'arresto. Operazione non valida.";
        private const string TaskAlreadyStoppedWarning = "Il task è già fermo.";

        /// <inheritdoc/>
        public string Name => "Stopped";

        /// <inheritdoc/>
        public void ApplyState() {
            // Ferma il task quando lo stato diventa Stopped
            _taskManager.Stop();
        }

        /// <inheritdoc/>
        public void Start(StateContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context), ContextNullErrorMessage);
            }

            CentralLogger<StoppedState>.LogInformation(StartFromStoppedStateMessage);
            _taskManager.Start();
            context.SetState(new RunningState(_taskManager));
        }

        /// <inheritdoc/>
        public void Pause(StateContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context), ContextNullErrorMessage);
            }

            CentralLogger<StoppedState>.LogWarning(InvalidPauseWarning);
        }

        /// <inheritdoc/>
        public void Resume(StateContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context), ContextNullErrorMessage);
            }

            CentralLogger<StoppedState>.LogWarning(InvalidResumeWarning);
        }

        /// <inheritdoc/>
        public void Stop(StateContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context), ContextNullErrorMessage);
            }

            CentralLogger<StoppedState>.LogWarning(TaskAlreadyStoppedWarning);
        }
    }
}
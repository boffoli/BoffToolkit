using System;
using BoffToolkit.Logging;

namespace BoffToolkit.Scheduling.Internal.States {
    /// <summary>
    /// Rappresenta lo stato di pausa per il task scheduler.
    /// </summary>
    /// <remarks>
    /// Inizializza una nuova istanza della classe <see cref="PausedState"/> con le operazioni specificate.
    /// </remarks>
    /// <param name="taskManager">Il gestore dei task del job scheduler.</param>
    internal class PausedState(JobSchedulerTaskManager taskManager) : IJobSchedulerState {
        private readonly JobSchedulerTaskManager _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager), TaskManagerNullErrorMessage);

        // Costanti per i messaggi
        private const string TaskManagerNullErrorMessage = "Il gestore dei task non può essere null.";
        private const string ContextNullErrorMessage = "Il contesto non può essere null.";
        private const string TaskAlreadyPausedWarning = "Il task è già in pausa.";
        private const string TaskCannotStartWarning = "Il task è già in pausa. Non è possibile avviarlo.";
        private const string TaskResumingInfo = "Ripresa del task dallo stato di pausa.";
        private const string TaskStoppingInfo = "Arresto del task dallo stato di pausa.";

        /// <inheritdoc/>
        public string Name => "Paused";

        /// <inheritdoc/>
        public void ApplyState() {
            // Pausa il task quando lo stato diventa Paused
            _taskManager.Pause();
        }

        /// <inheritdoc/>
        public void Start(StateContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context), ContextNullErrorMessage);
            }

            CentralLogger<PausedState>.LogWarning(TaskCannotStartWarning);
        }

        /// <inheritdoc/>
        public void Pause(StateContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context), ContextNullErrorMessage);
            }

            CentralLogger<PausedState>.LogWarning(TaskAlreadyPausedWarning);
        }

        /// <inheritdoc/>
        public void Resume(StateContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context), ContextNullErrorMessage);
            }

            CentralLogger<PausedState>.LogInformation(TaskResumingInfo);
            _taskManager.Resume();
            context.SetState(new RunningState(_taskManager));
        }

        /// <inheritdoc/>
        public void Stop(StateContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context), ContextNullErrorMessage);
            }

            CentralLogger<PausedState>.LogInformation(TaskStoppingInfo);
            _taskManager.Stop();
            context.SetState(new StoppedState(_taskManager));
        }
    }
}
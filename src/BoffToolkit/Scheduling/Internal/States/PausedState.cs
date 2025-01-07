using System;
using BoffToolkit.Logging;

namespace BoffToolkit.Scheduling.Internal.States {
    /// <summary>
    /// Represents the paused state for the task scheduler.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PausedState"/> class with the specified operations.
    /// </remarks>
    /// <param name="taskManager">The task manager for the job scheduler.</param>
    internal class PausedState(JobSchedulerTaskManager taskManager) : IJobSchedulerState {
        private readonly JobSchedulerTaskManager _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager), TaskManagerNullErrorMessage);

        // Constants for error and log messages
        private const string TaskManagerNullErrorMessage = "The task manager cannot be null.";
        private const string ContextNullErrorMessage = "The context cannot be null.";
        private const string TaskAlreadyPausedWarning = "The task is already paused.";
        private const string TaskCannotStartWarning = "The task is already paused. It cannot be started.";
        private const string TaskResumingInfo = "Resuming the task from the paused state.";
        private const string TaskStoppingInfo = "Stopping the task from the paused state.";

        /// <inheritdoc/>
        public void ApplyState() {
            // Pauses the task when the state transitions to Paused
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

        /// <inheritdoc/>
        public bool IsStopped() {
            return false;
        }

        /// <inheritdoc/>
        public bool IsPaused() {
            return true;
        }

        /// <inheritdoc/>
        public bool IsRunning() {
            return false;
        }
    }
}
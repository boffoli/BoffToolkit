using System;
using BoffToolkit.Logging;

namespace BoffToolkit.Scheduling.Internal.States {
    /// <summary>
    /// Represents the stopped state of the task scheduler.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="StoppedState"/> class with the specified task manager.
    /// </remarks>
    /// <param name="taskManager">The task manager of the job scheduler.</param>
    internal class StoppedState(JobSchedulerTaskManager taskManager) : IJobSchedulerState {
        private readonly JobSchedulerTaskManager _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager), TaskManagerNullErrorMessage);

        // Constants for error and log messages
        private const string TaskManagerNullErrorMessage = "The task manager cannot be null.";
        private const string ContextNullErrorMessage = "The context cannot be null.";
        private const string StartFromStoppedStateMessage = "Starting task from the stopped state.";
        private const string InvalidPauseWarning = "Attempted to pause while in the stopped state. Operation is not valid.";
        private const string InvalidResumeWarning = "Attempted to resume while in the stopped state. Operation is not valid.";
        private const string TaskAlreadyStoppedWarning = "The task is already stopped.";

        /// <inheritdoc/>
        public void ApplyState() {
            // Stops the task when the state is set to Stopped
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

        /// <inheritdoc/>
        public bool IsStopped() {
            return true;
        }

        /// <inheritdoc/>
        public bool IsPaused() {
            return false;
        }

        /// <inheritdoc/>
        public bool IsRunning() {
            return false;
        }
    }
}
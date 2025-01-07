using BoffToolkit.Logging;

namespace BoffToolkit.Scheduling.Internal.States {
    /// <summary>
    /// Represents the running state of the JobScheduler.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RunningState"/> class.
    /// </remarks>
    /// <param name="taskManager">The task manager for the JobScheduler.</param>
    internal class RunningState(JobSchedulerTaskManager taskManager) : IJobSchedulerState {
        private readonly JobSchedulerTaskManager _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager), TaskManagerNullErrorMessage);

        // Constants for error and log messages
        private const string TaskManagerNullErrorMessage = "The task manager cannot be null.";
        private const string ContextNullErrorMessage = "The context cannot be null.";
        private const string TaskAlreadyRunningWarning = "The task is already running.";
        private const string TaskStoppingInfo = "Stopping the currently running task.";
        private const string TaskPausingInfo = "Pausing the currently running task.";

        /// <inheritdoc/>
        public void ApplyState() {
            // Starts the task when the state transitions to Running
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
        /// Handles the scenario where the task is already running.
        /// </summary>
        /// <param name="context">The context of the job scheduler.</param>
        private static void HandleAlreadyRunning(StateContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context), ContextNullErrorMessage);
            }

            CentralLogger<RunningState>.LogWarning(TaskAlreadyRunningWarning);
        }

        /// <inheritdoc/>
        public bool IsStopped() {
            return false;
        }

        /// <inheritdoc/>
        public bool IsPaused() {
            return false;
        }

        /// <inheritdoc/>
        public bool IsRunning() {
            return true;
        }
    }
}
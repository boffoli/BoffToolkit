using BoffToolkit.Logging;
using BoffToolkit.Scheduling.Internal.Contexts;

namespace BoffToolkit.Scheduling.Internal.TaskManagers {
    /// <summary>
    /// Manages the scheduling task, including starting, stopping, pausing, and resuming operations.
    /// </summary>
    internal class JobSchedulerTaskManager {
        private readonly JobSchedulerContext _context;
        private CancellationTokenSource? _cancellationTokenSource;
        private readonly JobSchedulerTaskManagerStateHelper _stateHelper = new(); // Instanza dell'helper per la gestione dello stato

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSchedulerTaskManager"/> class.
        /// </summary>
        /// <param name="context">The JobScheduler context containing configurations and the current state.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <c>null</c>.</exception>
        internal JobSchedulerTaskManager(JobSchedulerContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Starts the execution of the scheduling task.
        /// </summary>
        public void Start() {
            if (!_stateHelper.TrySetRunning()) {
                // Scheduler già in esecuzione, logga un avviso e ritorna
                CentralLogger<JobSchedulerTaskManager>.LogWarning("Attempted to start the scheduler, but it is already running.");
                return;
            }

            ResetCancellationToken(createNew: true);

            // Avvia il task di scheduling senza attendere
            _ = RunSchedulingTaskAsync();
        }

        /// <summary>
        /// Asynchronously runs the scheduling task and handles its completion.
        /// </summary>
        private async Task RunSchedulingTaskAsync() {
            try {
                await JobSchedulingTaskRunner.RunAsync(
                    _context,
                    _cancellationTokenSource!.Token,
                    _stateHelper.IsPaused
                );
            }
            catch (Exception ex) when (ex is not OperationCanceledException) {
                CentralLogger<JobSchedulerTaskManager>.LogError(ex, "Scheduler encountered an unexpected error.");
            }
            finally {
                HandleTaskCompletion(_cancellationTokenSource!.IsCancellationRequested);
            }
        }

        /// <summary>
        /// Handles the completion of the scheduling task.
        /// </summary>
        /// <param name="wasCanceled">Indicates whether the task was canceled.</param>
        private void HandleTaskCompletion(bool wasCanceled) {
            _stateHelper.SetStopped(); // Imposta lo stato a "arrestato" dopo la terminazione

            if (wasCanceled) {
                CentralLogger<JobSchedulerTaskManager>.LogInformation("Scheduler was canceled.");
            }
            else {
                CentralLogger<JobSchedulerTaskManager>.LogInformation("Scheduler has stopped running.");
            }
        }

        /// <summary>
        /// Stops the execution of the scheduling task.
        /// </summary>
        public void Stop() {
            _ = StopAsync(); // Fire-and-forget the stop operation
        }

        /// <summary>
        /// Pauses the execution of the scheduling task.
        /// </summary>
        public void Pause() {
            _ = PauseAsync(); // Fire-and-forget the pause operation
        }

        /// <summary>
        /// Resumes the execution of the scheduling task.
        /// </summary>
        public void Resume() {
            _ = ResumeAsync(); // Fire-and-forget the resume operation
        }

        /// <summary>
        /// Determines whether the scheduler is stopped.
        /// </summary>
        /// <returns><c>true</c> if the scheduler is stopped; otherwise, <c>false</c>.</returns>
        public bool IsStopped() {
            return !_stateHelper.IsRunning();
        }

        /// <summary>
        /// Determines whether the scheduler is paused.
        /// </summary>
        public bool IsPaused() {
            return _stateHelper.IsPaused();
        }

        /// <summary>
        /// Resets the cancellation token source, canceling and disposing the previous one if necessary.
        /// </summary>
        /// <param name="createNew">Indicates whether to create a new <see cref="CancellationTokenSource"/> after resetting.</param>
        private void ResetCancellationToken(bool createNew) {
            if (_cancellationTokenSource != null) {
                try {
                    _cancellationTokenSource.Cancel();
                }
                finally {
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                }
            }

            if (createNew) {
                _cancellationTokenSource = new CancellationTokenSource();
            }
        }

        /// <summary>
        /// Handles the asynchronous stop operation.
        /// </summary>
        private async Task StopAsync() {
            if (!_stateHelper.IsRunning()) {
                CentralLogger<JobSchedulerTaskManager>.LogWarning("Attempted to stop the scheduler, but it is not running.");
                return;
            }

            ResetCancellationToken(createNew: false);
            _stateHelper.SetStopped();

            await Task.CompletedTask; // Placeholder per ulteriori logiche di stop
        }

        /// <summary>
        /// Handles the asynchronous pause operation.
        /// </summary>
        private async Task PauseAsync() {
            if (!_stateHelper.TryPause()) {
                CentralLogger<JobSchedulerTaskManager>.LogWarning("Attempted to pause the scheduler, but it is not running or already paused.");
                return;
            }

            await Task.Delay(100); // Waits briefly to ensure the pause state is recognized
        }

        /// <summary>
        /// Handles the asynchronous resume operation.
        /// </summary>
        private async Task ResumeAsync() {
            if (!_stateHelper.TryResume()) {
                CentralLogger<JobSchedulerTaskManager>.LogWarning("Attempted to resume the scheduler, but it is not running or not paused.");
                return;
            }

            await Task.CompletedTask; // Placeholder per ulteriori logiche di ripresa
        }
    }
}
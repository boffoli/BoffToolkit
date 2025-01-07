namespace BoffToolkit.Scheduling.Internal {
    /// <summary>
    /// Manages the scheduling task, including starting, stopping, pausing, and resuming operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="JobSchedulerTaskManager"/> class.
    /// </remarks>
    /// <param name="context">The JobScheduler context containing configurations and the current state.</param>
    internal class JobSchedulerTaskManager(JobSchedulerContext context) {
        private readonly JobSchedulerContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _isPaused;

        /// <summary>
        /// Starts the execution of the scheduling task.
        /// </summary>
        public void Start() {
            // If a CancellationTokenSource already exists, cancel and dispose of it
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose(); // Dispose of the previous CancellationTokenSource

            // Create a new CancellationTokenSource
            _cancellationTokenSource = new CancellationTokenSource();
            _isPaused = false;

            // Start the scheduling task
            RunSchedulingTask();
        }

        /// <summary>
        /// Stops the execution of the scheduling task.
        /// </summary>
        public void Stop() {
            _cancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// Pauses the execution of the scheduling task.
        /// </summary>
        public void Pause() {
            _isPaused = true;
        }

        /// <summary>
        /// Resumes the execution of the scheduling task.
        /// </summary>
        public void Resume() {
            _isPaused = false;
        }

        /// <summary>
        /// Private method that manages the execution of the scheduling task.
        /// </summary>
        private void RunSchedulingTask() {
            var token = _cancellationTokenSource?.Token ?? CancellationToken.None;

            Task.Factory.StartNew(async () => {
                while (!token.IsCancellationRequested) {
                    if (_isPaused) {
                        // Wait until resumed
                        await Task.Delay(100, token);
                        continue;
                    }

                    var now = DateTime.Now;
                    var nextExecution = _context.PeriodRule.GetNextOccurrence(now);
                    var delayToNextExecution = nextExecution - now;

                    if (delayToNextExecution > TimeSpan.Zero) {
                        await Task.Delay(delayToNextExecution, token);
                    }

                    if (!token.IsCancellationRequested) {
                        // Execute the callback
                        await _context.CallbackAdapter.ExecuteAsync();
                    }
                }
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }
}
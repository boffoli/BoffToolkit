using BoffToolkit.Scheduling.Internal.Callbacks;
using BoffToolkit.Scheduling.Internal.Contexts;
using BoffToolkit.Scheduling.Internal.States;
using BoffToolkit.Scheduling.Internal.TaskManagers;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Internal {
    /// <summary>
    /// Represents a scheduler for executing periodic jobs with a specific period rule.
    /// </summary>
    internal class JobScheduler<TPeriodRule> : IJobScheduler<TPeriodRule>
    where TPeriodRule : IPeriodRule {

        private readonly StateContext _context;
        private bool _disposed;

        /// <inheritdoc />
        public DateTime StartTime { get; }

        /// <inheritdoc />
        public DateTime? EndTime { get; }

        /// <inheritdoc />
        public TPeriodRule PeriodRule { get; }

        /// <summary>
        /// Occurs when a callback is completed.
        /// </summary>
        public event EventHandler<CallbackCompletedEventArgs>? OnCallbackCompleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobScheduler{TPeriodRule}"/> class.
        /// </summary>
        /// <param name="startTime">The start time of the job.</param>
        /// <param name="periodRule">The specific period rule defining the job's execution intervals.</param>
        /// <param name="callbackAdapter">The adapter responsible for executing callbacks.</param>
        /// <param name="endTime">
        /// The end time of the job. If set, it must be a valid date later than <paramref name="startTime"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="startTime"/> is not a valid date or <paramref name="endTime"/> is earlier than or equal to <paramref name="startTime"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="periodRule"/> or <paramref name="callbackAdapter"/> is <c>null</c>.
        /// </exception>
        internal JobScheduler(
            DateTime startTime,
            TPeriodRule periodRule,
            ICallbackAdapter callbackAdapter,
            DateTime? endTime = null) {

            // Validate and set startTime
            StartTime = startTime;

            // Validate and set endTime
            if (endTime.HasValue && endTime <= startTime) {
                throw new ArgumentException("The end time must be greater than the start time.", nameof(endTime));
            }
            EndTime = endTime;

            // Validate and set periodRule
            PeriodRule = periodRule ?? throw new ArgumentNullException(nameof(periodRule), "The period rule cannot be null.");

            // Create a context for the scheduler with the initial configuration
            var jobSchedulerContext = new JobSchedulerContext(startTime, periodRule, callbackAdapter, EndTime);

            // Subscribe to the callback completion event
            jobSchedulerContext.CallbackAdapter.CallbackCompleted += (sender, args) => {
                OnCallbackCompleted?.Invoke(this, args);
            };

            // Initialize scheduler operations and set the initial state
            var taskManager = new JobSchedulerTaskManager(jobSchedulerContext);
            _context = new StateContext(new StoppedState(taskManager));
        }

        /// <inheritdoc />
        public void Start() => _context.Start();

        /// <inheritdoc />
        public void Stop() => _context.Stop();

        /// <inheritdoc />
        public void Pause() => _context.Pause();

        /// <inheritdoc />
        public void Resume() => _context.Resume();

        /// <inheritdoc />
        public bool IsStopped() => _context.CurrentState.IsStopped();

        /// <inheritdoc />
        public bool IsPaused() => _context.CurrentState.IsPaused();

        /// <inheritdoc />
        public bool IsRunning() => _context.CurrentState.IsRunning();

        /// <inheritdoc />
        public bool IsDisposed() => _disposed;

        /// <inheritdoc />
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases resources used by the <see cref="JobScheduler{TPeriodRule}"/> instance.
        /// </summary>
        /// <param name="disposing">Indicates whether the method is being called from <see cref="Dispose()"/>.</param>
        protected virtual void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }

            if (disposing) {
                _context.Stop();
            }

            _disposed = true;
        }

        /// <summary>
        /// Finalizer to release unmanaged resources.
        /// </summary>
        ~JobScheduler() {
            Dispose(false);
        }
    }
}
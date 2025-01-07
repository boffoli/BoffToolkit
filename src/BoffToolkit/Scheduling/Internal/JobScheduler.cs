using System;
using BoffToolkit.Scheduling.Internal.Callbacks;
using BoffToolkit.Scheduling.Internal.States;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.Internal {
    /// <summary>
    /// Represents a scheduler for executing periodic jobs.
    /// </summary>
    internal class JobScheduler : IJobScheduler {
        private readonly StateContext _context;
        private bool _disposed;

        /// <inheritdoc />
        public DateTime StartTime { get; }

        /// <inheritdoc />
        public IPeriodRule PeriodRule { get; }

        /// <summary>
        /// Occurs when a callback is completed.
        /// </summary>
        public event EventHandler<CallbackCompletedEventArgs>? OnCallbackCompleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobScheduler"/> class.
        /// </summary>
        /// <param name="startTime">The start time of the job.</param>
        /// <param name="periodRule">The period rule defining the job's execution intervals.</param>
        /// <param name="callbackAdapter">The adapter responsible for executing callbacks.</param>
        /// <param name="isBackground">Indicates whether the job should run as a background process.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="startTime"/> is not a valid date.</exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="periodRule"/> or <paramref name="callbackAdapter"/> is <c>null</c>.
        /// </exception>
        internal JobScheduler(DateTime startTime, IPeriodRule periodRule, ICallbackAdapter callbackAdapter, bool isBackground) {
            StartTime = startTime != default
                ? startTime
                : throw new ArgumentException("The start time must be a valid date.", nameof(startTime));

            PeriodRule = periodRule ?? throw new ArgumentNullException(nameof(periodRule), "The period rule cannot be null.");

            // Create a context for the scheduler with the initial configuration
            var jobSchedulerContext = new JobSchedulerContext(startTime, periodRule, callbackAdapter, isBackground);

            // Subscribe to the callback completion event
            jobSchedulerContext.CallbackAdapter.CallbackCompleted += (sender, args) => {
                OnCallbackCompleted?.Invoke(this, args);
            };

            // Initialize scheduler operations and set the initial state
            var taskManager = new JobSchedulerTaskManager(jobSchedulerContext);
            _context = new StateContext(new StoppedState(taskManager));
        }

        /// <inheritdoc />
        public void Start() {
            _context.Start();
        }

        /// <inheritdoc />
        public void Stop() {
            _context.Stop();
        }

        /// <inheritdoc />
        public void Pause() {
            _context.Pause();
        }

        /// <inheritdoc />
        public void Resume() {
            _context.Resume();
        }

        /// <inheritdoc />
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases resources used by the <see cref="JobScheduler"/> instance.
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

        /// <inheritdoc />
        public bool IsStopped() {
            return _context.CurrentState.IsStopped();
        }

        /// <inheritdoc />
        public bool IsPaused() {
            return _context.CurrentState.IsPaused();
        }

        /// <inheritdoc />
        public bool IsRunning() {
            return _context.CurrentState.IsRunning();
        }

        /// <inheritdoc />
        public void Release() {
            Dispose(); // Calls Dispose to release resources.
        }

        /// <inheritdoc />
        public bool IsDisposed() {
            return _disposed;
        }

        /// <summary>
        /// Finalizer to release unmanaged resources.
        /// </summary>
        ~JobScheduler() {
            Dispose(false);
        }
    }
}
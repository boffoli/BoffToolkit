using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling {
    /// <summary>
    /// Provides a non-generic interface for managing the execution of scheduled tasks.
    /// </summary>
    public interface IJobScheduler : IDisposable {
        /// <summary>
        /// Starts the execution of the scheduled tasks.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the execution of the scheduled tasks.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses the execution of the scheduled tasks.
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes the execution of the scheduled tasks.
        /// </summary>
        void Resume();

        /// <summary>
        /// Determines whether the scheduler is in the <c>Stopped</c> state.
        /// </summary>
        /// <returns><c>true</c> if the scheduler is stopped; otherwise, <c>false</c>.</returns>
        bool IsStopped();

        /// <summary>
        /// Determines whether the scheduler is in the <c>Paused</c> state.
        /// </summary>
        /// <returns><c>true</c> if the scheduler is paused; otherwise, <c>false</c>.</returns>
        bool IsPaused();

        /// <summary>
        /// Determines whether the scheduler is in the <c>Running</c> state.
        /// </summary>
        /// <returns><c>true</c> if the scheduler is running; otherwise, <c>false</c>.</returns>
        bool IsRunning();

        /// <summary>
        /// Gets a value indicating whether the scheduler has been disposed.
        /// </summary>
        /// <returns><c>true</c> if the scheduler is disposed; otherwise, <c>false</c>.</returns>
        bool IsDisposed();

        /// <summary>
        /// Gets the start time of the job scheduler.
        /// </summary>
        /// <value>The start time of the scheduler.</value>
        DateTime StartTime { get; }

        /// <summary>
        /// Gets the end time of the job scheduler, if any.
        /// </summary>
        /// <value>The end time of the scheduler, or <c>null</c> if the scheduler does not have an end time.</value>
        DateTime? EndTime { get; }
    }

    /// <summary>
    /// Provides an interface for managing the execution of scheduled tasks with a specific period rule.
    /// </summary>
    public interface IJobScheduler<out TPeriodRule> : IJobScheduler
        where TPeriodRule : IPeriodRule {

        /// <summary>
        /// Gets the specific period rule for this job scheduler.
        /// </summary>
        TPeriodRule PeriodRule { get; }
    }
}
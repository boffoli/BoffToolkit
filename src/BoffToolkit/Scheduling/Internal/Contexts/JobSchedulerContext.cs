using BoffToolkit.Scheduling.PeriodRules;
using BoffToolkit.Scheduling.Internal.Callbacks;

namespace BoffToolkit.Scheduling.Internal.Contexts {
    /// <summary>
    /// Context for the JobScheduler that holds the necessary information for scheduling.
    /// </summary>
    internal class JobSchedulerContext {
        /// <summary>
        /// Start time of the job.
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        /// End time of the job, if any.
        /// </summary>
        public DateTime? EndTime { get; }

        /// <summary>
        /// Period rule for the job.
        /// </summary>
        public IPeriodRule PeriodRule { get; }

        /// <summary>
        /// Callback adapter for executing the scheduled actions.
        /// </summary>
        public ICallbackAdapter CallbackAdapter { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSchedulerContext"/> class.
        /// </summary>
        /// <param name="startTime">The start time of the job.</param>
        /// <param name="periodRule">The period rule for the job.</param>
        /// <param name="callbackAdapter">The callback adapter.</param>
        /// <param name="endTime">The end time of the job, or <c>null</c> for no end time.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="startTime"/> is not a valid date.</exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="periodRule"/> or <paramref name="callbackAdapter"/> is <c>null</c>.
        /// </exception>
        public JobSchedulerContext(DateTime startTime, IPeriodRule periodRule, ICallbackAdapter callbackAdapter, DateTime? endTime = null) {

            if (endTime.HasValue && endTime <= startTime) {
                throw new ArgumentException("The end time must be greater than the start time.", nameof(endTime));
            }

            PeriodRule = periodRule ?? throw new ArgumentNullException(nameof(periodRule), "The period rule cannot be null.");
            CallbackAdapter = callbackAdapter ?? throw new ArgumentNullException(nameof(callbackAdapter), "The callback adapter cannot be null.");
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
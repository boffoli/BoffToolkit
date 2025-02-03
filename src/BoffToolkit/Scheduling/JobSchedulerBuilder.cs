using BoffToolkit.Scheduling.PeriodRules;
using BoffToolkit.Scheduling.Internal;
using BoffToolkit.Scheduling.Internal.Callbacks;
using BoffToolkit.Scheduling.HttpCalls;
using BoffToolkit.Scheduling.BuilderSteps;

namespace BoffToolkit.Scheduling {
    /// <summary>
    /// Builds an instance of <see cref="IJobScheduler{TPeriodRule}"/> with the provided specifications.
    /// </summary>
    public class JobSchedulerBuilder<TPeriodRule>
        where TPeriodRule : IPeriodRule {
        private DateTime _startTime;
        private DateTime? _endTime;

        private TPeriodRule? _periodRule;
        private ICallbackAdapter? _callbackAdapter;
        private EventHandler<CallbackCompletedEventArgs>? _onCallbackCompleted;
        private string? _registryKey;
        private bool _overwriteRegistry;

        /// <summary>
        /// Starts building a <see cref="IJobScheduler{TPeriodRule}"/> by setting the start time.
        /// </summary>
        /// <param name="startTime">The start time for the job scheduler.</param>
        /// <returns>An instance of <see cref="IEndTimeStep{TPeriodRule}"/> to continue the configuration.</returns>
        public static IEndTimeStep<TPeriodRule> SetStartTime(DateTime startTime) {
            return new BuilderSteps(new JobSchedulerBuilder<TPeriodRule>()).SetStart(startTime);
        }

        /// <summary>
        /// Internal class implementing interfaces for building the JobScheduler.
        /// </summary>
        private sealed class BuilderSteps(JobSchedulerBuilder<TPeriodRule> builder) :
            IStartTimeStep<TPeriodRule>,
            IEndTimeStep<TPeriodRule>,
            IPeriodStep<TPeriodRule>,
            ICallbackStep<TPeriodRule>,
            IRegistrationStep<TPeriodRule>,
            IBuildableStep<TPeriodRule> {

            private readonly JobSchedulerBuilder<TPeriodRule> _builder = builder ?? throw new ArgumentNullException(nameof(builder));

            /// <inheritdoc/>
            public IEndTimeStep<TPeriodRule> SetStart(DateTime startTime) {
                _builder._startTime = startTime;
                return this;
            }

            /// <inheritdoc/>
            public IPeriodStep<TPeriodRule> SetEnd(DateTime endTime) {
                if (endTime <= _builder._startTime) {
                    throw new ArgumentException("The end time must be greater than the start time.", nameof(endTime));
                }
                _builder._endTime = endTime;
                return this;
            }

            /// <inheritdoc/>
            public IPeriodStep<TPeriodRule> SetNoEnd() {
                _builder._endTime = null;
                return this;
            }

            /// <inheritdoc/>
            public ICallbackStep<TPeriodRule> SetPeriod(TPeriodRule periodRule) {
                _builder._periodRule = periodRule ?? throw new ArgumentNullException(nameof(periodRule));
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep<TPeriodRule> SetCallback(Action callback) {
                _builder._callbackAdapter = CallbackAdapter<object, object>.Create(callback ?? throw new ArgumentNullException(nameof(callback)));
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep<TPeriodRule> SetCallback<TParam>(Action<TParam> callback, TParam param) {
                _builder._callbackAdapter = CallbackAdapter<object, TParam>.Create(
                    callback ?? throw new ArgumentNullException(nameof(callback)),
                    param ?? throw new ArgumentNullException(nameof(param))
                );
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep<TPeriodRule> SetCallback<TResult>(Func<TResult> func) {
                _builder._callbackAdapter = CallbackAdapter<TResult, object>.Create(func ?? throw new ArgumentNullException(nameof(func)));
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep<TPeriodRule> SetCallback<TParam, TResult>(Func<TParam, TResult> func, TParam param) {
                _builder._callbackAdapter = CallbackAdapter<TResult, TParam>.Create(
                    func ?? throw new ArgumentNullException(nameof(func)),
                    param ?? throw new ArgumentNullException(nameof(param))
                );
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep<TPeriodRule> SetCallback<TResult>(Func<Task<TResult>> func) {
                _builder._callbackAdapter = CallbackAdapter<TResult, object>.Create(func ?? throw new ArgumentNullException(nameof(func)));
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep<TPeriodRule> SetCallback<TParam, TResult>(Func<TParam, Task<TResult>> func, TParam param) {
                _builder._callbackAdapter = CallbackAdapter<TResult, TParam>.Create(
                    func ?? throw new ArgumentNullException(nameof(func)),
                    param ?? throw new ArgumentNullException(nameof(param))
                );
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep<TPeriodRule> SetCallback<TResult>(ISchedulable<TResult> schedulable) {
                _builder._callbackAdapter = CallbackAdapter<TResult, object>.Create(schedulable ?? throw new ArgumentNullException(nameof(schedulable)));
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep<TPeriodRule> SetCallback<TParam, TResult>(ISchedulable<TParam, TResult> schedulable, TParam param) {
                _builder._callbackAdapter = CallbackAdapter<TResult, TParam>.Create(
                    schedulable ?? throw new ArgumentNullException(nameof(schedulable)),
                    param ?? throw new ArgumentNullException(nameof(param))
                );
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep<TPeriodRule> SetCallback<TResult>(IHttpCall<TResult> httpCall) {
                _builder._callbackAdapter = CallbackAdapter<TResult, object>.Create(httpCall ?? throw new ArgumentNullException(nameof(httpCall)));
                return this;
            }

            /// <inheritdoc/>
            public IBuildableStep<TPeriodRule> AddToRegistry(string key, bool overwrite = false) {
                if (string.IsNullOrWhiteSpace(key)) {
                    throw new ArgumentException("Registry key must not be null or empty.", nameof(key));
                }

                _builder._registryKey = key;
                _builder._overwriteRegistry = overwrite;
                return this;
            }

            /// <inheritdoc/>
            public IBuildableStep<TPeriodRule> SkipRegistry() {
                _builder._registryKey = null;
                return this;
            }

            /// <inheritdoc/>
            public IBuildableStep<TPeriodRule> SetCallbackCompleted(EventHandler<CallbackCompletedEventArgs> handler) {
                _builder._onCallbackCompleted = handler ?? throw new ArgumentNullException(nameof(handler));
                return this;
            }

            /// <inheritdoc/>
            public IJobScheduler<TPeriodRule> Build() {
                if (_builder._callbackAdapter == null) {
                    throw new InvalidOperationException("A callback must be set.");
                }

                if (_builder._periodRule == null) {
                    throw new InvalidOperationException("A period rule must be set.");
                }

                var scheduler = new JobScheduler<TPeriodRule>(
                    _builder._startTime,
                    _builder._periodRule,
                    _builder._callbackAdapter,
                    _builder._endTime
                );

                if (_builder._onCallbackCompleted != null) {
                    scheduler.OnCallbackCompleted += _builder._onCallbackCompleted;
                }

                // Register in the global registry if needed
                if (_builder._registryKey != null) {
                    JobSchedulerRegistry.Add(_builder._registryKey, scheduler, _builder._overwriteRegistry);
                }

                return scheduler;
            }
        }
    }

    /// <summary>
    /// Builder for job schedulers using IPeriodRule period rules.
    /// </summary>
    public class JobSchedulerBuilder : JobSchedulerBuilder<IPeriodRule> { }

    /// <summary>
    /// Builder for job schedulers using TimeSpan-based period rules.
    /// </summary>
    public class JobSchedulerBuilderTimeSpan : JobSchedulerBuilder<ITimeSpanPeriodRule> { }

    /// <summary>
    /// Builder for job schedulers using daily period rules.
    /// </summary>
    public class JobSchedulerBuilderDaily : JobSchedulerBuilder<IDailyPeriodRule> { }

    /// <summary>
    /// Builder for job schedulers using weekly period rules.
    /// </summary>
    public class JobSchedulerBuilderWeekly : JobSchedulerBuilder<IWeeklyPeriodRule> { }

    /// <summary>
    /// Builder for job schedulers using monthly period rules.
    /// </summary>
    public class JobSchedulerBuilderMonthly : JobSchedulerBuilder<IMonthlyPeriodRule> { }

    /// <summary>
    /// Builder for job schedulers using annual period rules.
    /// </summary>
    public class JobSchedulerBuilderAnnual : JobSchedulerBuilder<IAnnualPeriodRule> { }
}
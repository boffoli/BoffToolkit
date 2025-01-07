using BoffToolkit.Scheduling.PeriodRules;
using BoffToolkit.Scheduling.Internal;
using BoffToolkit.Scheduling.Internal.Callbacks;
using BoffToolkit.Scheduling.HttpCalls;
using BoffToolkit.Scheduling.BuilderSteps;

namespace BoffToolkit.Scheduling {
    /// <summary>
    /// Builds an instance of <see cref="IJobScheduler"/> with the provided specifications.
    /// </summary>
    public class JobSchedulerBuilder {
        private DateTime _startTime;
        private IPeriodRule? _periodRule;
        private ICallbackAdapter? _callbackAdapter;
        private bool _isBackground;
        private EventHandler<CallbackCompletedEventArgs>? _onCallbackCompleted;
        private bool _register;

        /// <summary>
        /// Starts building a <see cref="IJobScheduler"/> by setting the start time.
        /// </summary>
        /// <param name="startTime">The start time for the job scheduler.</param>
        /// <returns>An instance of <see cref="IPeriodStep"/> to continue the configuration.</returns>
        /// <exception cref="ArgumentException">Thrown if the start time is a default value.</exception>
        public static IPeriodStep SetStartTime(DateTime startTime) {
            return (startTime == default)
                ? throw new ArgumentException("The start time must be a valid date.", nameof(startTime))
                : new BuilderSteps(new JobSchedulerBuilder()).SetStart(startTime);
        }

        /// <summary>
        /// Internal class implementing interfaces for building the JobScheduler.
        /// </summary>
        private sealed class BuilderSteps(JobSchedulerBuilder builder) : IStartTimeStep, IPeriodStep, ICallbackStep, IRegistrationStep, IBackgroundStep, IBuildableStep {
            private readonly JobSchedulerBuilder _builder = builder ?? throw new ArgumentNullException(nameof(builder));

            /// <inheritdoc/>
            public IPeriodStep SetStart(DateTime startTime) {
                _builder._startTime = startTime == default
                    ? throw new ArgumentException("The start time must be a valid date.", nameof(startTime))
                    : startTime;
                return this;
            }

            /// <inheritdoc/>
            public ICallbackStep SetPeriod(IPeriodRule periodRule) {
                _builder._periodRule = periodRule ?? throw new ArgumentNullException(nameof(periodRule));
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep SetCallback(Action callback) {
                _builder._callbackAdapter = CallbackAdapter<object, object>.Create(callback ?? throw new ArgumentNullException(nameof(callback)));
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep SetCallback<TParam>(Action<TParam> callback, TParam param) {
                _builder._callbackAdapter = CallbackAdapter<object, TParam>.Create(
                    callback ?? throw new ArgumentNullException(nameof(callback)),
                    param ?? throw new ArgumentNullException(nameof(param))
                );
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep SetCallback<TResult>(Func<TResult> func) {
                _builder._callbackAdapter = CallbackAdapter<TResult, object>.Create(func ?? throw new ArgumentNullException(nameof(func)));
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep SetCallback<TParam, TResult>(Func<TParam, TResult> func, TParam param) {
                _builder._callbackAdapter = CallbackAdapter<TResult, TParam>.Create(
                    func ?? throw new ArgumentNullException(nameof(func)),
                    param ?? throw new ArgumentNullException(nameof(param))
                );
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep SetCallback<TResult>(Func<Task<TResult>> func) {
                _builder._callbackAdapter = CallbackAdapter<TResult, object>.Create(func ?? throw new ArgumentNullException(nameof(func)));
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep SetCallback<TParam, TResult>(Func<TParam, Task<TResult>> func, TParam param) {
                _builder._callbackAdapter = CallbackAdapter<TResult, TParam>.Create(
                    func ?? throw new ArgumentNullException(nameof(func)),
                    param ?? throw new ArgumentNullException(nameof(param))
                );
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep SetCallback<TResult>(ISchedulable<TResult> schedulable) {
                _builder._callbackAdapter = CallbackAdapter<TResult, object>.Create(schedulable ?? throw new ArgumentNullException(nameof(schedulable)));
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep SetCallback<TParam, TResult>(ISchedulable<TParam, TResult> schedulable, TParam param) {
                _builder._callbackAdapter = CallbackAdapter<TResult, TParam>.Create(
                    schedulable ?? throw new ArgumentNullException(nameof(schedulable)),
                    param ?? throw new ArgumentNullException(nameof(param))
                );
                return this;
            }

            /// <inheritdoc/>
            public IRegistrationStep SetCallback<TResult>(IHttpCall<TResult> httpCall) {
                _builder._callbackAdapter = CallbackAdapter<TResult, object>.Create(httpCall ?? throw new ArgumentNullException(nameof(httpCall)));
                return this;
            }

            /// <inheritdoc/>
            public IBackgroundStep RegisterScheduler(bool register) {
                _builder._register = register;
                return this;
            }

            /// <inheritdoc/>
            public IBuildableStep RunInBackground(bool isBackground) {
                _builder._isBackground = isBackground;
                return this;
            }

            /// <inheritdoc/>
            public IBuildableStep SetCallbackCompleted(EventHandler<CallbackCompletedEventArgs> handler) {
                _builder._onCallbackCompleted = handler ?? throw new ArgumentNullException(nameof(handler));
                return this;
            }

            /// <inheritdoc/>
            public IJobScheduler Build() {
                if (_builder._callbackAdapter == null) {
                    throw new InvalidOperationException("A callback must be set.");
                }

                if (_builder._periodRule == null) {
                    throw new InvalidOperationException("A period rule must be set.");
                }

                var scheduler = new JobScheduler(
                    _builder._startTime,
                    _builder._periodRule,
                    _builder._callbackAdapter,
                    _builder._isBackground
                );

                if (_builder._onCallbackCompleted != null) {
                    scheduler.OnCallbackCompleted += _builder._onCallbackCompleted;
                }

                // Register in the global registry if _register is true
                if (_builder._register) {
                    JobSchedulerRegistry.Add(scheduler.GetType().Name, scheduler);
                }

                return scheduler;
            }
        }
    }
}
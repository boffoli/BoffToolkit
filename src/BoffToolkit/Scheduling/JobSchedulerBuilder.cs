using BoffToolkit.Scheduling.PeriodRules;
using BoffToolkit.Scheduling.Internal;
using BoffToolkit.Scheduling.Internal.Callbacks;
using BoffToolkit.Scheduling.HttpCalls;
using BoffToolkit.Scheduling.BuilderInterfaces;
using System;
using System.Threading.Tasks;

namespace BoffToolkit.Scheduling
{
    /// <summary>
    /// Costruisce un'istanza di <see cref="IJobScheduler"/> con le specifiche fornite.
    /// </summary>
    public class JobSchedulerBuilder
    {
        private DateTime _startTime;
        private IPeriodRule? _periodRule;
        private ICallbackAdapter? _callbackAdapter;
        private bool _isBackground;
        private EventHandler<CallbackCompletedEventArgs>? _onCallbackCompleted;

        /// <summary>
        /// Inizia la costruzione di un <see cref="IJobScheduler"/> impostando l'orario di inizio.
        /// </summary>
        /// <param name="startTime">L'orario di inizio del job scheduler.</param>
        /// <returns>Un'istanza di <see cref="IPeriodSetter"/> per continuare la configurazione.</returns>
        /// <exception cref="ArgumentException">Se l'orario di inizio è una data di default.</exception>
        public static IPeriodSetter SetStartTime(DateTime startTime)
        {
            if (startTime == default)
                throw new ArgumentException("L'orario di inizio deve essere una data valida.", nameof(startTime));

            IStartTimeSetter firstStep = new BuilderSteps(new JobSchedulerBuilder());
            return firstStep.SetStartTime(startTime);
        }

        /// <summary>
        /// Classe interna che implementa le interfacce per costruire il JobScheduler.
        /// </summary>
        private sealed class BuilderSteps : IStartTimeSetter, IPeriodSetter, ICallbackSetter, IBackgroundSetting, IBuildable
        {
            private readonly JobSchedulerBuilder _builder;

            /// <summary>
            /// Inizializza una nuova istanza della classe <see cref="BuilderSteps"/>.
            /// </summary>
            /// <param name="builder">L'istanza di <see cref="JobSchedulerBuilder"/>.</param>
            public BuilderSteps(JobSchedulerBuilder builder)
            {
                _builder = builder ?? throw new ArgumentNullException(nameof(builder), "Il builder non può essere null.");
            }

            /// <inheritdoc/>
            public IPeriodSetter SetStartTime(DateTime startTime)
            {
                if (startTime == default)
                    throw new ArgumentException("L'orario di inizio deve essere una data valida.", nameof(startTime));

                _builder._startTime = startTime;
                return this;
            }

            /// <inheritdoc/>
            public ICallbackSetter SetPeriod(IPeriodRule periodRule)
            {
                _builder._periodRule = periodRule ?? throw new ArgumentNullException(nameof(periodRule), "Il periodo non può essere null.");
                return this;
            }

            /// <inheritdoc/>
            public IBackgroundSetting SetCallback(Action callback)
            {
                if (callback == null)
                    throw new ArgumentNullException(nameof(callback), "Il callback non può essere null.");

                _builder._callbackAdapter = CallbackAdapter<object, object>.Create(callback);
                return this;
            }

            /// <inheritdoc/>
            public IBackgroundSetting SetCallback<TParam>(Action<TParam> callback, TParam param)
            {
                if (callback == null)
                    throw new ArgumentNullException(nameof(callback), "Il callback non può essere null.");
                if (param == null)
                    throw new ArgumentNullException(nameof(param), "Il parametro non può essere null.");

                _builder._callbackAdapter = CallbackAdapter<object, TParam>.Create(callback, param);
                return this;
            }

            /// <inheritdoc/>
            public IBackgroundSetting SetCallback<TResult>(Func<TResult> func)
            {
                if (func == null)
                    throw new ArgumentNullException(nameof(func), "La funzione di callback non può essere null.");

                _builder._callbackAdapter = CallbackAdapter<TResult, object>.Create(func);
                return this;
            }

            /// <inheritdoc/>
            public IBackgroundSetting SetCallback<TParam, TResult>(Func<TParam, TResult> func, TParam param)
            {
                if (func == null)
                    throw new ArgumentNullException(nameof(func), "La funzione di callback non può essere null.");
                if (param == null)
                    throw new ArgumentNullException(nameof(param), "Il parametro non può essere null.");

                _builder._callbackAdapter = CallbackAdapter<TResult, TParam>.Create(func, param);
                return this;
            }

            /// <inheritdoc/>
            public IBackgroundSetting SetCallback<TResult>(Func<Task<TResult>> func)
            {
                if (func == null)
                    throw new ArgumentNullException(nameof(func), "La funzione di callback non può essere null.");

                _builder._callbackAdapter = CallbackAdapter<TResult, object>.Create(func);
                return this;
            }

            /// <inheritdoc/>
            public IBackgroundSetting SetCallback<TParam, TResult>(Func<TParam, Task<TResult>> func, TParam param)
            {
                if (func == null)
                    throw new ArgumentNullException(nameof(func), "La funzione di callback non può essere null.");
                if (param == null)
                    throw new ArgumentNullException(nameof(param), "Il parametro non può essere null.");

                _builder._callbackAdapter = CallbackAdapter<TResult, TParam>.Create(func, param);
                return this;
            }

            /// <inheritdoc/>
            public IBackgroundSetting SetCallback<TResult>(ISchedulable<TResult> schedulable)
            {
                if (schedulable == null)
                    throw new ArgumentNullException(nameof(schedulable), "Il schedulable non può essere null.");

                _builder._callbackAdapter = CallbackAdapter<TResult, object>.Create(schedulable);
                return this;
            }

            /// <inheritdoc/>
            public IBackgroundSetting SetCallback<TParam, TResult>(ISchedulable<TParam, TResult> schedulable, TParam param)
            {
                if (schedulable == null)
                    throw new ArgumentNullException(nameof(schedulable), "Il schedulable non può essere null.");
                if (param == null)
                    throw new ArgumentNullException(nameof(param), "Il parametro non può essere null.");

                _builder._callbackAdapter = CallbackAdapter<TResult, TParam>.Create(schedulable, param);
                return this;
            }

            /// <inheritdoc/>
            public IBackgroundSetting SetCallback<TResult>(IHttpCall<TResult> httpCall)
            {
                if (httpCall == null)
                    throw new ArgumentNullException(nameof(httpCall), "Il HttpCall non può essere null.");

                _builder._callbackAdapter = CallbackAdapter<TResult, object>.Create(httpCall);
                return this;
            }

            /// <inheritdoc/>
            public IBuildable RunInBackground(bool isBackground)
            {
                _builder._isBackground = isBackground;
                return this;
            }

            /// <inheritdoc/>
            public IBuildable SetCallbackCompleted(EventHandler<CallbackCompletedEventArgs> handler)
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler), "Il gestore dell'evento non può essere null.");

                _builder._onCallbackCompleted = handler;
                return this;
            }

            /// <inheritdoc/>
            public IJobScheduler Build()
            {
                if (_builder._callbackAdapter == null)
                {
                    throw new InvalidOperationException("Il callback deve essere impostato.");
                }

                if (_builder._periodRule == null)
                {
                    throw new InvalidOperationException("La regola del periodo deve essere impostata.");
                }

                var scheduler = new JobScheduler(
                    _builder._startTime,
                    _builder._periodRule,
                    _builder._callbackAdapter,
                    _builder._isBackground
                );

                if (_builder._onCallbackCompleted != null)
                {
                    scheduler.OnCallbackCompleted += _builder._onCallbackCompleted;
                }

                return scheduler;
            }
        }
    }
}
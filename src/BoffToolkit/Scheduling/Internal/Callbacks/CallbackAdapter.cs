using System;
using System.Threading.Tasks;
using BoffToolkit.Scheduling.HttpCalls;

namespace BoffToolkit.Scheduling.Internal.Callbacks {
    /// <summary>
    /// Adattatore per la gestione di diversi tipi di callback, inclusa l'esecuzione di chiamate API.
    /// </summary>
    /// <typeparam name="TResult">Il tipo di risultato del callback.</typeparam>
    /// <typeparam name="TParam">Il tipo del parametro del callback.</typeparam>
    public class CallbackAdapter<TResult, TParam> : ICallbackAdapter {
        private readonly Action? _action;
        private readonly Action<TParam>? _actionWithParam;
        private readonly Func<Task<TResult>>? _taskFunc;
        private readonly Func<TParam, Task<TResult>>? _taskFuncWithParam;
        private readonly Func<TResult>? _func;
        private readonly Func<TParam, TResult>? _funcWithParam;
        private readonly ISchedulable<TResult>? _schedulable;
        private readonly ISchedulable<TParam, TResult>? _schedulableWithParam;
        private readonly TParam? _param;

        private CallbackAdapter(Action action) {
            _action = action ?? throw new ArgumentNullException(nameof(action), "L'azione non può essere null.");
        }

        private CallbackAdapter(Action<TParam> actionWithParam, TParam param) {
            _actionWithParam = actionWithParam ?? throw new ArgumentNullException(nameof(actionWithParam), "L'azione con parametro non può essere null.");
            _param = param;
        }

        private CallbackAdapter(Func<Task<TResult>> taskFunc) {
            _taskFunc = taskFunc ?? throw new ArgumentNullException(nameof(taskFunc), "La funzione asincrona non può essere null.");
        }

        private CallbackAdapter(Func<TParam, Task<TResult>> taskFuncWithParam, TParam param) {
            _taskFuncWithParam = taskFuncWithParam ?? throw new ArgumentNullException(nameof(taskFuncWithParam), "La funzione asincrona con parametro non può essere null.");
            _param = param;
        }

        private CallbackAdapter(Func<TResult> func) {
            _func = func ?? throw new ArgumentNullException(nameof(func), "La funzione non può essere null.");
        }

        private CallbackAdapter(Func<TParam, TResult> funcWithParam, TParam param) {
            _funcWithParam = funcWithParam ?? throw new ArgumentNullException(nameof(funcWithParam), "La funzione con parametro non può essere null.");
            _param = param;
        }

        private CallbackAdapter(ISchedulable<TResult> schedulable) {
            _schedulable = schedulable ?? throw new ArgumentNullException(nameof(schedulable), "Il schedulable non può essere null.");
        }

        private CallbackAdapter(ISchedulable<TParam, TResult> schedulableWithParam, TParam param) {
            _schedulableWithParam = schedulableWithParam ?? throw new ArgumentNullException(nameof(schedulableWithParam), "Il schedulable con parametro non può essere null.");
            _param = param;
        }

        private CallbackAdapter(IHttpCall<TResult> httpCall) {
            _schedulable = httpCall ?? throw new ArgumentNullException(nameof(httpCall), "L'oggetto HttpCall non può essere null.");
        }

        /// <summary>
        /// Crea un'istanza di <see cref="CallbackAdapter{TResult, TParam}"/> per un'azione.
        /// </summary>
        /// <param name="action">L'azione da eseguire.</param>
        /// <returns>Una nuova istanza di <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(Action action) => new(action);

        /// <summary>
        /// Crea un'istanza di <see cref="CallbackAdapter{TResult, TParam}"/> per un'azione con parametro.
        /// </summary>
        /// <param name="actionWithParam">L'azione con parametro da eseguire.</param>
        /// <param name="param">Il parametro per l'azione.</param>
        /// <returns>Una nuova istanza di <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(Action<TParam> actionWithParam, TParam param) => new(actionWithParam, param);

        /// <summary>
        /// Crea un'istanza di <see cref="CallbackAdapter{TResult, TParam}"/> per una funzione asincrona.
        /// </summary>
        /// <param name="taskFunc">La funzione asincrona da eseguire.</param>
        /// <returns>Una nuova istanza di <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(Func<Task<TResult>> taskFunc) => new(taskFunc);

        /// <summary>
        /// Crea un'istanza di <see cref="CallbackAdapter{TResult, TParam}"/> per una funzione asincrona con parametro.
        /// </summary>
        /// <param name="taskFuncWithParam">La funzione asincrona con parametro da eseguire.</param>
        /// <param name="param">Il parametro per la funzione.</param>
        /// <returns>Una nuova istanza di <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(Func<TParam, Task<TResult>> taskFuncWithParam, TParam param) => new(taskFuncWithParam, param);

        /// <summary>
        /// Crea un'istanza di <see cref="CallbackAdapter{TResult, TParam}"/> per una funzione.
        /// </summary>
        /// <param name="func">La funzione da eseguire.</param>
        /// <returns>Una nuova istanza di <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(Func<TResult> func) => new(func);

        /// <summary>
        /// Crea un'istanza di <see cref="CallbackAdapter{TResult, TParam}"/> per una funzione con parametro.
        /// </summary>
        /// <param name="funcWithParam">La funzione con parametro da eseguire.</param>
        /// <param name="param">Il parametro per la funzione.</param>
        /// <returns>Una nuova istanza di <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(Func<TParam, TResult> funcWithParam, TParam param) => new(funcWithParam, param);

        /// <summary>
        /// Crea un'istanza di <see cref="CallbackAdapter{TResult, TParam}"/> per un oggetto <see cref="ISchedulable{TResult}"/>.
        /// </summary>
        /// <param name="schedulable">L'oggetto <see cref="ISchedulable{TResult}"/> da eseguire.</param>
        /// <returns>Una nuova istanza di <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(ISchedulable<TResult> schedulable) => new(schedulable);

        /// <summary>
        /// Crea un'istanza di <see cref="CallbackAdapter{TResult, TParam}"/> per un oggetto <see cref="ISchedulable{TParam, TResult}"/>.
        /// </summary>
        /// <param name="schedulableWithParam">L'oggetto <see cref="ISchedulable{TParam, TResult}"/> da eseguire.</param>
        /// <param name="param">Il parametro per l'oggetto schedulable.</param>
        /// <returns>Una nuova istanza di <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(ISchedulable<TParam, TResult> schedulableWithParam, TParam param) => new(schedulableWithParam, param);

        /// <summary>
        /// Crea un'istanza di <see cref="CallbackAdapter{TResult, TParam}"/> per una chiamata HTTP.
        /// </summary>
        /// <param name="httpCall">L'oggetto <see cref="IHttpCall{TResult}"/> che rappresenta la chiamata HTTP da eseguire.</param>
        /// <returns>Una nuova istanza di <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(IHttpCall<TResult> httpCall) => new(httpCall);

        /// <inheritdoc />
        public event EventHandler<CallbackCompletedEventArgs>? CallbackCompleted;

        /// <inheritdoc />
        public async Task ExecuteAsync() {
            object? result = null;

            if (_action != null) {
                _action();
            }
            else if (_actionWithParam != null) {
                _actionWithParam(_param!);
            }
            else if (_taskFunc != null) {
                result = await _taskFunc();
            }
            else if (_taskFuncWithParam != null) {
                result = await _taskFuncWithParam(_param!);
            }
            else if (_func != null) {
                result = _func();
            }
            else if (_funcWithParam != null) {
                result = _funcWithParam(_param!);
            }
            else if (_schedulable != null) {
                result = _schedulable.Execute();
            }
            else if (_schedulableWithParam != null) {
                result = _schedulableWithParam.Execute(_param!);
            }
            else {
                throw new InvalidOperationException("Nessun callback valido trovato.");
            }
            // Solleva l'evento di completamento
            CallbackCompleted?.Invoke(this, new CallbackCompletedEventArgs(result));
        }
    }
}
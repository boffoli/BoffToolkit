using System;
using System.Threading.Tasks;
using BoffToolkit.Scheduling.HttpCalls;

namespace BoffToolkit.Scheduling.Internal.Callbacks {
    /// <summary>
    /// Adapter for managing various types of callbacks, including executing API calls.
    /// </summary>
    /// <typeparam name="TResult">The type of the callback result.</typeparam>
    /// <typeparam name="TParam">The type of the callback parameter.</typeparam>
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
            _action = action ?? throw new ArgumentNullException(nameof(action), "The action cannot be null.");
        }

        private CallbackAdapter(Action<TParam> actionWithParam, TParam param) {
            _actionWithParam = actionWithParam ?? throw new ArgumentNullException(nameof(actionWithParam), "The action with parameter cannot be null.");
            _param = param;
        }

        private CallbackAdapter(Func<Task<TResult>> taskFunc) {
            _taskFunc = taskFunc ?? throw new ArgumentNullException(nameof(taskFunc), "The asynchronous function cannot be null.");
        }

        private CallbackAdapter(Func<TParam, Task<TResult>> taskFuncWithParam, TParam param) {
            _taskFuncWithParam = taskFuncWithParam ?? throw new ArgumentNullException(nameof(taskFuncWithParam), "The asynchronous function with parameter cannot be null.");
            _param = param;
        }

        private CallbackAdapter(Func<TResult> func) {
            _func = func ?? throw new ArgumentNullException(nameof(func), "The function cannot be null.");
        }

        private CallbackAdapter(Func<TParam, TResult> funcWithParam, TParam param) {
            _funcWithParam = funcWithParam ?? throw new ArgumentNullException(nameof(funcWithParam), "The function with parameter cannot be null.");
            _param = param;
        }

        private CallbackAdapter(ISchedulable<TResult> schedulable) {
            _schedulable = schedulable ?? throw new ArgumentNullException(nameof(schedulable), "The schedulable cannot be null.");
        }

        private CallbackAdapter(ISchedulable<TParam, TResult> schedulableWithParam, TParam param) {
            _schedulableWithParam = schedulableWithParam ?? throw new ArgumentNullException(nameof(schedulableWithParam), "The schedulable with parameter cannot be null.");
            _param = param;
        }

        private CallbackAdapter(IHttpCall<TResult> httpCall) {
            _schedulable = httpCall ?? throw new ArgumentNullException(nameof(httpCall), "The HTTP call object cannot be null.");
        }

        /// <summary>
        /// Creates an instance of <see cref="CallbackAdapter{TResult, TParam}"/> for an action.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        /// <returns>A new instance of <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(Action action) => new(action);

        /// <summary>
        /// Creates an instance of <see cref="CallbackAdapter{TResult, TParam}"/> for an action with a parameter.
        /// </summary>
        /// <param name="actionWithParam">The action with a parameter to be executed.</param>
        /// <param name="param">The parameter for the action.</param>
        /// <returns>A new instance of <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(Action<TParam> actionWithParam, TParam param) => new(actionWithParam, param);

        /// <summary>
        /// Creates an instance of <see cref="CallbackAdapter{TResult, TParam}"/> for an asynchronous function.
        /// </summary>
        /// <param name="taskFunc">The asynchronous function to be executed.</param>
        /// <returns>A new instance of <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(Func<Task<TResult>> taskFunc) => new(taskFunc);

        /// <summary>
        /// Creates an instance of <see cref="CallbackAdapter{TResult, TParam}"/> for an asynchronous function with a parameter.
        /// </summary>
        /// <param name="taskFuncWithParam">The asynchronous function with a parameter to be executed.</param>
        /// <param name="param">The parameter for the function.</param>
        /// <returns>A new instance of <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(Func<TParam, Task<TResult>> taskFuncWithParam, TParam param) => new(taskFuncWithParam, param);

        /// <summary>
        /// Creates an instance of <see cref="CallbackAdapter{TResult, TParam}"/> for a function.
        /// </summary>
        /// <param name="func">The function to be executed.</param>
        /// <returns>A new instance of <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(Func<TResult> func) => new(func);

        /// <summary>
        /// Creates an instance of <see cref="CallbackAdapter{TResult, TParam}"/> for a function with a parameter.
        /// </summary>
        /// <param name="funcWithParam">The function with a parameter to be executed.</param>
        /// <param name="param">The parameter for the function.</param>
        /// <returns>A new instance of <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(Func<TParam, TResult> funcWithParam, TParam param) => new(funcWithParam, param);

        /// <summary>
        /// Creates an instance of <see cref="CallbackAdapter{TResult, TParam}"/> for a <see cref="ISchedulable{TResult}"/>.
        /// </summary>
        /// <param name="schedulable">The <see cref="ISchedulable{TResult}"/> object to be executed.</param>
        /// <returns>A new instance of <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(ISchedulable<TResult> schedulable) => new(schedulable);

        /// <summary>
        /// Creates an instance of <see cref="CallbackAdapter{TResult, TParam}"/> for a <see cref="ISchedulable{TParam, TResult}"/>.
        /// </summary>
        /// <param name="schedulableWithParam">The <see cref="ISchedulable{TParam, TResult}"/> object to be executed.</param>
        /// <param name="param">The parameter for the schedulable object.</param>
        /// <returns>A new instance of <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
        public static CallbackAdapter<TResult, TParam> Create(ISchedulable<TParam, TResult> schedulableWithParam, TParam param) => new(schedulableWithParam, param);

        /// <summary>
        /// Creates an instance of <see cref="CallbackAdapter{TResult, TParam}"/> for an HTTP call.
        /// </summary>
        /// <param name="httpCall">The <see cref="IHttpCall{TResult}"/> representing the HTTP call to be executed.</param>
        /// <returns>A new instance of <see cref="CallbackAdapter{TResult, TParam}"/>.</returns>
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
                throw new InvalidOperationException("No valid callback found.");
            }
            // Raise the callback completed event
            CallbackCompleted?.Invoke(this, new CallbackCompletedEventArgs(result));
        }
    }
}
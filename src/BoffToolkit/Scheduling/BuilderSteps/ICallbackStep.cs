using BoffToolkit.Scheduling.HttpCalls;

namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interface for configuring the callback to be executed.
    /// </summary>
    public interface ICallbackStep {
        /// <summary>
        /// Sets the callback to be executed.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        /// <returns>An instance of <see cref="IRegistrationStep"/>.</returns>
        IRegistrationStep SetCallback(Action callback);

        /// <summary>
        /// Sets the callback to be executed with a parameter.
        /// </summary>
        /// <typeparam name="TParam">The type of the callback parameter.</typeparam>
        /// <param name="callback">The callback action.</param>
        /// <param name="param">The callback parameter.</param>
        /// <returns>An instance of <see cref="IRegistrationStep"/>.</returns>
        IRegistrationStep SetCallback<TParam>(Action<TParam> callback, TParam param);

        /// <summary>
        /// Sets the callback to be executed with a result.
        /// </summary>
        /// <typeparam name="TResult">The type of the callback result.</typeparam>
        /// <param name="func">The callback function.</param>
        /// <returns>An instance of <see cref="IRegistrationStep"/>.</returns>
        IRegistrationStep SetCallback<TResult>(Func<TResult> func);

        /// <summary>
        /// Sets the callback to be executed with a parameter and a result.
        /// </summary>
        /// <typeparam name="TParam">The type of the callback parameter.</typeparam>
        /// <typeparam name="TResult">The type of the callback result.</typeparam>
        /// <param name="func">The callback function.</param>
        /// <param name="param">The callback parameter.</param>
        /// <returns>An instance of <see cref="IRegistrationStep"/>.</returns>
        IRegistrationStep SetCallback<TParam, TResult>(Func<TParam, TResult> func, TParam param);

        /// <summary>
        /// Sets an asynchronous callback to be executed.
        /// </summary>
        /// <typeparam name="TResult">The type of the callback result.</typeparam>
        /// <param name="func">The asynchronous callback function.</param>
        /// <returns>An instance of <see cref="IRegistrationStep"/>.</returns>
        IRegistrationStep SetCallback<TResult>(Func<Task<TResult>> func);

        /// <summary>
        /// Sets an asynchronous callback to be executed with a parameter and a result.
        /// </summary>
        /// <typeparam name="TParam">The type of the callback parameter.</typeparam>
        /// <typeparam name="TResult">The type of the callback result.</typeparam>
        /// <param name="func">The asynchronous callback function.</param>
        /// <param name="param">The callback parameter.</param>
        /// <returns>An instance of <see cref="IRegistrationStep"/>.</returns>
        IRegistrationStep SetCallback<TParam, TResult>(Func<TParam, Task<TResult>> func, TParam param);

        /// <summary>
        /// Sets an instance of <see cref="ISchedulable{TResult}"/> as the callback to be executed.
        /// </summary>
        /// <typeparam name="TResult">The type of the callback result.</typeparam>
        /// <param name="schedulable">The instance of <see cref="ISchedulable{TResult}"/>.</param>
        /// <returns>An instance of <see cref="IRegistrationStep"/>.</returns>
        IRegistrationStep SetCallback<TResult>(ISchedulable<TResult> schedulable);

        /// <summary>
        /// Sets an instance of <see cref="ISchedulable{TParam, TResult}"/> as the callback to be executed with a parameter.
        /// </summary>
        /// <typeparam name="TParam">The type of the callback parameter.</typeparam>
        /// <typeparam name="TResult">The type of the callback result.</typeparam>
        /// <param name="schedulable">The instance of <see cref="ISchedulable{TParam, TResult}"/>.</param>
        /// <param name="param">The callback parameter.</param>
        /// <returns>An instance of <see cref="IRegistrationStep"/>.</returns>
        IRegistrationStep SetCallback<TParam, TResult>(ISchedulable<TParam, TResult> schedulable, TParam param);

        /// <summary>
        /// Sets an instance of <see cref="IHttpCall{TResult}"/> as the callback to be executed.
        /// </summary>
        /// <typeparam name="TResult">The type of the callback result.</typeparam>
        /// <param name="httpCall">The instance of <see cref="IHttpCall{TResult}"/>.</param>
        /// <returns>An instance of <see cref="IRegistrationStep"/>.</returns>
        IRegistrationStep SetCallback<TResult>(IHttpCall<TResult> httpCall);
    }
}
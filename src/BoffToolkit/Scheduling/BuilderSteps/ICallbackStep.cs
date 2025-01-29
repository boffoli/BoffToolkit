using BoffToolkit.Scheduling.HttpCalls;
using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interface for configuring the callback to be executed in the job scheduler.
    /// </summary>
    /// <typeparam name="TPeriodRule">The specific period rule type associated with the job scheduler.</typeparam>
    public interface ICallbackStep<TPeriodRule> where TPeriodRule : IPeriodRule {
        /// <summary>
        /// Sets the callback action to be executed.
        /// </summary>
        /// <param name="callback">The callback action to execute.</param>
        /// <returns>An instance of <see cref="IRegistrationStep{TPeriodRule}"/> for further configuration.</returns>
        IRegistrationStep<TPeriodRule> SetCallback(Action callback);

        /// <summary>
        /// Sets a callback action that requires a parameter.
        /// </summary>
        /// <typeparam name="TParam">The type of the callback parameter.</typeparam>
        /// <param name="callback">The callback action to execute.</param>
        /// <param name="param">The parameter to pass to the callback.</param>
        /// <returns>An instance of <see cref="IRegistrationStep{TPeriodRule}"/> for further configuration.</returns>
        IRegistrationStep<TPeriodRule> SetCallback<TParam>(Action<TParam> callback, TParam param);

        /// <summary>
        /// Sets a callback function that returns a result.
        /// </summary>
        /// <typeparam name="TResult">The return type of the callback function.</typeparam>
        /// <param name="func">The function to execute.</param>
        /// <returns>An instance of <see cref="IRegistrationStep{TPeriodRule}"/> for further configuration.</returns>
        IRegistrationStep<TPeriodRule> SetCallback<TResult>(Func<TResult> func);

        /// <summary>
        /// Sets a callback function that requires a parameter and returns a result.
        /// </summary>
        /// <typeparam name="TParam">The type of the callback parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the callback function.</typeparam>
        /// <param name="func">The function to execute.</param>
        /// <param name="param">The parameter to pass to the callback function.</param>
        /// <returns>An instance of <see cref="IRegistrationStep{TPeriodRule}"/> for further configuration.</returns>
        IRegistrationStep<TPeriodRule> SetCallback<TParam, TResult>(Func<TParam, TResult> func, TParam param);

        /// <summary>
        /// Sets an asynchronous callback function that returns a result.
        /// </summary>
        /// <typeparam name="TResult">The return type of the asynchronous function.</typeparam>
        /// <param name="func">The asynchronous function to execute.</param>
        /// <returns>An instance of <see cref="IRegistrationStep{TPeriodRule}"/> for further configuration.</returns>
        IRegistrationStep<TPeriodRule> SetCallback<TResult>(Func<Task<TResult>> func);

        /// <summary>
        /// Sets an asynchronous callback function that requires a parameter and returns a result.
        /// </summary>
        /// <typeparam name="TParam">The type of the callback parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the asynchronous function.</typeparam>
        /// <param name="func">The asynchronous function to execute.</param>
        /// <param name="param">The parameter to pass to the function.</param>
        /// <returns>An instance of <see cref="IRegistrationStep{TPeriodRule}"/> for further configuration.</returns>
        IRegistrationStep<TPeriodRule> SetCallback<TParam, TResult>(Func<TParam, Task<TResult>> func, TParam param);

        /// <summary>
        /// Sets a schedulable instance as the callback to be executed.
        /// </summary>
        /// <typeparam name="TResult">The return type of the schedulable instance.</typeparam>
        /// <param name="schedulable">The instance of <see cref="ISchedulable{TResult}"/> to execute.</param>
        /// <returns>An instance of <see cref="IRegistrationStep{TPeriodRule}"/> for further configuration.</returns>
        IRegistrationStep<TPeriodRule> SetCallback<TResult>(ISchedulable<TResult> schedulable);

        /// <summary>
        /// Sets a schedulable instance as the callback to be executed with a parameter.
        /// </summary>
        /// <typeparam name="TParam">The type of the callback parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the schedulable instance.</typeparam>
        /// <param name="schedulable">The instance of <see cref="ISchedulable{TParam, TResult}"/> to execute.</param>
        /// <param name="param">The parameter to pass to the schedulable instance.</param>
        /// <returns>An instance of <see cref="IRegistrationStep{TPeriodRule}"/> for further configuration.</returns>
        IRegistrationStep<TPeriodRule> SetCallback<TParam, TResult>(ISchedulable<TParam, TResult> schedulable, TParam param);

        /// <summary>
        /// Sets an HTTP call as the callback to be executed.
        /// </summary>
        /// <typeparam name="TResult">The return type of the HTTP call.</typeparam>
        /// <param name="httpCall">The instance of <see cref="IHttpCall{TResult}"/> to execute.</param>
        /// <returns>An instance of <see cref="IRegistrationStep{TPeriodRule}"/> for further configuration.</returns>
        IRegistrationStep<TPeriodRule> SetCallback<TResult>(IHttpCall<TResult> httpCall);
    }
}
using System;
using System.Threading.Tasks;

namespace BoffToolkit.Scheduling.BuilderInterfaces {
    /// <summary>
    /// Interfaccia per impostare il callback da eseguire.
    /// </summary>
    public interface ICallbackSetter {
        /// <summary>
        /// Imposta il callback da eseguire.
        /// </summary>
        /// <param name="callback">L'azione del callback.</param>
        /// <returns>Un'istanza di <see cref="IBackgroundSetting"/>.</returns>
        IBackgroundSetting SetCallback(Action callback);

        /// <summary>
        /// Imposta il callback da eseguire con un parametro.
        /// </summary>
        /// <typeparam name="TParam">Il tipo del parametro del callback.</typeparam>
        /// <param name="callback">L'azione del callback.</param>
        /// <param name="param">Il parametro del callback.</param>
        /// <returns>Un'istanza di <see cref="IBackgroundSetting"/>.</returns>
        IBackgroundSetting SetCallback<TParam>(Action<TParam> callback, TParam param);

        /// <summary>
        /// Imposta il callback da eseguire con un risultato.
        /// </summary>
        /// <typeparam name="TResult">Il tipo del risultato del callback.</typeparam>
        /// <param name="func">La funzione del callback.</param>
        /// <returns>Un'istanza di <see cref="IBackgroundSetting"/>.</returns>
        IBackgroundSetting SetCallback<TResult>(Func<TResult> func);

        /// <summary>
        /// Imposta il callback da eseguire con un parametro e un risultato.
        /// </summary>
        /// <typeparam name="TParam">Il tipo del parametro del callback.</typeparam>
        /// <typeparam name="TResult">Il tipo del risultato del callback.</typeparam>
        /// <param name="func">La funzione del callback.</param>
        /// <param name="param">Il parametro del callback.</param>
        /// <returns>Un'istanza di <see cref="IBackgroundSetting"/>.</returns>
        IBackgroundSetting SetCallback<TParam, TResult>(Func<TParam, TResult> func, TParam param);

        /// <summary>
        /// Imposta il callback asincrono da eseguire.
        /// </summary>
        /// <typeparam name="TResult">Il tipo del risultato del callback.</typeparam>
        /// <param name="func">La funzione del callback.</param>
        /// <returns>Un'istanza di <see cref="IBackgroundSetting"/>.</returns>
        IBackgroundSetting SetCallback<TResult>(Func<Task<TResult>> func);

        /// <summary>
        /// Imposta il callback asincrono da eseguire con un parametro e un risultato.
        /// </summary>
        /// <typeparam name="TParam">Il tipo del parametro del callback.</typeparam>
        /// <typeparam name="TResult">Il tipo del risultato del callback.</typeparam>
        /// <param name="func">La funzione del callback.</param>
        /// <param name="param">Il parametro del callback.</param>
        /// <returns>Un'istanza di <see cref="IBackgroundSetting"/>.</returns>
        IBackgroundSetting SetCallback<TParam, TResult>(Func<TParam, Task<TResult>> func, TParam param);

        /// <summary>
        /// Imposta un'istanza di <see cref="ISchedulable{TResult}"/> come callback da eseguire.
        /// </summary>
        /// <typeparam name="TResult">Il tipo del risultato del callback.</typeparam>
        /// <param name="schedulable">L'istanza di <see cref="ISchedulable{TResult}"/>.</param>
        /// <returns>Un'istanza di <see cref="IBackgroundSetting"/>.</returns>
        IBackgroundSetting SetCallback<TResult>(ISchedulable<TResult> schedulable);

        /// <summary>
        /// Imposta un'istanza di <see cref="ISchedulable{TParam, TResult}"/> come callback da eseguire con un parametro.
        /// </summary>
        /// <typeparam name="TParam">Il tipo del parametro del callback.</typeparam>
        /// <typeparam name="TResult">Il tipo del risultato del callback.</typeparam>
        /// <param name="schedulable">L'istanza di <see cref="ISchedulable{TParam, TResult}"/>.</param>
        /// <param name="param">Il parametro del callback.</param>
        /// <returns>Un'istanza di <see cref="IBackgroundSetting"/>.</returns>
        IBackgroundSetting SetCallback<TParam, TResult>(ISchedulable<TParam, TResult> schedulable, TParam param);
    }
}
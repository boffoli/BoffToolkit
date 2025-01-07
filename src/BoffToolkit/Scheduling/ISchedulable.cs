using System;

namespace BoffToolkit.Scheduling {
    /// <summary>
    /// Defines an interface for a schedulable object that can perform a specific action and return a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result produced by the scheduled operation.</typeparam>
    public interface ISchedulable<out TResult> {
        /// <summary>
        /// Executes the defined action and returns the result.
        /// </summary>
        /// <returns>The result of the operation.</returns>
        TResult Execute();
    }

    /// <summary>
    /// Defines an interface for a schedulable object that can perform a specific action with a parameter and return a result.
    /// </summary>
    /// <typeparam name="TParam">The type of the parameter passed to the scheduled operation.</typeparam>
    /// <typeparam name="TResult">The type of the result produced by the scheduled operation.</typeparam>
    public interface ISchedulable<in TParam, out TResult> {
        /// <summary>
        /// Executes the defined action using the provided parameter and returns the result.
        /// </summary>
        /// <param name="param">The parameter passed to the scheduled operation.</param>
        /// <returns>The result of the operation.</returns>
        TResult Execute(TParam param);
    }
}
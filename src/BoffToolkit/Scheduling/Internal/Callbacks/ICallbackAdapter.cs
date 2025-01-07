using System;
using System.Threading.Tasks;

namespace BoffToolkit.Scheduling.Internal.Callbacks {
    /// <summary>
    /// Represents the event arguments for a callback completion event.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CallbackCompletedEventArgs"/> class.
    /// </remarks>
    /// <param name="result">The result of the callback.</param>
    public class CallbackCompletedEventArgs(object? result) : EventArgs {
        /// <summary>
        /// Gets the result of the callback, if available.
        /// </summary>
        public object? Result { get; } = result;
    }

    /// <summary>
    /// Interface for a callback adapter that manages the execution of various types of callbacks.
    /// </summary>
    internal interface ICallbackAdapter {
        /// <summary>
        /// Event raised when the callback execution is completed.
        /// </summary>
        event EventHandler<CallbackCompletedEventArgs>? CallbackCompleted;

        /// <summary>
        /// Executes the callback asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ExecuteAsync();
    }
}
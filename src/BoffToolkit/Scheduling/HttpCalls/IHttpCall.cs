namespace BoffToolkit.Scheduling.HttpCalls {
    /// <summary>
    /// Interface representing a schedulable HTTP call.
    /// </summary>
    /// <typeparam name="TResult">The type of the result produced by the HTTP call.</typeparam>
    public interface IHttpCall<out TResult> : ISchedulable<TResult> { }
}
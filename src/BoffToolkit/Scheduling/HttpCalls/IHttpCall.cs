using System.Net.Http;

namespace BoffToolkit.Scheduling.HttpCalls {
    /// <summary>
    /// Interfaccia che rappresenta una chiamata HTTP schedulabile.
    /// </summary>
    /// <typeparam name="TResult">Il tipo del risultato prodotto dalla chiamata HTTP.</typeparam>
    public interface IHttpCall<TResult> : ISchedulable<TResult> { }
}
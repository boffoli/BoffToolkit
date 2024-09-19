using System.Net.Http;

namespace BoffToolkit.Scheduling.Internal.HttpCalls {
    /// <summary>
    /// Rappresenta una chiamata HTTP GET schedulabile.
    /// </summary>
    /// <typeparam name="TResult">Il tipo del risultato atteso dalla chiamata GET.</typeparam>
    /// <remarks>
    /// Inizializza una nuova istanza della classe <see cref="HttpGetCall{TResult}"/>.
    /// </remarks>
    /// <param name="url">L'URL dell'endpoint API da chiamare.</param>
    internal sealed class HttpGetCall<TResult>(string url) : HttpCallBase<TResult>(url) {

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendRequestAsync() {
            return HttpClient.GetAsync(Url);
        }
    }
}
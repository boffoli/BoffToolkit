namespace BoffToolkit.Scheduling.Internal.HttpCalls
{
    /// <summary>
    /// Rappresenta una chiamata HTTP DELETE schedulabile.
    /// </summary>
    /// <typeparam name="TResult">Il tipo del risultato atteso dalla chiamata DELETE.</typeparam>
    internal sealed class HttpDeleteCall<TResult> : HttpCallBase<TResult>
    {
        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="HttpDeleteCall{TResult}"/>.
        /// </summary>
        /// <param name="url">L'URL dell'endpoint API da chiamare.</param>
        public HttpDeleteCall(string url) : base(url) { }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendRequestAsync()
        {
            return HttpClient.DeleteAsync(Url);
        }
    }
}
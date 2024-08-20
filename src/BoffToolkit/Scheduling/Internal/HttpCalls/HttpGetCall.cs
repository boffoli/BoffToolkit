namespace BoffToolkit.Scheduling.Internal.HttpCalls
{
    /// <summary>
    /// Rappresenta una chiamata HTTP GET schedulabile.
    /// </summary>
    /// <typeparam name="TResult">Il tipo del risultato atteso dalla chiamata GET.</typeparam>
    internal sealed class HttpGetCall<TResult> : HttpCallBase<TResult>
    {
        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="HttpGetCall{TResult}"/>.
        /// </summary>
        /// <param name="url">L'URL dell'endpoint API da chiamare.</param>
        public HttpGetCall(string url) : base(url) { }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendRequestAsync()
        {
            return HttpClient.GetAsync(Url);
        }
    }
}
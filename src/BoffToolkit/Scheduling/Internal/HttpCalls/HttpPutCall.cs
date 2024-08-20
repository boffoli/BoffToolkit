using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BoffToolkit.Scheduling.Internal.HttpCalls
{
    /// <summary>
    /// Rappresenta una chiamata HTTP PUT schedulabile.
    /// </summary>
    /// <typeparam name="TResult">Il tipo del risultato atteso dalla chiamata PUT.</typeparam>
    /// <typeparam name="TParam">Il tipo dei dati inviati nel corpo della richiesta PUT.</typeparam>
    internal sealed class HttpPutCall<TParam, TResult> : HttpCallBase<TResult>
    {
        private readonly HttpContent _content;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="HttpPutCall{TParam, TResult}"/>.
        /// </summary>
        /// <param name="url">L'URL dell'endpoint API da chiamare.</param>
        /// <param name="data">I dati da inviare nel corpo della richiesta PUT.</param>
        /// <exception cref="ArgumentNullException">Sollevata se l'URL o i dati sono null.</exception>
        public HttpPutCall(string url, TParam data) : base(url)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "I dati non possono essere null.");

            var jsonContent = JsonConvert.SerializeObject(data);
            _content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendRequestAsync()
        {
            if (_content == null)
                throw new InvalidOperationException("Il contenuto della richiesta PUT non può essere null.");

            return HttpClient.PutAsync(Url, _content);
        }
    }
}
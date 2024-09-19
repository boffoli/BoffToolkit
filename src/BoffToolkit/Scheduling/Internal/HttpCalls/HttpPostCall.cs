using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BoffToolkit.Scheduling.Internal.HttpCalls {
    /// <summary>
    /// Rappresenta una chiamata HTTP POST schedulabile.
    /// </summary>
    /// <typeparam name="TResult">Il tipo del risultato atteso dalla chiamata POST.</typeparam>
    /// <typeparam name="TParam">Il tipo dei dati inviati nel corpo della richiesta POST.</typeparam>
    internal sealed class HttpPostCall<TParam, TResult> : HttpCallBase<TResult> {
        private readonly HttpContent _content;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="HttpPostCall{TParam, TResult}"/>.
        /// </summary>
        /// <param name="url">L'URL dell'endpoint API da chiamare.</param>
        /// <param name="data">I dati da inviare nel corpo della richiesta POST.</param>
        /// <exception cref="ArgumentNullException">Sollevata se l'URL o i dati sono null.</exception>
        public HttpPostCall(string url, TParam data) : base(url) {
            if (object.Equals(data, default(TParam))) {
                throw new ArgumentNullException(nameof(data), "I dati non possono essere null.");
            }

            var jsonContent = JsonConvert.SerializeObject(data);
            _content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendRequestAsync() {
            if (_content == null) {
                throw new InvalidOperationException("Il contenuto della richiesta POST non può essere null.");
            }

            return HttpClient.PostAsync(Url, _content);
        }
    }
}
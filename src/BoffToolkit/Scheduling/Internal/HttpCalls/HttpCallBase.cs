using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BoffToolkit.Scheduling.HttpCalls;
using Newtonsoft.Json;

namespace BoffToolkit.Scheduling.Internal.HttpCalls {
    /// <summary>
    /// Classe base astratta per la gestione delle chiamate HTTP schedulabili.
    /// </summary>
    /// <typeparam name="TResult">Il tipo del risultato prodotto dalla chiamata HTTP.</typeparam>
    internal abstract class HttpCallBase<TResult> : IHttpCall<TResult> {
        /// <summary>
        /// Client HTTP utilizzato per inviare la richiesta.
        /// </summary>
        protected readonly HttpClient HttpClient;

        /// <summary>
        /// L'URL dell'endpoint API.
        /// </summary>
        protected readonly string Url;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="HttpCallBase{TResult}"/>.
        /// </summary>
        /// <param name="url">L'URL dell'endpoint API.</param>
        /// <exception cref="ArgumentNullException">Sollevata se l'URL è null.</exception>
        protected HttpCallBase(string url) {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url), "L'URL non può essere null o vuoto.");

            HttpClient = new HttpClient();
            Url = url;
        }

        /// <summary>
        /// Metodo astratto che deve essere implementato per inviare la richiesta HTTP specifica.
        /// </summary>
        /// <returns>Un <see cref="HttpResponseMessage"/> rappresentante la risposta HTTP.</returns>
        protected abstract Task<HttpResponseMessage> SendRequestAsync();

        /// <summary>
        /// Gestisce la risposta HTTP, garantendo il successo e deserializzando il contenuto della risposta.
        /// </summary>
        /// <param name="response">Il <see cref="HttpResponseMessage"/> da gestire.</param>
        /// <returns>Il risultato deserializzato dalla risposta HTTP.</returns>
        /// <exception cref="InvalidOperationException">Sollevata se la risposta non può essere deserializzata nel tipo specificato o è null.</exception>
        private static async Task<TResult> HandleResponseAsync(HttpResponseMessage response) {
            if (response == null)
                throw new ArgumentNullException(nameof(response), "La risposta HTTP non può essere null.");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException("Il contenuto della risposta è vuoto.");

            if (typeof(TResult) == typeof(string)) {
                // Se TResult è string, restituisci il contenuto direttamente
                return (TResult)(object)content;
            }

            try {
                // Log del contenuto della risposta
                Console.WriteLine("Contenuto della risposta JSON:");
                Console.WriteLine(content);

                // Deserializza il contenuto
                var result = JsonConvert.DeserializeObject<TResult>(content);

                // Solleva un'eccezione se il risultato è null
                if (result == null) {
                    throw new InvalidOperationException("La risposta non può essere deserializzata nel tipo specificato o è null.");
                }

                return result;
            }
            catch (JsonReaderException ex) {
                Console.WriteLine("Errore durante la deserializzazione del contenuto JSON: " + ex.Message);
                throw new InvalidOperationException("Errore durante la deserializzazione del contenuto JSON.", ex);
            }
        }

        /// <inheritdoc />
        public TResult Execute() {
            try {
                var response = SendRequestAsync().GetAwaiter().GetResult();
                return HandleResponseAsync(response).GetAwaiter().GetResult();
            }
            catch (AggregateException ex) {
                // Unwrap the AggregateException to get the actual exception
                throw ex.InnerException ?? ex;
            }
        }
    }
}
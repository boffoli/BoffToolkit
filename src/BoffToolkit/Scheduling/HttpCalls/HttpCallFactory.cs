using BoffToolkit.Scheduling.Internal.HttpCalls;

namespace BoffToolkit.Scheduling.HttpCalls {
    /// <summary>
    /// Classe factory per la creazione di istanze di chiamate HTTP.
    /// </summary>
    public static class HttpCallFactory {
        // Costanti per i messaggi di errore
        private const string UrlNullOrEmptyErrorMessage = "L'URL non può essere null o vuoto.";
        private const string DataNullErrorMessage = "I dati non possono essere null.";

        /// <summary>
        /// Crea un'istanza di <see cref="HttpGetCall{TResult}"/>.
        /// </summary>
        /// <typeparam name="TResult">Il tipo del risultato atteso dalla richiesta GET.</typeparam>
        /// <param name="url">L'URL dell'endpoint API.</param>
        /// <returns>Un'istanza di <see cref="IHttpCall{TResult}"/> che rappresenta la richiesta GET.</returns>
        /// <exception cref="ArgumentNullException">Se l'URL è null o vuoto.</exception>
        public static IHttpCall<TResult> CreateGetCall<TResult>(string url) {
            if (string.IsNullOrWhiteSpace(url)) {
                throw new ArgumentNullException(nameof(url), UrlNullOrEmptyErrorMessage);
            }
            return new HttpGetCall<TResult>(url);
        }

        /// <summary>
        /// Crea un'istanza di <see cref="HttpPostCall{TParam, TResult}"/>.
        /// </summary>
        /// <typeparam name="TParam">Il tipo dei dati inviati nel corpo della richiesta POST.</typeparam>
        /// <typeparam name="TResult">Il tipo del risultato atteso dalla richiesta POST.</typeparam>
        /// <param name="url">L'URL dell'endpoint API.</param>
        /// <param name="data">I dati da inviare nel corpo della richiesta.</param>
        /// <returns>Un'istanza di <see cref="IHttpCall{TResult}"/> che rappresenta la richiesta POST.</returns>
        /// <exception cref="ArgumentNullException">Se l'URL è null o vuoto, o se i dati sono null.</exception>
        public static IHttpCall<TResult> CreatePostCall<TParam, TResult>(string url, TParam data) {
            if (string.IsNullOrWhiteSpace(url)) {
                throw new ArgumentNullException(nameof(url), UrlNullOrEmptyErrorMessage);
            }
            if (object.Equals(data, default(TParam))) {
                throw new ArgumentNullException(nameof(data), DataNullErrorMessage);
            }
            return new HttpPostCall<TParam, TResult>(url, data);
        }

        /// <summary>
        /// Crea un'istanza di <see cref="HttpPutCall{TParam, TResult}"/>.
        /// </summary>
        /// <typeparam name="TParam">Il tipo dei dati inviati nel corpo della richiesta PUT.</typeparam>
        /// <typeparam name="TResult">Il tipo del risultato atteso dalla richiesta PUT.</typeparam>
        /// <param name="url">L'URL dell'endpoint API.</param>
        /// <param name="data">I dati da inviare nel corpo della richiesta.</param>
        /// <returns>Un'istanza di <see cref="IHttpCall{TResult}"/> che rappresenta la richiesta PUT.</returns>
        /// <exception cref="ArgumentNullException">Se l'URL è null o vuoto, o se i dati sono null.</exception>
        public static IHttpCall<TResult> CreatePutCall<TParam, TResult>(string url, TParam data) where TParam : class {
            if (string.IsNullOrWhiteSpace(url)) {
                throw new ArgumentNullException(nameof(url), DataNullErrorMessage);
            }
            if (data == null) {
                throw new ArgumentNullException(nameof(data), DataNullErrorMessage);
            }
            return new HttpPutCall<TParam, TResult>(url, data);
        }

        /// <summary>
        /// Crea un'istanza di <see cref="HttpDeleteCall{TResult}"/>.
        /// </summary>
        /// <typeparam name="TResult">Il tipo del risultato atteso dalla richiesta DELETE.</typeparam>
        /// <param name="url">L'URL dell'endpoint API.</param>
        /// <returns>Un'istanza di <see cref="IHttpCall{TResult}"/> che rappresenta la richiesta DELETE.</returns>
        /// <exception cref="ArgumentNullException">Se l'URL è null o vuoto.</exception>
        public static IHttpCall<TResult> CreateDeleteCall<TResult>(string url) {
            if (string.IsNullOrWhiteSpace(url)) {
                throw new ArgumentNullException(nameof(url), UrlNullOrEmptyErrorMessage);
            }
            return new HttpDeleteCall<TResult>(url);
        }
    }
}
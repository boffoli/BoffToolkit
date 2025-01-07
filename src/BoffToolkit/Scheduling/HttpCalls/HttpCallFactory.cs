using BoffToolkit.Scheduling.Internal.HttpCalls;

namespace BoffToolkit.Scheduling.HttpCalls {
    /// <summary>
    /// Factory class for creating HTTP call instances.
    /// </summary>
    public static class HttpCallFactory {
        // Constants for error messages
        private const string UrlNullOrEmptyErrorMessage = "The URL cannot be null or empty.";
        private const string DataNullErrorMessage = "The data cannot be null.";

        /// <summary>
        /// Creates an instance of <see cref="HttpGetCall{TResult}"/>.
        /// </summary>
        /// <typeparam name="TResult">The expected result type of the GET request.</typeparam>
        /// <param name="url">The API endpoint URL.</param>
        /// <returns>An instance of <see cref="IHttpCall{TResult}"/> representing the GET request.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the URL is null or empty.</exception>
        public static IHttpCall<TResult> CreateGetCall<TResult>(string url) {
            if (string.IsNullOrWhiteSpace(url)) {
                throw new ArgumentNullException(nameof(url), UrlNullOrEmptyErrorMessage);
            }
            return new HttpGetCall<TResult>(url);
        }

        /// <summary>
        /// Creates an instance of <see cref="HttpPostCall{TParam, TResult}"/>.
        /// </summary>
        /// <typeparam name="TParam">The type of the data sent in the POST request body.</typeparam>
        /// <typeparam name="TResult">The expected result type of the POST request.</typeparam>
        /// <param name="url">The API endpoint URL.</param>
        /// <param name="data">The data to send in the request body.</param>
        /// <returns>An instance of <see cref="IHttpCall{TResult}"/> representing the POST request.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the URL is null or empty, or if the data is null.</exception>
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
        /// Creates an instance of <see cref="HttpPutCall{TParam, TResult}"/>.
        /// </summary>
        /// <typeparam name="TParam">The type of the data sent in the PUT request body.</typeparam>
        /// <typeparam name="TResult">The expected result type of the PUT request.</typeparam>
        /// <param name="url">The API endpoint URL.</param>
        /// <param name="data">The data to send in the request body.</param>
        /// <returns>An instance of <see cref="IHttpCall{TResult}"/> representing the PUT request.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the URL is null or empty, or if the data is null.</exception>
        public static IHttpCall<TResult> CreatePutCall<TParam, TResult>(string url, TParam data) where TParam : class {
            if (string.IsNullOrWhiteSpace(url)) {
                throw new ArgumentNullException(nameof(url), UrlNullOrEmptyErrorMessage);
            }
            if (data == null) {
                throw new ArgumentNullException(nameof(data), DataNullErrorMessage);
            }
            return new HttpPutCall<TParam, TResult>(url, data);
        }

        /// <summary>
        /// Creates an instance of <see cref="HttpDeleteCall{TResult}"/>.
        /// </summary>
        /// <typeparam name="TResult">The expected result type of the DELETE request.</typeparam>
        /// <param name="url">The API endpoint URL.</param>
        /// <returns>An instance of <see cref="IHttpCall{TResult}"/> representing the DELETE request.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the URL is null or empty.</exception>
        public static IHttpCall<TResult> CreateDeleteCall<TResult>(string url) {
            if (string.IsNullOrWhiteSpace(url)) {
                throw new ArgumentNullException(nameof(url), UrlNullOrEmptyErrorMessage);
            }
            return new HttpDeleteCall<TResult>(url);
        }
    }
}
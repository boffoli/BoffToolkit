using System.Net.Http;
using BoffToolkit.Scheduling.HttpCalls;
using Newtonsoft.Json;

namespace BoffToolkit.Scheduling.Internal.HttpCalls {
    /// <summary>
    /// Abstract base class for handling schedulable HTTP calls.
    /// </summary>
    /// <typeparam name="TResult">The type of the result produced by the HTTP call.</typeparam>
    internal abstract class HttpCallBase<TResult> : IHttpCall<TResult> {
        /// <summary>
        /// The HTTP client used to send the request.
        /// </summary>
        protected readonly HttpClient HttpClient;

        /// <summary>
        /// The URL of the API endpoint.
        /// </summary>
        protected readonly string Url;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpCallBase{TResult}"/> class.
        /// </summary>
        /// <param name="url">The URL of the API endpoint.</param>
        /// <exception cref="ArgumentNullException">Thrown if the URL is null or empty.</exception>
        protected HttpCallBase(string url) {
            if (string.IsNullOrWhiteSpace(url)) {
                throw new ArgumentNullException(nameof(url), "The URL cannot be null or empty.");
            }

            HttpClient = new HttpClient();
            Url = url;
        }

        /// <summary>
        /// Abstract method that must be implemented to send the specific HTTP request.
        /// </summary>
        /// <returns>
        /// A <see cref="HttpResponseMessage"/> representing the HTTP response.
        /// </returns>
        protected abstract Task<HttpResponseMessage> SendRequestAsync();

        /// <summary>
        /// Handles the HTTP response, ensures success, and deserializes the response content.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponseMessage"/> to process.</param>
        /// <returns>
        /// The deserialized result from the HTTP response.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the response cannot be deserialized into the specified type or is null.
        /// </exception>
        private static async Task<TResult> HandleResponseAsync(HttpResponseMessage response) {
            if (response == null) {
                throw new ArgumentNullException(nameof(response), "The HTTP response cannot be null.");
            }

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content)) {
                throw new InvalidOperationException("The response content is empty.");
            }

            if (typeof(TResult) == typeof(string)) {
                // If TResult is string, return the content directly
                return (TResult)(object)content;
            }

            try {
                // Log the response content
                Console.WriteLine("Response JSON content:");
                Console.WriteLine(content);

                // Deserialize the content
                var result = JsonConvert.DeserializeObject<TResult>(content)
                    ?? throw new InvalidOperationException("The response cannot be deserialized into the specified type or is null.");

                return result;
            }
            catch (JsonReaderException ex) {
                Console.WriteLine("Error during JSON deserialization: " + ex.Message);
                throw new InvalidOperationException("Error during JSON deserialization.", ex);
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
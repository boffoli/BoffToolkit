using System.Net.Http;

namespace BoffToolkit.Scheduling.Internal.HttpCalls {
    /// <summary>
    /// Represents a schedulable HTTP GET call.
    /// </summary>
    /// <typeparam name="TResult">The expected type of the result from the GET call.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="HttpGetCall{TResult}"/> class.
    /// </remarks>
    /// <param name="url">The URL of the API endpoint to call.</param>
    internal sealed class HttpGetCall<TResult>(string url) : HttpCallBase<TResult>(url) {

        /// <summary>
        /// Sends the HTTP GET request asynchronously.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous operation, 
        /// containing the HTTP response message.
        /// </returns>
        protected override Task<HttpResponseMessage> SendRequestAsync() {
            return HttpClient.GetAsync(Url);
        }
    }
}
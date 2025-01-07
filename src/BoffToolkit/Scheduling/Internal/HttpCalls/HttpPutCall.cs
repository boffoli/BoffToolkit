using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BoffToolkit.Scheduling.Internal.HttpCalls {
    /// <summary>
    /// Represents a schedulable HTTP PUT call.
    /// </summary>
    /// <typeparam name="TResult">The expected type of the result from the PUT call.</typeparam>
    /// <typeparam name="TParam">The type of the data sent in the body of the PUT request.</typeparam>
    internal sealed class HttpPutCall<TParam, TResult> : HttpCallBase<TResult> {
        private readonly HttpContent _content;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPutCall{TParam, TResult}"/> class.
        /// </summary>
        /// <param name="url">The URL of the API endpoint to call.</param>
        /// <param name="data">The data to send in the body of the PUT request.</param>
        /// <exception cref="ArgumentNullException">Thrown if the URL or the data is null.</exception>
        public HttpPutCall(string url, TParam data) : base(url) {
            if (object.Equals(data, default(TParam))) {
                throw new ArgumentNullException(nameof(data), "The data cannot be null.");
            }

            var jsonContent = JsonConvert.SerializeObject(data);
            _content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendRequestAsync() {
            if (_content == null) {
                throw new InvalidOperationException("The content of the PUT request cannot be null.");
            }

            return HttpClient.PutAsync(Url, _content);
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Intelecom.DirectPayment.Client.Tests.Helpers
{
    public class FakeRequestHandler : HttpClientHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly Func<HttpRequestMessage, object> _responseFunc;

        public FakeRequestHandler(HttpStatusCode statusCode, Func<HttpRequestMessage, object> responseFunc)
        {
            if (responseFunc == null)
            {
                throw new ArgumentNullException(nameof(responseFunc));
            }

            _statusCode = statusCode;
            _responseFunc = responseFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = _responseFunc(request);

            return Task.FromResult(
                response != null
                ? new HttpResponseMessage(_statusCode) { Content = new StringContent(JsonConvert.SerializeObject(response)) }
                : new HttpResponseMessage(_statusCode) { Content = new StringContent("<html></html>", Encoding.UTF8, "text/html") });
        }
    }
}
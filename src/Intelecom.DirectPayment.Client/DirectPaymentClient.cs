using Intelecom.DirectPayment.Client.Constants;
using Intelecom.DirectPayment.Client.Exceptions;
using Intelecom.DirectPayment.Client.Extensions;
using Intelecom.DirectPayment.Client.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Intelecom.DirectPayment.Client
{
    /// <summary>
    /// Direct payment client.
    /// </summary>
    public class DirectPaymentClient : IDirectPaymentClient, IDisposable
    {
        private const string BaseUri = "https://directpayment.intele.com/restV1.svc/service";
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectPaymentClient"/> class.
        /// </summary>
        /// <param name="credentials">Account credentials.</param>
        public DirectPaymentClient(DirectPaymentCredentials credentials)
            : this(BaseUri, credentials, new HttpClientHandler())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectPaymentClient"/> class.
        /// </summary>
        /// <param name="baseUri">Base URI.</param>
        /// <param name="credentials">Account credentials.</param>
        /// <param name="handler">HTTP client handler.</param>
        /// <exception cref="ArgumentException">Thrown if the base URI is null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="credentials"/> or <paramref name="handler"/> is null.</exception>
        public DirectPaymentClient(string baseUri, DirectPaymentCredentials credentials, HttpClientHandler handler)
        {
            if (string.IsNullOrEmpty(baseUri))
            {
                throw new ArgumentException("Argument is null or empty", nameof(baseUri));
            }

            if (credentials == null)
            {
                throw new ArgumentNullException(nameof(credentials));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (baseUri.EndsWith("/"))
            {
                baseUri = baseUri.Substring(0, baseUri.Length - 1);
            }

            _client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"{baseUri}/{credentials.ServiceId}/"),
                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Basic", credentials.ToBasicAuthBase64String()) }
            };
        }

        /// <summary>
        /// Used to directly charge a mobile subscription with a provided amount.
        /// </summary>
        /// <param name="request">Payment request.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <see cref="PaymentRequest"/> is null.</exception>
        /// <returns>The payment response.</returns>
        public async Task<PaymentResponse> PayAsync(PaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var content = request.CreateStringContent(_serializerSettings);
            var responseMessage = await _client.PostAsync(RelativeUri.Pay, content).ConfigureAwait(false);
            await CheckIfFailedRequestAsync(responseMessage);

            return await responseMessage.DeserializeAsAsync<PaymentResponse>();
        }

        /// <summary>
        /// Used to cancel a payment already made.
        /// The transaction as a whole is then credited the mobile phone subscription.
        /// It is not possible to reverse only part of a payment.
        /// </summary>
        /// <param name="details">Details about the transaction.</param>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="ReversePaymentDetails"/> is null.</exception>
        /// <returns>The reverse payment response.</returns>
        public async Task<ReversePaymentDetails> ReversePaymentAsync(ReversePaymentDetails details)
        {
            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            var responseMessage = await _client.DeleteAsync($"{RelativeUri.Pay}/{details.TransactionId}").ConfigureAwait(false);
            await CheckIfFailedRequestAsync(responseMessage);

            return await responseMessage.DeserializeAsAsync<ReversePaymentDetails>();
        }

        /// <summary>
        /// Gets the payment status of a specific transaction.
        /// It can be useful in cases where something goes wrong in the payment or
        /// reverse payment flow and your client is unsure of the outcome of the operation.
        /// </summary>
        /// <param name="request">Payment status request.</param>
        /// <returns>The payment status response.</returns>
        public async Task<PaymentStatusResponse> GetPaymentStatusAsync(PaymentStatusRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var responseMessage = await _client.GetAsync($"{RelativeUri.Pay}/{request.ClientReference}").ConfigureAwait(false);
            await CheckIfFailedRequestAsync(responseMessage);

            return await responseMessage.DeserializeAsAsync<PaymentStatusResponse>();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">True if called from <see cref="Dispose()"/>, false if called from the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client?.Dispose();
            }
        }

        // TODO: Accept error message
        private static async Task CheckIfFailedRequestAsync(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                try
                {
                    var fault = await responseMessage.DeserializeAsAsync<DirectPaymentFault>();
                    throw new DirectPaymentException(fault, "Payment failed", null);
                }
                catch (Exception e) when (!(e is DirectPaymentException))
                {
                    throw new DirectPaymentException("Payment failed", e);
                }
            }
        }
    }
}
using Intelecom.DirectPayment.Client.Exceptions;
using Intelecom.DirectPayment.Client.Models;
using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Intelecom.DirectPayment.Client.Extensions
{
    /// <summary>
    /// <see cref="ConfiguredTaskAwaitable{TResult}" /> extension methods.
    /// </summary>
    internal static class ConfiguredTaskAwaitableExtensions
    {
        // TODO: Accept error message

        /// <summary>
        /// Throws a <see cref="DirectPaymentException"/> if the response status code indicates an error.
        /// </summary>
        /// <param name="awaitable">The awaitable.</param>
        /// <returns>A task that returns an <see cref="HttpResponseMessage"/>.</returns>
        /// <exception cref="DirectPaymentException">Thrown in case of errors.</exception>
        public static async Task<HttpResponseMessage> ThrowIfErrorResponseAsync(this ConfiguredTaskAwaitable<HttpResponseMessage> awaitable)
        {
            var responseMessage = await awaitable;

            if (!responseMessage.IsSuccessStatusCode)
            {
                try
                {
                    var fault = await responseMessage.DeserializeAsync<DirectPaymentFault>();
                    throw new DirectPaymentException(fault, "Payment failed", null);
                }
                catch (Exception e) when (!(e is DirectPaymentException))
                {
                    throw new DirectPaymentException("Payment failed", e);
                }
            }

            return responseMessage;
        }
    }
}
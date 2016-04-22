using Intelecom.DirectPayment.Client.Models;
using System.Threading.Tasks;

namespace Intelecom.DirectPayment.Client
{
    /// <summary>
    /// Direct payment client.
    /// </summary>
    public interface IDirectPaymentClient
    {
        /// <summary>
        /// Performs a payment.
        /// </summary>
        /// <param name="request">Payment request.</param>
        /// <returns>Payment response.</returns>
        Task<PaymentResponse> PayAsync(PaymentRequest request);
    }
}
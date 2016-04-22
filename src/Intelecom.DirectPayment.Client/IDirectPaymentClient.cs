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
        /// Used to directly charge a mobile subscription with a provided amount.
        /// </summary>
        /// <param name="request">Payment request.</param>
        /// <returns>The payment response.</returns>
        Task<PaymentResponse> PayAsync(PaymentRequest request);
    }
}
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

        /// <summary>
        /// Used to cancel a payment already made.
        /// The transaction as a whole is then credited the mobile phone subscription.
        /// It is not possible to reverse only part of a payment.
        /// </summary>
        /// <param name="details">Details about the transaction.</param>
        /// <returns>The reverse payment response.</returns>
        Task<ReversePaymentDetails> ReversePaymentAsync(ReversePaymentDetails details);

        /// <summary>
        /// Gets the payment status of a specific transaction.
        /// It can be useful in cases where something goes wrong in the payment or
        /// reverse payment flow and your client is unsure of the outcome of the operation.
        /// </summary>
        /// <param name="request">Payment status request.</param>
        /// <returns>The payment status response.</returns>
        Task<PaymentStatusResponse> GetPaymentStatusAsync(PaymentStatusRequest request);
    }
}
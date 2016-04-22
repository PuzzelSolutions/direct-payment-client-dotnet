using System;

namespace Intelecom.DirectPayment.Client.Models
{
    /// <summary>
    /// Payment request.
    /// </summary>
    public class PaymentRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRequest"/> class.
        /// </summary>
        /// <param name="paymentDetails">Payment details.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="paymentDetails"/> is invalid.</exception>
        public PaymentRequest(PaymentDetails paymentDetails)
        {
            if (paymentDetails == null)
            {
                throw new ArgumentNullException(nameof(paymentDetails));
            }

            PaymentDetails = paymentDetails;
        }

        /// <summary>
        /// Gets the payment details.
        /// </summary>
        public PaymentDetails PaymentDetails { get; }

        /// <inheritdoc/>
        public override string ToString() => $"PaymentDetails: {PaymentDetails}";
    }
}
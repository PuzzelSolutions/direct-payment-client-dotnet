using System;

namespace Intelecom.DirectPayment.Client.Models
{
    /// <summary>
    /// Contains details about the payment that's being reversed.
    /// </summary>
    public class ReversePaymentDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReversePaymentDetails"/> class.
        /// </summary>
        /// <param name="transactionId">The unique transaction ID.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the transaction ID is invalid.</exception>
        public ReversePaymentDetails(int transactionId)
        {
            if (transactionId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(transactionId));
            }

            TransactionId = transactionId;
        }

        /// <summary>
        /// Gets or sets the transaction ID.
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"TransactionId: {TransactionId}";
    }
}
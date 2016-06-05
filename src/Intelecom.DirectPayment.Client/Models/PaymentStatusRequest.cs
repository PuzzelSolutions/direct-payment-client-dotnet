using System;

namespace Intelecom.DirectPayment.Client.Models
{
    /// <summary>
    /// Payment status request.
    /// </summary>
    public class PaymentStatusRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentStatusRequest"/> class.
        /// </summary>
        /// <param name="clientReference">Client reference.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="clientReference"></paramref> is invalid.</exception>
        public PaymentStatusRequest(string clientReference)
        {
            if (string.IsNullOrEmpty(clientReference))
            {
                throw new ArgumentException("Argument is null or empty", nameof(clientReference));
            }

            ClientReference = clientReference;
        }

        /// <summary>
        /// Gets or sets the client reference.
        /// </summary>
        public string ClientReference { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"ClientReference: {ClientReference}";
    }
}
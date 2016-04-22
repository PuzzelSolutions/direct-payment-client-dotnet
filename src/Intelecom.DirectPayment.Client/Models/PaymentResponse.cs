namespace Intelecom.DirectPayment.Client.Models
{
    /// <summary>
    /// Payment response.
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// Gets or sets the client reference.
        /// </summary>
        public string ClientReference { get; set; }

        /// <summary>
        /// Gets or sets the transaction ID.
        /// </summary>
        public int TransactionId { get; set; }

        /// <inheritdoc/>
        public override string ToString() => $"ClientReference: {ClientReference}, TransactionId: {TransactionId}";
    }
}
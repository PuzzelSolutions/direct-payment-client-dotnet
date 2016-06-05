namespace Intelecom.DirectPayment.Client.Models
{
    /// <summary>
    /// Details regarding the payment status.
    /// </summary>
    public class PaymentStatusResponse
    {
        /// <summary>
        /// Gets or sets the client reference.
        /// </summary>
        public string ClientReference { get; set; }

        /// <summary>
        /// Gets or sets the payment status.
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"ClientReference: {ClientReference}, PaymentStatus: {PaymentStatus}";
    }
}
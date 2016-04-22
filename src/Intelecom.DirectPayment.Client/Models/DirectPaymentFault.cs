namespace Intelecom.DirectPayment.Client.Models
{
    /// <summary>
    /// Fault details.
    /// </summary>
    public class DirectPaymentFault
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <inheritdoc/>
        public override string ToString() => $"Code: {Code}, Description: {Description}";
    }
}
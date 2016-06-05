namespace Intelecom.DirectPayment.Client.Models
{
    /// <summary>
    /// Payment status.
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Not paid.
        /// </summary>
        NotPaid = 0,

        /// <summary>
        /// Paid.
        /// </summary>
        Paid = 1,

        /// <summary>
        /// Reversed.
        /// </summary>
        Reversed = 2
    }
}
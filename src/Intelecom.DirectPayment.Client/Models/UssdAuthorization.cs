namespace Intelecom.DirectPayment.Client.Models
{
    /// <summary>
    /// USSD Authorization type
    /// </summary>
    public enum UssdAuthorization
    {
        /// <summary>
        /// No USSD confirmation needed
        /// </summary>
        None = 1,


        /// <summary>
        /// USSD confirmation needed
        /// </summary>
        Confirmation = 2
    }
}

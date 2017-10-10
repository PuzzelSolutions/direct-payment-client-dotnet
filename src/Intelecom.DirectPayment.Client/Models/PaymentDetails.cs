using System;

namespace Intelecom.DirectPayment.Client.Models
{
    /// <summary>
    /// Details about the payment.
    /// </summary>
    public class PaymentDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentDetails"/> class.
        /// </summary>
        /// <param name="msisdn">The MSISDN that should be charged. The format should follow the ITU-T E.164 standard with a + prefix.</param>
        /// <param name="price">The amount that should be debited, in lowest monetary unit. Example: 100 (1,- NOK).</param>
        /// <param name="serviceCode">Service code identifying the type of transaction.</param>
        /// <param name="clientReference">Client reference for the transaction, must be unique.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="msisdn"/>, <paramref name="serviceCode"/> or <paramref name="clientReference"/> is invalid.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="price"/> is invalid.</exception>
        public PaymentDetails(string msisdn, int price, string serviceCode, string clientReference)
        {
            if (string.IsNullOrEmpty(msisdn))
            {
                throw new ArgumentException("Argument is null or empty", nameof(msisdn));
            }

            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price));
            }

            if (string.IsNullOrEmpty(serviceCode))
            {
                throw new ArgumentException("Argument is null or empty", nameof(serviceCode));
            }

            if (string.IsNullOrEmpty(clientReference))
            {
                throw new ArgumentException("Argument is null or empty", nameof(clientReference));
            }

            Msisdn = msisdn;
            Price = price;
            ServiceCode = serviceCode;
            ClientReference = clientReference;
        }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Gets the client reference for the transaction
        /// </summary>
        public string ClientReference { get; }

        /// <summary>
        /// Gets or sets the differentiator.
        /// </summary>
        public string Differentiator { get; set; }

        /// <summary>
        /// Gets or sets the end-user invoice text.
        /// </summary>
        public string EndUserInvoiceText { get; set; }

        /// <summary>
        /// Gets or sets the invoice node.
        /// </summary>
        public string InvoiceNode { get; set; }

        /// <summary>
        /// Gets the MSISDN that should be charged.
        /// </summary>
        public string Msisdn { get; }

        /// <summary>
        /// Gets the amount that should be debited, in lowest monetary unit. Example: 100 (1,- NOK).
        /// </summary>
        public int Price { get; }

        /// <summary>
        /// Gets the service code identifying the type of transaction.
        /// </summary>
        public string ServiceCode { get; }

        /// <summary>
        /// Gets or sets the business model.
        /// </summary>
        public string BusinessModel { get; set; }

        /// <summary>
        /// Gets or sets the Ussd Authorization flag
        /// 1=None, 2=Confirmation.
        /// </summary>
        public UssdAuthorization? UssdAuthorization { get; set; }

        /// <inheritdoc/>
        public override string ToString() => $"{nameof(Age)}: {Age}, {nameof(ClientReference)}: {ClientReference}, {nameof(Differentiator)}: {Differentiator}, {nameof(EndUserInvoiceText)}: {EndUserInvoiceText}, {nameof(InvoiceNode)}: {InvoiceNode}, {nameof(Msisdn)}: {Msisdn}, {nameof(Price)}: {Price}, {nameof(ServiceCode)}: {ServiceCode}, {nameof(BusinessModel)}: {BusinessModel}, {nameof(UssdAuthorization)}: {UssdAuthorization}";
    }
}
using Intelecom.DirectPayment.Client.Models;
using System;

namespace Intelecom.DirectPayment.Client.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a Direct Payment API error occurs.
    /// </summary>
    public class DirectPaymentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectPaymentException"/> class.
        /// </summary>
        /// <param name="fault">Fault details.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="fault"/> is invalid.</exception>
        public DirectPaymentException(DirectPaymentFault fault, string message, Exception innerException)
            : this(message, innerException)
        {
            if (fault == null)
            {
                throw new ArgumentNullException(nameof(fault));
            }

            Fault = fault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectPaymentException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public DirectPaymentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets the fault.
        /// </summary>
        public DirectPaymentFault Fault { get; private set; }
    }
}
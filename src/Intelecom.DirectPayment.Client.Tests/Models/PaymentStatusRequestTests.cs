using Intelecom.DirectPayment.Client.Models;
using NUnit.Framework;
using System;

namespace Intelecom.DirectPayment.Client.Tests.Models
{
    public class PaymentStatusRequestTests
    {
        [TestCase(null)]
        [TestCase("")]
        public void Ctor_ShouldThrowExceptionIfInvalidTransactionId(string clientReference)
        {
            Assert.Throws<ArgumentException>(() => new PaymentStatusRequest(clientReference));
        }
    }
}
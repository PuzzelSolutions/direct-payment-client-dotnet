using System;
using Intelecom.DirectPayment.Client.Models;
using NUnit.Framework;

namespace Intelecom.DirectPayment.Client.Tests.Models
{
    public class PaymentRequestTests
    {
        [Test]
        public void Ctor_ShouldThrowExceptionIfInvalidPaymentDetails()
        {
            Assert.Throws<ArgumentNullException>(() => new PaymentRequest(null));
        }
    }
}
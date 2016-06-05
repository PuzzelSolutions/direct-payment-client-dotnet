using Intelecom.DirectPayment.Client.Models;
using NUnit.Framework;
using System;

namespace Intelecom.DirectPayment.Client.Tests.Models
{
    public class ReversePaymentDetailsTests
    {
        [TestCase(-1)]
        [TestCase(-0)]
        public void Ctor_ShouldThrowExceptionIfInvalidTransactionId(int transactionId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReversePaymentDetails(transactionId));
        }
    }
}
using Intelecom.DirectPayment.Client.Models;
using NUnit.Framework;
using System;

namespace Intelecom.DirectPayment.Client.Tests.Models
{
    public class PaymentDetailsTests
    {
        private const string DummyMsisdn = "+4712345678";
        private const int DummyPrice = 100;
        private const string DummyServiceCode = "service code";
        private const string DummyClientReference = "client ref";

        [Test]
        public void Ctor_ShouldThrowExceptionIfInvalidMsisdn()
        {
            Assert.Throws<ArgumentException>(() => new PaymentDetails(null, DummyPrice, DummyServiceCode, DummyClientReference));
            Assert.Throws<ArgumentException>(() => new PaymentDetails("", DummyPrice, DummyServiceCode, DummyClientReference));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Ctor_ShouldThrowExceptionIfInvalidPrice(int price)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PaymentDetails(DummyMsisdn, price, DummyServiceCode, DummyClientReference));
        }

        [Test]
        public void Ctor_ShouldThrowExceptionIfInvalidServiceCode()
        {
            Assert.Throws<ArgumentException>(() => new PaymentDetails(DummyMsisdn, DummyPrice, null, DummyClientReference));
            Assert.Throws<ArgumentException>(() => new PaymentDetails(DummyMsisdn, DummyPrice, "", DummyClientReference));
        }

        [Test]
        public void Ctor_ShouldThrowExceptionIfInvalidClientReference()
        {
            Assert.Throws<ArgumentException>(() => new PaymentDetails(DummyMsisdn, DummyPrice, DummyServiceCode, null));
            Assert.Throws<ArgumentException>(() => new PaymentDetails(DummyMsisdn, DummyPrice, DummyServiceCode, ""));
        }
    }
}
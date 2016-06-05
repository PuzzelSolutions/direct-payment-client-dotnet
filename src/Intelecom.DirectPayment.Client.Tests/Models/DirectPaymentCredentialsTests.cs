using System;
using Intelecom.DirectPayment.Client.Models;
using NUnit.Framework;

namespace Intelecom.DirectPayment.Client.Tests.Models
{
    public class DirectPaymentCredentialsTests
    {
        private const int DummySeviceId = 1;
        private const string DummyUsername = "username";
        private const string DummyPassword = "password";

        [Test]
        public void Ctor_ShouldThrowExceptionIfInvalidServiceId()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DirectPaymentCredentials(0, DummyUsername, DummyPassword));
        }

        [Test]
        public void Ctor_ShouldThrowExceptionIfInvalidUsername()
        {
            Assert.Throws<ArgumentException>(() => new DirectPaymentCredentials(DummySeviceId, null, DummyPassword));
            Assert.Throws<ArgumentException>(() => new DirectPaymentCredentials(DummySeviceId, "", DummyPassword));
        }

        [Test]
        public void Ctor_ShouldThrowExceptionIfInvalidPassword()
        {
            Assert.Throws<ArgumentException>(() => new DirectPaymentCredentials(DummySeviceId, DummyUsername, null));
            Assert.Throws<ArgumentException>(() => new DirectPaymentCredentials(DummySeviceId, DummyUsername, ""));
        }
    }
}
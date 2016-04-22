using Intelecom.DirectPayment.Client.Exceptions;
using Intelecom.DirectPayment.Client.Models;
using Intelecom.DirectPayment.Client.Tests.Helpers;
using NUnit.Framework;
using Should;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Intelecom.DirectPayment.Client.Tests
{
    public class DirectPaymentClientTests
    {
        private readonly DirectPaymentCredentials _validDummyCredentials = new DirectPaymentCredentials(1, "username", "password");
        private readonly string _guid = Guid.NewGuid().ToString();
        private readonly int _randomInteger = new Random().Next(100, int.MaxValue);
        private const string DummyUri = "https://dummy.uri";
        private const string DummyMsisdn = "+4712345678";

        private DirectPaymentClient CreateValidClient(HttpStatusCode statusCode, Func<object> responseFunc)
        {
            if (responseFunc == null)
            {
                throw new ArgumentNullException(nameof(responseFunc));
            }

            var fakeRequestHandler = new FakeRequestHandler(statusCode, responseFunc);
            var client = new DirectPaymentClient(DummyUri, _validDummyCredentials, fakeRequestHandler);

            return client;
        }

        public class Ctor : DirectPaymentClientTests
        {
            [Test]
            public void WhenCredentialsIsNull_ShouldThrowArgumentException()
            {
                Assert.Throws<ArgumentNullException>(() => new DirectPaymentClient(null));
                Assert.Throws<ArgumentNullException>(() => new DirectPaymentClient(DummyUri, null, new HttpClientHandler()));
            }

            [TestCase("")]
            [TestCase(null)]
            public void WhenBaseUriIsInvalid_ShouldThrowArgumentException(string baseUri)
            {
                Assert.Throws<ArgumentException>(() => new DirectPaymentClient(baseUri, _validDummyCredentials, new HttpClientHandler()));
            }

            [Test]
            public void WhenHandlerIsNull_ShouldThrowArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(() => new DirectPaymentClient(DummyUri, _validDummyCredentials, null));
            }
        }

        public class PayAsync : DirectPaymentClientTests
        {
            [Test]
            public async Task WhenRequestSucceeds_ShouldReturnPaymentResponse()
            {
                Func<PaymentResponse> responseFunc = () => new PaymentResponse
                {
                    ClientReference = _guid,
                    TransactionId = _randomInteger
                };
                var client = CreateValidClient(HttpStatusCode.OK, responseFunc);
                var request = new PaymentRequest(new PaymentDetails(DummyMsisdn, _randomInteger, _guid, _guid));

                var response = await client.PayAsync(request);

                response.ClientReference.ShouldEqual(_guid);
                response.TransactionId.ShouldEqual(_randomInteger);
            }

            [Test]
            public async Task WhenRequestFailsWithFault_ShouldThrowDirectPaymentExceptionContainingFault()
            {
                Func<DirectPaymentFault> responseFunc = () => new DirectPaymentFault { Code = _randomInteger, Description = _guid };
                var client = CreateValidClient(HttpStatusCode.BadRequest, responseFunc);
                var request = new PaymentRequest(new PaymentDetails(DummyMsisdn, _randomInteger, _guid, _guid));

                try
                {
                    await client.PayAsync(request);
                    Assert.Fail();
                }
                catch (DirectPaymentException e)
                {
                    e.Fault.Code.ShouldEqual(_randomInteger);
                    e.Fault.Description.ShouldEqual(_guid);
                    e.InnerException.ShouldBeNull();
                }
                catch (Exception e)
                {
                    Assert.Fail(e.Message);
                }
            }

            [Test]
            public async Task WhenRequestFailsWithoutFault_ShouldThrowDirectPaymentExceptionWithInnerException()
            {
                Func<PaymentResponse> responseFunc = () => null;
                var client = CreateValidClient(HttpStatusCode.InternalServerError, responseFunc);
                var request = new PaymentRequest(new PaymentDetails(DummyMsisdn, _randomInteger, _guid, _guid));

                try
                {
                    await client.PayAsync(request);
                    Assert.Fail();
                }
                catch (DirectPaymentException e)
                {
                    e.Fault.ShouldBeNull();
                    e.InnerException.ShouldNotBeNull();
                }
                catch (Exception e)
                {
                    Assert.Fail(e.Message);
                }
            }
        }
    }
}

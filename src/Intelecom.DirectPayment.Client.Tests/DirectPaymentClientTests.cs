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
        private const string DummyUri = "https://dummy.uri/service";
        private const string DummyMsisdn = "+4712345678";

        private DirectPaymentClient CreateValidClient(HttpStatusCode statusCode, Func<HttpRequestMessage, object> responseFunc)
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

            [Test]
            public async Task WhenPassedBaseUri_ShouldHandleBothWithAndWithoutTrailingSlash()
            {
                var expected = new Uri("http://fake.com/1/pay");
                var handler = new FakeRequestHandler(HttpStatusCode.OK, requestMessage =>
                                                                        {
                                                                            requestMessage.RequestUri.ShouldEqual(expected);
                                                                            return new { };
                                                                        });
                var request = new PaymentRequest(new PaymentDetails(DummyMsisdn, _randomInteger, _guid, _guid));
                await new DirectPaymentClient("http://fake.com", _validDummyCredentials, handler).PayAsync(request);
                await new DirectPaymentClient("http://fake.com/", _validDummyCredentials, handler).PayAsync(request);
            }
        }

        public class PayAsync : DirectPaymentClientTests
        {
            [Test]
            public async Task WhenRequestSucceeds_ShouldReturnPaymentResponse()
            {
                Func<HttpRequestMessage, PaymentResponse> responseFunc = requestMessage => new PaymentResponse
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
                Func<HttpRequestMessage, DirectPaymentFault> responseFunc = requestMessage => new DirectPaymentFault { Code = _randomInteger, Description = _guid };
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
                Func<HttpRequestMessage, PaymentResponse> responseFunc = requestMessage => null;
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

        public class ReversePaymentAsync : DirectPaymentClientTests
        {
            [Test]
            public async Task WhenRequestSucceeds_ShouldReturnReversePaymentResponse()
            {
                Func<HttpRequestMessage, ReversePaymentDetails> responseFunc = requestMessage => new ReversePaymentDetails(_randomInteger);
                var client = CreateValidClient(HttpStatusCode.OK, responseFunc);

                try
                {
                    var response = await client.ReversePaymentAsync(new ReversePaymentDetails(_randomInteger));
                    response.TransactionId.ShouldEqual(_randomInteger);
                }
                catch (Exception e)
                {
                    Assert.Fail(e.Message);
                }
            }

            [Test]
            public async Task WhenRequestFailsWithFault_ShouldThrowDirectPaymentExceptionContainingFault()
            {
                Func<HttpRequestMessage, DirectPaymentFault> responseFunc = requestMessage => new DirectPaymentFault { Code = _randomInteger, Description = _guid };
                var client = CreateValidClient(HttpStatusCode.Forbidden, responseFunc);

                try
                {
                    await client.ReversePaymentAsync(new ReversePaymentDetails(_randomInteger));
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
                Func<HttpRequestMessage, DirectPaymentFault> responseFunc = requestMessage => null;
                var client = CreateValidClient(HttpStatusCode.InternalServerError, responseFunc);

                try
                {
                    await client.ReversePaymentAsync(new ReversePaymentDetails(_randomInteger));
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

        public class GetPaymentStatusAsync : DirectPaymentClientTests
        {
            [Test]
            public async Task WhenRequestIsNull_ShouldThrowArgumentNullException()
            {
                var client = CreateValidClient(HttpStatusCode.OK, message => null);

                try
                {
                    await client.GetPaymentStatusAsync(null);
                    Assert.Fail();
                }
                catch (ArgumentNullException)
                {
                }
                catch (Exception e)
                {
                    Assert.Fail(e.Message);
                }
            }

            [Test]
            public async Task WhenValidRequest_ShouldReturnStatusResponse()
            {
                var client = CreateValidClient(HttpStatusCode.OK, message => new PaymentStatusResponse { ClientReference = _guid, PaymentStatus = PaymentStatus.Paid });

                var response = await client.GetPaymentStatusAsync(new PaymentStatusRequest(_guid));

                response.ClientReference.ShouldEqual(_guid);
                response.PaymentStatus.ShouldEqual(PaymentStatus.Paid);
            }

            [Test]
            public async Task WhenRequestFailsWithFault_ShouldThrowDirectPaymentExceptionContainingFault()
            {
                Func<HttpRequestMessage, DirectPaymentFault> responseFunc = requestMessage => new DirectPaymentFault { Code = _randomInteger, Description = _guid };
                var client = CreateValidClient(HttpStatusCode.NotFound, responseFunc);

                try
                {
                    await client.GetPaymentStatusAsync(new PaymentStatusRequest(_guid));
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
                Func<HttpRequestMessage, DirectPaymentFault> responseFunc = requestMessage => null;
                var client = CreateValidClient(HttpStatusCode.InternalServerError, responseFunc);

                try
                {
                    await client.GetPaymentStatusAsync(new PaymentStatusRequest(_guid));
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

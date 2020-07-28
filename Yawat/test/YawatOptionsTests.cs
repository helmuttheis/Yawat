namespace Yawat.Tests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using NUnit.Framework;

    using Yawat.Interfaces;
    using Yawat.ResponseData;

    [TestFixture]
    public class YawatOptionsTests
    {
        [Test]
        public void ShouldCreateOptions()
        {
            var options = new Yawat.YawatOptions(null)
            {
                Host = "localhost",
                Protocol = "http",
                Port = "5000",
            };
            Assert.AreEqual(options.Host, "localhost");
            Assert.AreEqual(options.Protocol, "http");
            Assert.AreEqual(options.Port, "5000");
        }

        [Test]
        public void ShouldOverrideProperties()
        {
            var options1 = new Yawat.YawatOptions(typeof(XmlResponseData))
            {
                Host = "localhost",
                Protocol = "http",
                Port = "5000",
                Authenticator = new Authenticator1(),
                IncludeHeaders = true,
            };
            var options2 = new Yawat.YawatOptions(typeof(JsonResponseData))
            {
                Host = "remote",
                Protocol = "http",
                Port = "5000",
                Authenticator = new Authenticator2(),
                IncludeHeaders = false,
            };

            var options3 = options1.OverwrittenBy(options2);

            Assert.AreEqual(options3.Host, options2.Host);
            Assert.IsTrue(options3.Authenticator is Authenticator2);
            Assert.AreEqual(options3.ResponseType, typeof(JsonResponseData));

            Assert.True(options3.IncludeHeaders);
        }

        private class Authenticator1 : IAuthenticator
        {
            List<string> IAuthenticator.GetErrors()
            {
                throw new System.NotImplementedException();
            }

            Task<bool> IAuthenticator.Setup()
            {
                throw new System.NotImplementedException();
            }

            bool IAuthenticator.UpdateHttpClient(HttpClient client)
            {
                throw new System.NotImplementedException();
            }
        }

        private class Authenticator2 : IAuthenticator
        {
            List<string> IAuthenticator.GetErrors()
            {
                throw new System.NotImplementedException();
            }

            Task<bool> IAuthenticator.Setup()
            {
                throw new System.NotImplementedException();
            }

            bool IAuthenticator.UpdateHttpClient(HttpClient client)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}

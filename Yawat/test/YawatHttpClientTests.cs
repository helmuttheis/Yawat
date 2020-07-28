namespace Yawat.Tests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using Yawat.MockHttpClient;
    using Yawat.RequestData;
    using Yawat.ResponseData;

    [TestFixture]
    public class YawatHttpClientTests
    {
        [Test]
        public async Task ShouldUseMockJsonData()
        {
            var definition = new { test = string.Empty };
            var jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(new { test = "Test" });

            var options = new Yawat.YawatOptions(typeof(JsonResponseData))
            {
                Host = "localhost",
                Protocol = "http",
                Port = "5000",
                MockHttpClient = new YawatMockHttpClient()
                {
                    EndpointHandlersDictionary = new Dictionary<string, YawatMockEndpoint>()
                    {
                        {
                            "/test",
                            new YawatMockEndpoint()
                            {
                                StatusCode = HttpStatusCode.OK,
                                Data = jsonText
                            }
                        }
                    }
                }
            };
            var httpClient = new YawatHttpClient(options);

            var response = await httpClient.GetAsync("/test");

            var typedResponse = response.AsAnonymous(definition);

            Assert.AreEqual(typedResponse.test, "Test");
        }

        [Test]
        public async Task ShouldUseMockXmlData()
        {
            var definition = new { test = string.Empty };
            var jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(new { test = "Test" });

            var doc = JsonConvert.DeserializeXmlNode(jsonText);
            Assert.NotNull(doc);

            var xmlText = doc.DocumentElement.OuterXml;

            var options = new Yawat.YawatOptions(typeof(XmlResponseData))
            {
                Host = "localhost",
                Protocol = "http",
                Port = "5000",
                MockHttpClient = new YawatMockHttpClient()
                {
                    EndpointHandlersDictionary = new Dictionary<string, YawatMockEndpoint>()
                    {
                        {
                            "/test",
                            new YawatMockEndpoint()
                            {
                                StatusCode = HttpStatusCode.OK,
                                Data = xmlText
                            }
                        }
                    }
                }
            };
            var httpClient = new YawatHttpClient(options);

            var response = await httpClient.GetAsync("/test");

            var typedResponse = response.AsAnonymous(definition);

            Assert.AreEqual(typedResponse.test, "Test");
        }

        [Test]
        public async Task ShouldUseMockJsonDataForPost()
        {
            var definition = new { test = string.Empty };
            var jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(new { test = "Test" });

            var options = new Yawat.YawatOptions(typeof(JsonResponseData))
            {
                Host = "localhost",
                Protocol = "http",
                Port = "5000",
                MockHttpClient = new YawatMockHttpClient()
                {
                    EndpointHandlersDictionary = new Dictionary<string, YawatMockEndpoint>()
                    {
                        {
                            "/test",
                            new YawatMockEndpoint()
                            {
                                StatusCode = HttpStatusCode.OK,
                                Data = jsonText
                            }
                        }
                    }
                }
            };
            var httpClient = new YawatHttpClient(options);

            var jsonRequestData = new JsonRequestData()
            {
                Data = new { test = "Test" }
            };

            var response = await httpClient.PostAsync("/test", jsonRequestData);

            var typedResponse = response.AsAnonymous(definition);

            Assert.AreEqual(typedResponse.test, "Test");
        }
    }
}

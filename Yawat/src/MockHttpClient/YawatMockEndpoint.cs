namespace Yawat.MockHttpClient
{
    using System.Net;

    public class YawatMockEndpoint
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Data { get; set; }
    }
}

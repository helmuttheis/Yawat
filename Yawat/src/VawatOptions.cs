namespace Yawat
{
    using System;
    using System.Threading.Tasks;

    using Yawat.Interfaces;

    using Yawat.MockHttpClient;

    public class YawatOptions
    {
        public YawatOptions(Type responseType)
        {
            this.ResponseType = responseType;
        }

        public YawatOptions(Type requestType, Type responseType)
        {
            this.ResponseType = responseType;
            this.RequestType = requestType;
        }

        public string Host { get; set; }

        public string Protocol { get; set; }

        public string Port { get; set; }

        public bool IncludeHeaders { get; set; }

        public IAuthenticator Authenticator { get; set; }

        public string MediaType { get; set; } = "application/json";

        public Type ResponseType { get; set; }

        public Type RequestType { get; set; }

        public YawatMockHttpClient MockHttpClient { get; set; }

        public async Task<bool> Prepare()
        {
            var ret = true;
            if (this.Authenticator != null )
            {
                ret = await this.Authenticator.Setup();
            }

            return ret;
        }

        public string GetUrl(string endPoint)
        {
            if (!endPoint.StartsWith("/"))
            {
                endPoint = "/" + endPoint;
            }

            return $"{this.Protocol}://{this.Host}:{this.Port}{endPoint}";
        }

        public YawatOptions OverwrittenBy(YawatOptions options)
        {
            var newOption = new YawatOptions(null)
            {
                MediaType = options.MediaType ?? this.MediaType,
                ResponseType = options.ResponseType ?? this.ResponseType,
                Host = options.Host ?? this.Host,
                Protocol = options.Protocol ?? this.Protocol,
                Port = options.Port ?? this.Port,
                Authenticator = options.Authenticator ?? this.Authenticator,
                IncludeHeaders = this.IncludeHeaders || options.IncludeHeaders
            };

            return newOption;
        }
    }
}

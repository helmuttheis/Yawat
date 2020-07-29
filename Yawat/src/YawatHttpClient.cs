namespace Yawat
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Yawat.Interfaces;

    public class YawatHttpClient
    {
        private readonly System.Net.Http.HttpClient httpClient;

        private readonly YawatOptions yawatOptions;

        public YawatHttpClient(YawatOptions options)
        {
            this.yawatOptions = options;
            this.httpClient = new System.Net.Http.HttpClient();
        }

        public delegate int BeforeRequestAsyncHandler(string urlRequested, HttpClient httpClient);

        public BeforeRequestAsyncHandler BeforeRequestAsync { get; set; }

        public YawatResult Delete(string endPoint, YawatOptions options = null) => this.Request(HttpMethod.Delete, endPoint, string.Empty, options);

        public YawatResult Get(string endPoint, YawatOptions options = null) => this.Request(HttpMethod.Get, endPoint, string.Empty, options);

        public YawatResult Options(string endPoint, YawatOptions options = null) => this.Request(HttpMethod.Options, endPoint, string.Empty, options);

        public YawatResult Post(string endPoint, string jsonString, YawatOptions options = null) => this.Request(HttpMethod.Post, endPoint, jsonString, options);

        public YawatResult Put(string endPoint, string jsonString, YawatOptions options = null) => this.Request(HttpMethod.Put, endPoint, jsonString, options);

        public YawatResult Trace(string endPoint, YawatOptions options = null) => this.Request(HttpMethod.Put, endPoint, string.Empty, options);

        public async Task<YawatResult> DeleteAsync(string endPoint, YawatOptions options = null) => await this.RequestAsync(HttpMethod.Delete, endPoint, string.Empty, options);

        public async Task<YawatResult> GetAsync(string endPoint, YawatOptions options = null) => await this.RequestAsync(HttpMethod.Get, endPoint, string.Empty, options);

        public async Task<YawatResult> OptionsAsync(string endPoint, YawatOptions options = null) => await this.RequestAsync(HttpMethod.Options, endPoint, string.Empty, options);

        public async Task<YawatResult> PostAsync(string endPoint, string jsonString, YawatOptions options = null) => await this.RequestAsync(HttpMethod.Post, endPoint, jsonString, options);

        public async Task<YawatResult> PostAsync(string endPoint, IYawatRequestData requestData, YawatOptions options = null) => await this.RequestAsync(HttpMethod.Post, endPoint, requestData, options);

        public async Task<YawatResult> PutAsync(string endPoint, string jsonString, YawatOptions options = null) => await this.RequestAsync(HttpMethod.Put, endPoint, jsonString, options);

        public async Task<YawatResult> PutAsync(string endPoint, IYawatRequestData requestData, YawatOptions options = null) => await this.RequestAsync(HttpMethod.Put, endPoint, requestData, options);

        public async Task<YawatResult> TraceAsync(string endPoint, YawatOptions options = null) => await this.RequestAsync(HttpMethod.Put, endPoint, string.Empty, options);

        private YawatResult Request(HttpMethod verb, string endPoint, string jsonString = "",
            YawatOptions options = null)
        {
            YawatResult json = null;
            Task.Run(async () =>
            {
                json = await this.RequestAsync(verb, endPoint, jsonString, options);
            }).Wait();

            return json;
        }

        private async Task<YawatResult> RequestAsync(HttpMethod verb, string endPoint, IYawatRequestData requestData,
            YawatOptions options = null)
        {
            var byteArray = requestData?.Serialize();

            return await this.RequestAsync(verb, endPoint, byteArray, options);
        }

        private async Task<YawatResult> RequestAsync(HttpMethod verb, string endPoint, byte[] byteArray, YawatOptions options = null)
        {
            return await this.RequestAsync(verb, endPoint,
                System.Text.Encoding.UTF8.GetString(byteArray, 0, byteArray.Length), options);
        }

        private async Task<YawatResult> RequestAsync(HttpMethod verb, string endPoint, string jsonString = "", YawatOptions options = null)
        {
            var useOptions = this.yawatOptions;

            if (options != null)
            {
                useOptions = this.yawatOptions.OverwrittenBy(options);
            }

            var requestUri = this.PreprocessRequest(endPoint, useOptions);

            var request = new HttpRequestMessage(verb, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(useOptions.MediaType));

            if (verb == HttpMethod.Post || verb == HttpMethod.Put )
            {
                request.Content = new StringContent(jsonString, Encoding.UTF8, "text/json"); // useOptions.MediaType);
            }

            HttpResponseMessage response;
            if (useOptions.MockHttpClient != null && useOptions.MockHttpClient.EndpointHandlersDictionary.ContainsKey(endPoint))
            {
                var endpointHandler = useOptions.MockHttpClient.EndpointHandlersDictionary[endPoint];
                response = new HttpResponseMessage(endpointHandler.StatusCode)
                {
                    Content = new StringContent(endpointHandler.Data, System.Text.Encoding.UTF8,
                        useOptions.MediaType)
                };
            }
            else
            {
                try
                {
                    response = await this.httpClient.SendAsync(request);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }

            return await this.PostProcessResponse(response, useOptions);
        }

        private string PreprocessRequest(string endPoint, YawatOptions options)
        {
            var url = options.GetUrl(endPoint);

            options.Authenticator?.UpdateHttpClient(this.httpClient);

            this.BeforeRequestAsync?.Invoke(url, this.httpClient);

            return url;
        }

        private async Task<YawatResult> PostProcessResponse(HttpResponseMessage response, YawatOptions options)
        {
            var byteArray = Array.Empty<byte>();

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                byteArray = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            }

            var responseData = Activator.CreateInstance(options.ResponseType, byteArray) as IYawatResponseData;

            var result = new YawatResult(responseData)
            {
                StatusCode = response.StatusCode
            };

            if (options.IncludeHeaders)
            {
                foreach (var (key, value) in response.Headers)
                {
                    result.Headers.Add(key, value);
                }

                foreach (var (key, value) in response.Content.Headers)
                {
                    result.Headers.Add(key, value);
                }
            }

            return result;
        }
    }
}

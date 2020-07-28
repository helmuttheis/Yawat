namespace Yawat
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using Yawat.Interfaces;

    public class YawatResult
    {
        private readonly IYawatResponseData responseData;

        public YawatResult(IYawatResponseData responseData)
        {
            this.responseData = responseData;
            this.Headers = new Dictionary<string, IEnumerable<string>>();
        }

        public HttpStatusCode StatusCode { get; set; }

        public Dictionary<string, IEnumerable<string>> Headers { get; set; }

        public T As<T>()
        {
            try
            {
                return this.responseData.Deserialize<T>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default;
            }
        }

        public T AsAnonymous<T>(T anonymousTypeObject)
        {
            return this.responseData.DeserializeAsAnonymous(anonymousTypeObject);
        }

        public string AsString()
        {
            return this.responseData.Data;
        }

        public override string ToString()
        {
            return this.AsString();
        }
    }
}

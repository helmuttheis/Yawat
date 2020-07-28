namespace Yawat.ResponseData
{
    using Yawat.Interfaces;

    public class JsonResponseData : IYawatResponseData
    {
        public JsonResponseData(byte[] byteArray)
        {
            this.Data = System.Text.Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
        }

        public string Data { get; set; }

        public T Deserialize<T>()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(this.Data);
        }

        public T DeserializeAsAnonymous<T>(T anonymousTypeObject)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(this.Data, anonymousTypeObject);
        }
    }
}

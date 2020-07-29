namespace Yawat.RequestData
{
    using Yawat.Interfaces;

    public class JsonRequestData : IYawatRequestData
    {
        public string MediaType { get;  } = "text/json";

        public object Data { get; set; }

        public byte[] Serialize()
        {
            return System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(this.Data));
        }
    }
}

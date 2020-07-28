namespace Yawat.RequestData
{
    using Yawat.Interfaces;

    public class JsonRequestData : IYawatRequestData
    {
        public object Data { get; set; }

        public string MediaType { get; set; }

        public string Serialize()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this.Data);
        }
    }
}

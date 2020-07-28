namespace Yawat.RequestData
{
    using Yawat.Interfaces;

    public class StringRequestData : IYawatRequestData
    {
        public object Data { get; set; }

        public string MediaType { get; set; }

        public string Serialize()
        {
            return this.Data.ToString();
        }
    }
}
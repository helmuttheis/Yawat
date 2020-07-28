namespace Yawat.RequestData
{
    using Yawat.Interfaces;

    public class StringRequestData : IYawatRequestData
    {
        public object Data { get; set; }

        public string MediaType { get; set; }

        public byte[] Serialize()
        {
            return System.Text.Encoding.UTF8.GetBytes(this.Data.ToString());
        }
    }
}
namespace Yawat.Interfaces
{
    public interface IYawatRequestData
    {
        object Data { get; set; }

        string MediaType { get; set; }

        string Serialize();
    }
}

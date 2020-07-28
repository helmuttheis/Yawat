namespace Yawat.Interfaces
{
    public interface IYawatResponseData
    {
        string Data { get; set; }

        T Deserialize<T>();

        T DeserializeAsAnonymous<T>(T anonymousTypeObject);
    }
}

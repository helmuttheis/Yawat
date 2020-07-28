using ProtoBuf;

namespace YawatServer.Models
{
    [ProtoContract]
    public class TodoItemDto
    {
        [ProtoMember(1)]
        public long Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public bool IsComplete { get; set; }
    }
}

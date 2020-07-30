// ReSharper disable InconsistentNaming
namespace Yawat.Models
{
    public class VirtualDataResult
    {
        public long totalCount { get; set; }

        public long groupSize { get; set; }

        public long queryCount { get; set; }

        public long count { get; set; }

        public Entry[] entries { get; set; }
    }
}

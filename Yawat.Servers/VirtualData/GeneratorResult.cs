namespace VirtualData
{
    using System.Collections.Generic;

    public class GeneratorResult
    {
        public int TotalCount { get; set; }

        public int GroupSize { get; set; }

        public int QueryCount { get; set; }

        public int Count { get; set; }

        public List<Entry> Entries { get; set; } = new List<Entry>();
    }
}

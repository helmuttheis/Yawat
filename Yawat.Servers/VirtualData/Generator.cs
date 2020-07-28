namespace VirtualData
{
    using System;
    using System.Collections.Generic;

    public class Generator
    {
        private readonly int totalCount;

        private readonly int groupSize;

        private readonly int size;

        private readonly GeneratorRange generatorRange;

        public Generator(int totalCount, int groupSize = -1)
        {
            this.generatorRange = new GeneratorRange
            {
                Min = 0,
                Count = totalCount
            };

            this.totalCount = totalCount;
            this.groupSize = groupSize;
            this.size = totalCount.ToString().Length;
        }

        public string Order { get; set; } = string.Empty;

        public string Dir { get; set; } = string.Empty;

        public void Reset()
        {
            this.generatorRange.Min = 0;
            this.generatorRange.Count = this.totalCount;
            this.Order = string.Empty;
            this.Dir = string.Empty;
        }

        public GeneratorRange NameStartsWith(string pattern)
        {
            var gRange = this.GetIdRange(pattern, this.generatorRange);

            this.generatorRange.Intersect(gRange);

            return this.generatorRange;
        }

        public GeneratorRange GroupStartsWith(string pattern)
        {
            var gRange = new GeneratorRange
            {
                Min = 0,
                Count = this.totalCount
            };
            gRange = this.GetIdRange(pattern, gRange);

            gRange.Min *= this.groupSize;
            gRange.Count *= this.groupSize;

            this.generatorRange.Intersect(gRange);

            return this.generatorRange;
        }

        public string Get(int first, int take)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this.GetAsObject(first, take));
        }

        public GeneratorResult GetAsObject(int first = 0, int take = -1)
        {
            List<Entry> entries = new List<Entry>();

            if (first < 0)
            {
                first = 0;
            }

            var last = this.generatorRange.Count;

            if (take > 0 && take + first <= this.generatorRange.Count)
            {
                last = take + first;
            }

            first += this.generatorRange.Min;
            last += this.generatorRange.Min;
            var step = 1;

            if ( (this.Order.Equals("name", StringComparison.InvariantCultureIgnoreCase) && this.Dir.StartsWith("d", StringComparison.InvariantCultureIgnoreCase))
              || (this.Order.Equals("reverted", StringComparison.InvariantCultureIgnoreCase) && this.Dir.StartsWith("a", StringComparison.InvariantCultureIgnoreCase)) )
            {
                first = this.generatorRange.Count - first - 1;
                last = this.generatorRange.Count - last - 1;
                step = -1;
            }

            if (step > 0)
            {
                for (var i = first; i < last; i++)
                {
                    entries.Add(new Entry(i, this.totalCount, this.groupSize));
                }
            }
            else
            {
                for (var i = first; i > last; i--)
                {
                    entries.Add(new Entry(i, this.totalCount, this.groupSize));
                }

            }

            return new GeneratorResult()
            {
                TotalCount = this.totalCount,
                GroupSize = this.groupSize,
                QueryCount = this.generatorRange.Count,
                Count = entries.Count,
                Entries = entries
            };
        }

        private GeneratorRange GetIdRange(string pattern, GeneratorRange gRange)
        {
            var lowerPattern = pattern.Substring(1).PadRight(this.size, '0');
            var upperPattern = pattern.Substring(1).PadRight(this.size, '9');

            if (int.TryParse(lowerPattern, out var minVal))
            {
                gRange.Min = Math.Max(gRange.Min, minVal);
            }

            if (int.TryParse(upperPattern, out var maxVal))
            {
                gRange.Count = Math.Min(gRange.Count, maxVal - gRange.Min + 1);
            }

            return gRange;
        }
    }
}

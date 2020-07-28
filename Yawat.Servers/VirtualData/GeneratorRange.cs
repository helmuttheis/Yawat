namespace VirtualData
{
    using System;

    public class GeneratorRange
    {
        public int Min { get; set; }

        public int Count { get; set; }

        public int Max => this.Min + this.Count - 1;

        public GeneratorRange Intersect(GeneratorRange generatorRange)
        {
            if (this.Min > generatorRange.Max)
            {
                this.Count = 0;
            }
            else if (this.Max < generatorRange.Min)
            {
                this.Count = 0;
            }
            else
            {
                var max = Math.Min(this.Max, generatorRange.Max);
                this.Min = Math.Max(this.Min, generatorRange.Min);
                this.Count = max - this.Min + 1;
            }

            return this;
        }
    }
}

namespace VirtualData
{
    public class Entry
    {
        public Entry(int number, int totalCount, int groupSize = -1, int size = -1)
        {
            if (groupSize <= 0)
            {
                groupSize = totalCount / 10;
            }

            var groupNumber = number / groupSize;

            if (size <= 0)
            {
                size = totalCount.ToString().Length;
            }

            this.Id = number;
            this.Name = "N" + this.GetString(number, size);
            this.Reverted = "R" + this.GetString(totalCount - number, size);

            this.Group = "G" + this.GetString(groupNumber, size);
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Reverted { get; private set; }

        public string Group { get; private set; }

        private string GetString(int val, int size)
        {
            return val.ToString("D" + size);
        }
    }
}

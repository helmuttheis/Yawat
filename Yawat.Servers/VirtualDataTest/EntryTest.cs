namespace VirtualDataTest
{
    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VirtualData;

    [TestClass]
    public class EntryTest
    {
        [TestMethod]
        public void ShouldCalculateGroupSize1()
        {
            Entry entry = new Entry(0, 10);
            entry.Id.Should().Be(0);
            entry.Name.Should().Be("N00");
            entry.Reverted.Should().Be("R10");
            entry.Group.Should().Be("G00");
        }

        [TestMethod]
        public void ShouldCalculateGroupSize10()
        {
            Entry entry = new Entry(15, 100);
            entry.Id.Should().Be(15);
            entry.Name.Should().Be("N015");
            entry.Reverted.Should().Be("R085");
            entry.Group.Should().Be("G001");
        }

        [TestMethod]
        public void ShouldUseGroupSize5()
        {
            Entry entry = new Entry(15, 100, 5);
            entry.Id.Should().Be(15);
            entry.Name.Should().Be("N015");
            entry.Reverted.Should().Be("R085");
            entry.Group.Should().Be("G003");
        }
    }
}

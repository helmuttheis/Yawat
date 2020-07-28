namespace VirtualDataTest
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VirtualData;

    [TestClass]
    public class GeneratorTest
    {
        [TestMethod]
        public void ShouldCreate10()
        {
            var generator = new Generator(10);

            var result = generator.GetAsObject(2, 4);

            result.TotalCount.Should().Be(10);
            result.Entries.Count.Should().Be(4);

            var entry = result.Entries[0];

            entry.Id.Should().Be(2);
            entry.Name.Should().Be("N02");
            entry.Reverted.Should().Be("R08");
            entry.Group.Should().Be("G02");
        }

        [TestMethod]
        public void ShouldCreateMio()
        {
            var generator = new Generator(1000000, 100);

            var result = generator.GetAsObject(199, 50);

            result.TotalCount.Should().Be(1000000);
            result.Entries.Count.Should().Be(50);

            var entry = result.Entries[0];

            entry.Id.Should().Be(199);
            entry.Name.Should().Be("N0000199");

            entry.Reverted.Should().Be("R0999801");
            entry.Group.Should().Be("G0000001");

            entry = result.Entries[2];

            entry.Id.Should().Be(201);
            entry.Name.Should().Be("N0000201");

            entry.Reverted.Should().Be("R0999799");
            entry.Group.Should().Be("G0000002");
        }

        [TestMethod]
        public void ShouldCreateThousand()
        {
            var generator = new Generator(1000, 20);

            var result = generator.GetAsObject(199, 50);

            result.TotalCount.Should().Be(1000);
            result.QueryCount.Should().Be(1000);
            result.Entries.Count.Should().Be(50);

            var entry = result.Entries[0];

            entry.Id.Should().Be(199);
            entry.Name.Should().Be("N0199");

            entry.Reverted.Should().Be("R0801");
            entry.Group.Should().Be("G0009");

            entry = result.Entries[2];

            entry.Id.Should().Be(201);
            entry.Name.Should().Be("N0201");

            entry.Reverted.Should().Be("R0799");
            entry.Group.Should().Be("G0010");
        }

        [TestMethod]
        public void ShouldOrderByReverted()
        {
            var generator = new Generator(1000, 20)
            {
                Order = "reverted",
                Dir = "asc"
            };

            var result = generator.GetAsObject(0, 50);

            result.TotalCount.Should().Be(1000);
            result.QueryCount.Should().Be(1000);
            result.Entries.Count.Should().Be(50);

            var entry = result.Entries[0];

            entry.Id.Should().Be(999);
            entry.Name.Should().Be("N0999");

            entry.Reverted.Should().Be("R0001");
            entry.Group.Should().Be("G0049");
        }

        [TestMethod]
        public void ShouldOrderByRevertedWithFirst()
        {
            var generator = new Generator(1000, 20)
            {
                Order = "reverted",
                Dir = "asc"
            };

            var result = generator.GetAsObject(100, 50);

            result.QueryCount.Should().Be(1000);
            result.Entries.Count.Should().Be(50);

            var entry = result.Entries[0];

            entry.Id.Should().Be(899);
            entry.Name.Should().Be("N0899");

            entry.Reverted.Should().Be("R0101");
            entry.Group.Should().Be("G0044");
        }

        [TestMethod]
        public void ShouldCalculateMinByName()
        {
            var generator = new Generator(1000, 100);

            var generatorRange = generator.NameStartsWith("N09");

            generatorRange.Min.Should().Be(900);
            generatorRange.Count.Should().Be(100);
            generatorRange.Max.Should().Be(999);

            var result = generator.GetAsObject(0, -1);

            result.TotalCount.Should().Be(1000);
            result.QueryCount.Should().Be(100);
            result.Entries.Count.Should().Be(100);
        }

        [TestMethod]
        public void ShouldCalculateMinByGroup()
        {
            var generator = new Generator(1000, 20);

            var generatorRange = generator.GroupStartsWith("G002");

            generatorRange.Min.Should().Be(400);
            generatorRange.Count.Should().Be(200);
            generatorRange.Max.Should().Be(599);
        }

        [TestMethod]
        public void ShouldCalculateMinWithGroup()
        {
            var generator = new Generator(1000, 20);

            var generatorRange = generator.NameStartsWith("N04");

            generatorRange.Min.Should().Be(400);
            generatorRange.Count.Should().Be(100);
            generatorRange.Max.Should().Be(499);

            generatorRange = generator.GroupStartsWith("G002");

            generatorRange.Min.Should().Be(400);
            generatorRange.Count.Should().Be(100);

            var result = generator.GetAsObject(0, -1);

            result.QueryCount.Should().Be(100);
            result.Entries.Count.Should().Be(100);
        }
    }
}

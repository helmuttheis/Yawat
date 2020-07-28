namespace VirtualDataTest
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VirtualData;

    [TestClass]
    public class GeneratorRangeTest
    {
        [TestMethod]
        public void ShouldCreate()
        {
            var gRange3010 = new GeneratorRange() { Min = 30, Count = 10 };
            gRange3010.Min.Should().Be(30);
            gRange3010.Max.Should().Be(39);
            gRange3010.Count.Should().Be(10);
        }

        [TestMethod]
        public void ShouldIntersect()
        {
            var gRange1005 = new GeneratorRange() { Min = 10, Count = 5 };
            var gRange2020 = new GeneratorRange() { Min = 20, Count = 20 };
            var gRange3910 = new GeneratorRange() { Min = 39, Count = 10 };
            var gRange5010 = new GeneratorRange() { Min = 50, Count = 10 };

            var gRange3010 = new GeneratorRange() { Min = 30, Count = 10 };

            gRange3010.Intersect(gRange2020);
            gRange3010.Count.Should().Be(10);

            gRange3010 = new GeneratorRange() { Min = 30, Count = 10 };
            gRange3010.Intersect(gRange1005);
            gRange3010.Count.Should().Be(0);

            gRange3010 = new GeneratorRange() { Min = 30, Count = 10 };
            gRange3010.Intersect(gRange3910);
            gRange3010.Count.Should().Be(1);

            gRange3010 = new GeneratorRange() { Min = 30, Count = 10 };
            gRange3010.Intersect(gRange5010);
            gRange3010.Count.Should().Be(0);
        }
    }
}

using Yawat.Models;

namespace Unrestricted
{
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class Sample1
    {
        [Test]
        public void ShouldGetJsonFromGet()
        {
            var json = SetupTeardown.HttpClientWithOptions.Get("/weatherforecast").AsString();

            Assert.AreNotEqual(json, string.Empty);
        }

        [Test]
        public void ShouldGetObjectFromGet()
        {
            var foreCastList = SetupTeardown.HttpClientWithOptions.Get("/weatherforecast").As<List<ForeCast>>();

            Assert.AreNotEqual(foreCastList.Count, 0);
        }
    }
}

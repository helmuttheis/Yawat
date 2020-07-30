namespace Unrestricted
{
    using System.Threading.Tasks;
    using Yawat.Models;
    using NUnit.Framework;

    [TestFixture]
    public class VirtualData
    {
        private const string BaseRoute = "/vdata";

        [Test]
        public async Task ShouldGetJson()
        {
            var result = await SetupTeardown.HttpClientWithOptions.GetAsync($"{BaseRoute}?first=20&take=30&group=G0000001");

            var json = result.AsString();

            Assert.AreNotEqual(json, string.Empty);
        }

        [Test]
        public async Task ShouldGetObject()
        {
            var result = await SetupTeardown.HttpClientWithOptions.GetAsync($"{BaseRoute}?first=20&take=30");

            var vData = result.As<VirtualDataResult>();

            Assert.AreEqual(vData.count, 30);

            Assert.AreEqual(vData.entries[0].Id, 20);
        }

        [Test]
        public async Task ShouldGetGroup1Object()
        {
            var result = await SetupTeardown.HttpClientWithOptions.GetAsync($"{BaseRoute}?first=20&take=30&group=G0000001");

            var vData = result.As<VirtualDataResult>();

            Assert.AreEqual(vData.count, 30);

            Assert.AreEqual(vData.entries[0].Id, 520);
        }
    }
}

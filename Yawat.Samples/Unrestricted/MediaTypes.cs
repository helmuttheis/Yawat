namespace Unrestricted
{
    using System.Threading.Tasks;
    using System.Xml;

    using NUnit.Framework;
    using Yawat;

    [TestFixture]
    public class MediaTypes
    {
        private const string BaseRoute = "/api/TodoItems";

        [Test]
        public async Task ShouldGetXmlFromGetAllAsync()
        {
            var options = new YawatOptions(null)
            {
                MediaType = "application/xml"
            };
            var xml = (await SetupTeardown.HttpClientWithOptions.GetAsync($"{BaseRoute}", options)).AsString();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            var nodeList = doc.SelectNodes("//TodoItemDto");

            Assert.AreEqual(nodeList.Count, 1);
        }

        [Test]
        public async Task ShouldGetPlainTextFromGetAllAsync()
        {
            var options = new YawatOptions(null)
            {
                MediaType = "text/plain"
            };
            var txt = (await SetupTeardown.HttpClientWithOptions.GetAsync($"{BaseRoute}", options)).AsString();

            Assert.Greater(txt.Length, 1);
        }
    }
}

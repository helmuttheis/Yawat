namespace Yawat.Cli
{
    using System.Threading.Tasks;

    using NUnit.Framework;

    using Yawat.Cli.Data;

    using Yawat.RequestData;
    using Yawat.ResponseData;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var config = new Config();

            Task.Run(async() =>
            {
                await DebugJsonRequestData(config);

                // ToDo: await DebugXmlRequestData(config);
            }).Wait();
        }

        private static async Task DebugJsonRequestData(Config config)
        {
            var jsonData = new GenericData(
                config,
                new JsonRequestData());

            await DebugXxxRequestData(jsonData);
        }

        private static async Task DebugXmlRequestData(Config config)
        {
            var xmlData = new GenericData(
                config,
                new JsonRequestData(),
                new YawatOptions(typeof(XmlResponseData))
                {
                    MediaType = "application/xml"
                });

            await DebugXxxRequestData(xmlData);
        }

        private static async Task DebugXxxRequestData(IGenericData genericData)
        {
            var cntStart = await genericData.Count();

            await genericData.Insert();
            var cntInserted = await genericData.Count();

            await genericData.Update();

            await genericData.Delete();
            var cnt = await genericData.Count();

            Assert.AreEqual(cntInserted - 1, cntStart);
            Assert.AreEqual(cnt, cntStart);
        }
    }
}

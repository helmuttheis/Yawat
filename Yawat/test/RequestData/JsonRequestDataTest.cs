namespace Yawat.Tests.RequestData
{
    using NUnit.Framework;
    using Yawat.RequestData;

    [TestFixture]
    public class JsonRequestDataTest
    {
        [Test]
        public void ShouldSerializeJsonRequestData()
        {
            var anonymousObject = new { test = "test" };
            var expectedJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(anonymousObject);
            var requestData = new JsonRequestData()
            {
                Data = anonymousObject
            };

            var byteArray = requestData.Serialize();
            var jsonString = System.Text.Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

            Assert.AreEqual(expectedJsonString, jsonString);
        }
    }
}

namespace Yawat.ResponseData
{
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using Yawat.Interfaces;

    public class XmlResponseData : IYawatResponseData
    {
        public XmlResponseData(byte[] byteArray)
        {
            this.Data = System.Text.Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
        }

        public string Data { get; set; }

        public T Deserialize<T>()
        {
            var serializer = new XmlSerializer(typeof(T));

            using TextReader reader = new StringReader(this.Data);
            var result = serializer.Deserialize(reader);

            return (T)result;
        }

        public T DeserializeAsAnonymous<T>(T anonymousTypeObject)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(this.Xml2Json(), anonymousTypeObject);
        }

        private string Xml2Json()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(this.Data);

            return Newtonsoft.Json.JsonConvert.SerializeXmlNode(xmlDoc);
        }
    }
}

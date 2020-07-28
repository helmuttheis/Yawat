namespace Unrestricted
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Xml;
    using Models;

    using NUnit.Framework;

    using Yawat;
    using Yawat.ResponseData;

    [TestFixture]
    public class ToDoRequests
    {
        private const string BaseRoute = "/api/TodoItems";

        [Test]
        public void ShouldGetJsonFromGetAll()
        {
            var json = SetupTeardown.HttpClientWithOptions.Get($"{BaseRoute}").AsString();

            Assert.AreNotEqual(json, string.Empty);
        }

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

            Assert.AreEqual(nodeList.Count, 2);
        }

        [Test]
        public async Task ShouldGetObjectXmlFromGetAllAsync()
        {
            var options = new YawatOptions(typeof(XmlResponseData))
            {
                MediaType = "application/xml"
            };
            var toDoList = (await SetupTeardown.HttpClientWithOptions.GetAsync($"{BaseRoute}", options)).As<ArrayOfTodoItemDto>();

            Assert.AreNotEqual(toDoList.TodoItemDto.Length, 0);
        }

        [Test]
        public void ShouldGetObjectFromGetAll()
        {
            var toDoList = SetupTeardown.HttpClientWithOptions.Get($"{BaseRoute}").As<List<ToDoItem>>();

            Assert.AreNotEqual(toDoList.Count, 0);
        }

        [Test]
        public void ShouldGetObjectFromGet()
        {
            var toDo = SetupTeardown.HttpClientWithOptions.Get($"{BaseRoute}/1").As<ToDoItem>();

            Assert.AreEqual(toDo.Id, 1);
        }
    }
}

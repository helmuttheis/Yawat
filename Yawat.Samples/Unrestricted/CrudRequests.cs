using Yawat.RequestData;

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
    public class CrudRequests
    {
        private const string BaseRoute = "/api/TodoItems";

        [Test]
        public void ShouldReadObject()
        {
            var toDo = SetupTeardown.HttpClientWithOptions.Get($"{BaseRoute}/1").As<ToDoItem>();

            Assert.AreEqual(toDo.Id, 1);
        }

        [Test]
        public async Task ShouldCreateObject()
        {
            var newToDoItem = new ToDoItem()
            {
                Name = "new item",
                IsComplete = false
            };
            var requestData = new JsonRequestData()
            {
                Data = newToDoItem
            };

            var toDo = (await SetupTeardown.HttpClientWithOptions.PostAsync($"{BaseRoute}", requestData)).As<ToDoItem>();

            Assert.AreEqual(toDo.Name, newToDoItem.Name);
        }

    }
}

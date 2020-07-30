using System.Collections.Generic;
using Yawat.RequestData;

namespace Unrestricted
{
    using System.Threading.Tasks;

    using NUnit.Framework;
    using Yawat.Models;

    [TestFixture]
    public class CrudRequests
    {
        private const string BaseRoute = "/api/TodoItems";

        [Test]
        public void ShouldReadObject()
        {
            var toDoList = SetupTeardown.HttpClientWithOptions.Get($"{BaseRoute}").As<List<TodoItem>>();

            Assert.AreNotEqual(toDoList.Count, 0);

            var id = toDoList[0].Id;
            var toDo = SetupTeardown.HttpClientWithOptions.Get($"{BaseRoute}/{id}").As<TodoItem>();

            Assert.AreEqual(toDo.Id, id);
        }

        [Test]
        public async Task ShouldCreateUpdateDeleteObject()
        {
            var newToDoItem = new TodoItem()
            {
                Name = "new item",
                IsComplete = false
            };
            var requestData = new JsonRequestData()
            {
                Data = newToDoItem
            };

            var toDoCreated = (await SetupTeardown.HttpClientWithOptions.PostAsync($"{BaseRoute}", requestData)).As<TodoItem>();

            Assert.AreEqual(toDoCreated.Name, newToDoItem.Name);


            newToDoItem.Name = "Updated name";
            newToDoItem.Id = toDoCreated.Id;

            var result = (await SetupTeardown.HttpClientWithOptions.PutAsync($"{BaseRoute}/{toDoCreated.Id}", requestData));
            Assert.AreEqual(result.StatusCode.ToString(), "NoContent");

            var toDo = (await SetupTeardown.HttpClientWithOptions.GetAsync($"{BaseRoute}/{toDoCreated.Id}")).As<TodoItem>();

            Assert.AreEqual(toDo.Name, newToDoItem.Name);
        }
    }
}

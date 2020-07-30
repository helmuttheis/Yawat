using System.Collections.Generic;
using Yawat.RequestData;

namespace Unrestricted
{
    using System.Threading.Tasks;
    using Models;

    using NUnit.Framework;

    [TestFixture]
    public class CrudRequests
    {
        private const string BaseRoute = "/api/TodoItems";

        [Test]
        public void ShouldReadObject()
        {
            var toDoList = SetupTeardown.HttpClientWithOptions.Get($"{BaseRoute}").As<List<ToDoItem>>();

            Assert.AreNotEqual(toDoList.Count, 0);

            var id = toDoList[0].Id;
            var toDo = SetupTeardown.HttpClientWithOptions.Get($"{BaseRoute}/{id}").As<ToDoItem>();

            Assert.AreEqual(toDo.Id, id);
        }

        [Test]
        public async Task ShouldCreateUpdateDeleteObject()
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

            var toDoCreated = (await SetupTeardown.HttpClientWithOptions.PostAsync($"{BaseRoute}", requestData)).As<ToDoItem>();

            Assert.AreEqual(toDoCreated.Name, newToDoItem.Name);


            newToDoItem.Name = "Updated name";
            newToDoItem.Id = toDoCreated.Id;

            var result = (await SetupTeardown.HttpClientWithOptions.PutAsync($"{BaseRoute}/{toDoCreated.Id}", requestData));

            var toDo = (await SetupTeardown.HttpClientWithOptions.GetAsync($"{BaseRoute}/{toDoCreated.Id}")).As<ToDoItem>();

            Assert.AreEqual(toDo.Name, newToDoItem.Name);
        }
    }
}

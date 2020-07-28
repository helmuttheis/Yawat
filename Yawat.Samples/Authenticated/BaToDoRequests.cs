namespace Authenticated
{
    using System.Collections.Generic;
    using Models;
    using NUnit.Framework;

    using Yawat;
    using Yawat.ResponseData;

    [TestFixture]
    public class BaToDoRequests
    {
        private string baseRoute;
        private string userName;
        private string password;

        private YawatOptions options;

        [SetUp]
        public void Setup()
        {
            this.baseRoute = YawatSettings.Get("BasicAuthentication.BaseRoute");
            this.userName = YawatSettings.Get("BasicAuthentication.UserName");
            this.password = YawatSettings.Get("BasicAuthentication.Password");
            this.options = new YawatOptions(typeof(JsonResponseData))
            {
                Authenticator = new Yawat.Authenticator.BasicAuthenticator(this.userName, this.password),
            };
        }

        [Test]
        public void ShouldGetJsonFromGetAll()
        {
            var json = SetupTeardown.HttpClientWithOptions.Get($"{this.baseRoute}", this.options).AsString();

            Assert.AreNotEqual(json, string.Empty);
        }

        [Test]
        public void ShouldGetObjectFromGetAll()
        {
            var toDoList = SetupTeardown.HttpClientWithOptions.Get($"{this.baseRoute}", this.options).As<List<ToDoItem>>();

            Assert.AreNotEqual(toDoList.Count, 0);
        }

        [Test]
        public void ShouldGetObjectFromGet()
        {
            var toDo = SetupTeardown.HttpClientWithOptions.Get($"{this.baseRoute}/1", this.options).As<ToDoItem>();

            Assert.AreEqual(toDo.Id, 1);
        }
    }
}

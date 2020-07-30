namespace Authenticated
{
    using System.Collections.Generic;
    using NUnit.Framework;

    using Yawat;
    using Yawat.Models;
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
        public void ShouldGetFirstItemFromGetAll()
        {
            var toDoList = SetupTeardown.HttpClientWithOptions.Get($"{this.baseRoute}", this.options).As<List<TodoItem>>();

            Assert.AreNotEqual(toDoList.Count, 0);

            var id = toDoList[0].Id;

            var toDo = SetupTeardown.HttpClientWithOptions.Get($"{this.baseRoute}/{id}", this.options).As<TodoItem>();

            Assert.AreEqual(toDo.Id, id);
        }
    }
}

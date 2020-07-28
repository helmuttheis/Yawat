using System.Threading.Tasks;
using Yawat.RequestData;
using Yawat.ResponseData;

namespace Yawat.Cli
{
    using System;

    public static class Program
    {
        private const string BaseRoute = "/api/TodoItems";

        private static Yawat.YawatOptions options;

        private static Yawat.YawatHttpClient httpClientWithOptions;


        static void Main(string[] args)
        {
            options = new Yawat.YawatOptions(typeof(JsonResponseData))
            {
                Host = YawatSettings.Get("Host", "localhost"),
                Protocol = YawatSettings.Get("Protocol", "https"),
                Port = YawatSettings.Get("Port", "44328")
            };

            httpClientWithOptions = new YawatHttpClient(options);

            var newToDoItem = new ToDoItem()
            {
                Name = "new item",
                IsComplete = false
            };
            var requestData = new JsonRequestData()
            {
                Data = newToDoItem
            };

            long createdId = -1;
            Task.Run(async () =>
            {
                var result = await httpClientWithOptions.PostAsync($"{BaseRoute}", requestData);

                var createdToDoItem = result.As<ToDoItem>();
                if (createdToDoItem != null)
                {
                    createdId = createdToDoItem.Id;
                }
            }).Wait();

            newToDoItem.Id = createdId;
            newToDoItem.Name = "Updated name";
            requestData.Data = newToDoItem;
            Task.Run(async () =>
            {
                var result = await httpClientWithOptions.PutAsync($"{BaseRoute}/{createdId}", requestData);

                var createdToDoItem = result.As<ToDoItem>();
                if (createdToDoItem != null)
                {
                    createdId = createdToDoItem.Id;
                }
            }).Wait();

            var updatedToDoItem = httpClientWithOptions.Get($"{BaseRoute}/{createdId}").As<ToDoItem>();

            if (!updatedToDoItem.Name.Equals(newToDoItem.Name))
            {
                throw new Exception("");
            }
        }
    }
}

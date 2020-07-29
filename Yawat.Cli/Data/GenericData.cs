using Yawat.ResponseData;

namespace Yawat.Cli.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;

    using Yawat.Interfaces;

    public class GenericData : IGenericData
    {
        private readonly ToDoItem newToDoItem;
        private readonly IYawatRequestData requestData;
        private readonly Config config;
        private readonly YawatOptions options = null;

        private long createdId = -1;
        private ToDoItem updatedToDoItem;

        public GenericData(Config config, IYawatRequestData requestData, YawatOptions options = null)
        {
            this.config = config;
            this.options = options;
            this.newToDoItem = new ToDoItem()
            {
                Name = "new item",
                IsComplete = false
            };
            this.requestData = requestData;
            this.requestData.Data = this.newToDoItem;
        }

        public async Task<int> Count()
        {
            var result = await this.config.HttpClientWithOptions.GetAsync($"{Config.BaseRoute}", this.options);

            if ( this.options != null && this.options.ResponseType != null && this.options.ResponseType == typeof(XmlResponseData) )
            {
                return result.As<ArrayOfTodoItemDto>().TodoItemDto.Length;
            }

            return result.As<List<ToDoItem>>().Count;
        }

        public async Task<long> Insert()
        {
            this.createdId = -1;
            var result = await this.config.HttpClientWithOptions.PostAsync($"{Config.BaseRoute}", this.requestData, this.options);

            if (this.options != null && this.options.ResponseType != null && this.options.ResponseType == typeof(XmlResponseData))
            {
                var createdToDoItemXml = result.As<ArrayOfTodoItemDtoTodoItemDto>();
                if (createdToDoItemXml != null)
                {
                    this.createdId = createdToDoItemXml.Id;
                }
            }

            var createdToDoItem = result.As<ToDoItem>();
            if (createdToDoItem != null)
            {
                this.createdId = createdToDoItem.Id;
            }

            return this.createdId;
        }

        public async Task Delete()
        {
            var result = await this.config.HttpClientWithOptions.DeleteAsync($"{Config.BaseRoute}/{this.createdId}", this.options);

            var deletedToDoItem = result.As<ToDoItem>();
            if (deletedToDoItem == null)
            {
                Console.WriteLine("ERROR: delete");
            }
        }

        public async Task Update()
        {
            this.newToDoItem.Id = this.createdId;
            this.newToDoItem.Name = "Updated name";

            var result = await this.config.HttpClientWithOptions.PutAsync($"{Config.BaseRoute}/{this.createdId}", this.requestData, this.options);

            var createdToDoItem = result.As<ToDoItem>();
            if (createdToDoItem != null)
            {
                this.createdId = createdToDoItem.Id;
            }

            this.updatedToDoItem = (await this.config.HttpClientWithOptions.GetAsync($"{Config.BaseRoute}/{this.createdId}", this.options)).As<ToDoItem>();

            if (!this.updatedToDoItem.Name.Equals(this.newToDoItem.Name))
            {
                throw new Exception("Name was not updated");
            }
        }
    }
}

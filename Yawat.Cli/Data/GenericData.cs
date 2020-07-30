namespace Yawat.Cli.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Yawat.Interfaces;
    using Yawat.Models;
    using Yawat.ResponseData;

    public class GenericData : IGenericData
    {
        private readonly TodoItem newTodoItem;
        private readonly IYawatRequestData requestData;
        private readonly Config config;
        private readonly YawatOptions options;

        private long createdId = -1;
        private TodoItem updatedToDoItem;

        public GenericData(Config config, IYawatRequestData requestData, YawatOptions options = null)
        {
            this.config = config;
            this.options = options;
            this.newTodoItem = new TodoItem()
            {
                Name = "new item",
                IsComplete = false
            };
            this.requestData = requestData;
            this.requestData.Data = this.newTodoItem;
        }

        public async Task<int> Count()
        {
            var result = await this.config.HttpClientWithOptions.GetAsync($"{Config.BaseRoute}", this.options);

            if ( this.options != null && this.options.ResponseType != null && this.options.ResponseType == typeof(XmlResponseData) )
            {
                return result.As<ArrayOfTodoItemDto>().TodoItemDto.Length;
            }

            return result.As<List<TodoItem>>().Count;
        }

        public async Task<long> Insert()
        {
            this.createdId = -1;
            var result = await this.config.HttpClientWithOptions.PostAsync($"{Config.BaseRoute}", this.requestData, this.options);

            if (this.IsXmlResponse() )
            {
                var createdToDoItemXml = result.As<TodoItemDto>();
                if (createdToDoItemXml != null)
                {
                    this.createdId = createdToDoItemXml.Id;
                }
            }
            else
            {
                var createdToDoItem = result.As<TodoItem>();
                if (createdToDoItem != null)
                {
                    this.createdId = createdToDoItem.Id;
                }
            }

            return this.createdId;
        }

        public async Task<long> Delete()
        {
            long deletedId = -1;
            var result = await this.config.HttpClientWithOptions.DeleteAsync($"{Config.BaseRoute}/{this.createdId}", this.options);

            if (this.IsXmlResponse() )
            {
                var deletedToDoItem = result.As<TodoItem>();
                if (deletedToDoItem != null)
                {
                    deletedId = deletedToDoItem.Id;
                }
                else
                {
                    Console.WriteLine("ERROR: delete");
                }
            }
            else
            {
                var deletedToDoItem = result.As<TodoItem>();
                if (deletedToDoItem != null)
                {
                    deletedId = deletedToDoItem.Id;
                }
                else
                {
                    Console.WriteLine("ERROR: delete");
                }
            }

            return deletedId;
        }

        public async Task Update()
        {
            this.newTodoItem.Id = this.createdId;
            this.newTodoItem.Name = "Updated name";

            var result = await this.config.HttpClientWithOptions.PutAsync($"{Config.BaseRoute}/{this.createdId}", this.requestData, this.options);

            var resultUpdated = await this.config.HttpClientWithOptions.GetAsync(
                $"{Config.BaseRoute}/{this.createdId}",
                this.options);

            if (this.IsXmlResponse())
            {
                var updatedToDoItemXml = resultUpdated.As<TodoItemDto>();

                if (!updatedToDoItemXml.Name.Equals(this.newTodoItem.Name))
                {
                    throw new Exception("Name was not updated");
                }
            }
            else
            {
                this.updatedToDoItem = resultUpdated.As<TodoItem>();

                if (!this.updatedToDoItem.Name.Equals(this.newTodoItem.Name))
                {
                    throw new Exception("Name was not updated");
                }
            }
        }

        private bool IsXmlResponse() => this.options != null && this.options.ResponseType != null &&
                                      this.options.ResponseType == typeof(XmlResponseData);
    }
}

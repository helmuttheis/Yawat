﻿namespace YawatServer.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Yawat.Models;
    using YawatServer.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class BaTodoItemsController : ControllerBase
    {
        private readonly TodoContext context;

        public BaTodoItemsController(TodoContext context)
        {
            this.context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetBaTodoItems()
        {
            return await this.context.TodoItems
                .Select(x => ItemToDto(x))
                .ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public async Task<ActionResult<TodoItemDto>> GetBaTodoItem(long id)
        {
            var todoItem = await this.context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return this.NotFound();
            }

            return ItemToDto(todoItem);
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public async Task<IActionResult> PutBaTodoItem(long id, TodoItemDto todoItemDto)
        {
            if (id != todoItemDto.Id)
            {
                return this.BadRequest();
            }

            var todoItem = await this.context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return this.NotFound();
            }

            todoItem.Name = todoItemDto.Name;
            todoItem.IsComplete = todoItemDto.IsComplete;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!this.TodoItemExists(id))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public async Task<ActionResult<TodoItem>> PostBaTodoItem(TodoItemDto todoItemDto)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDto.IsComplete,
                Name = todoItemDto.Name
            };

            this.context.TodoItems.Add(todoItem);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction(
                nameof(this.GetBaTodoItem),
                new { id = todoItem.Id },
                ItemToDto(todoItem));
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public async Task<ActionResult<TodoItem>> DeleteBaTodoItem(long id)
        {
            var todoItem = await this.context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return this.NotFound();
            }

            this.context.TodoItems.Remove(todoItem);
            await this.context.SaveChangesAsync();

            return todoItem;
        }

        private static TodoItemDto ItemToDto(TodoItem todoItem) =>
            new TodoItemDto
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };

        private bool TodoItemExists(long id)
        {
            return this.context.TodoItems.Any(e => e.Id == id);
        }
    }
}

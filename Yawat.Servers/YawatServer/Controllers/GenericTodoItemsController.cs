using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YawatServer.Models;
using YawatServer.Models.TodoApi.Models;

namespace YawatServer.Controllers
{
    public class GenericTodoItemsController : ControllerBase
    {
        private readonly TodoContext context;

        public GenericTodoItemsController(TodoContext context)
        {
            this.context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetGenericTodoItems()
        {
            return await this.context.TodoItems
                .Select(x => ItemToDto(x))
                .ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetGenericTodoItem(long id)
        {
            var todoItem = await this.context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDto(todoItem);
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenericTodoItem(long id, TodoItemDto todoItemDto)
        {
            if (id != todoItemDto.Id)
            {
                return BadRequest();
            }

            var todoItem = await this.context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoItemDto.Name;
            todoItem.IsComplete = todoItemDto.IsComplete;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostGenericTodoItem(TodoItemDto todoItemDto)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDto.IsComplete,
                Name = todoItemDto.Name
            };

            this.context.TodoItems.Add(todoItem);
            await this.context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetGenericTodoItem),
                new { id = todoItem.Id },
                ItemToDto(todoItem));
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteGenericTodoItem(long id)
        {
            var todoItem = await this.context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            this.context.TodoItems.Remove(todoItem);
            await this.context.SaveChangesAsync();

            return todoItem;
        }

        private bool TodoItemExists(long id)
        {
            return this.context.TodoItems.Any(e => e.Id == id);
        }
        private static TodoItemDto ItemToDto(TodoItem todoItem) =>
            new TodoItemDto
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
    }
}

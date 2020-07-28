using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Okta.AspNetCore;
using YawatServer.Models;
using YawatServer.Models.TodoApi.Models;

namespace YawatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OktaTodoItemsController: ControllerBase
    {
        private readonly TodoContext context;

        public OktaTodoItemsController(TodoContext context)
        {
            this.context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        [Authorize(AuthenticationSchemes = OktaDefaults.ApiAuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetBaTodoItems()
        {
            return await this.context.TodoItems
                .Select(x => ItemToDto(x))
                .ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = OktaDefaults.ApiAuthenticationScheme)]
        public async Task<ActionResult<TodoItemDto>> GetBaTodoItem(long id)
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
        [Authorize(AuthenticationSchemes = OktaDefaults.ApiAuthenticationScheme)]
        public async Task<IActionResult> PutBaTodoItem(long id, TodoItemDto todoItemDto)
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
        [Authorize(AuthenticationSchemes = OktaDefaults.ApiAuthenticationScheme)]
        public async Task<ActionResult<TodoItem>> PostBaTodoItem(TodoItemDto todoItemDto)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDto.IsComplete,
                Name = todoItemDto.Name
            };

            this.context.TodoItems.Add(todoItem);
            await this.context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBaTodoItem),
                new { id = todoItem.Id },
                ItemToDto(todoItem));
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = OktaDefaults.ApiAuthenticationScheme)]
        public async Task<ActionResult<TodoItem>> DeleteBaTodoItem(long id)
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

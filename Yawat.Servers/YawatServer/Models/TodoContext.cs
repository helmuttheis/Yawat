namespace YawatServer.Models
{
    using Microsoft.EntityFrameworkCore;
    using Yawat.Models;

    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}

namespace YawatServer.SeedData
{
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Yawat.Models;
    using YawatServer.Models;

    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceScopeFactory scopeFactory;

        public DbInitializer(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public void SeedData()
        {
            using var serviceScope = this.scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<TodoContext>();
            context.Database.EnsureCreated();

            if (!context.TodoItems.Any())
            {
                context.TodoItems.Add(new TodoItem
                {
                    Id = 1,
                    Name = "walk dog",
                    IsComplete = true
                });
            }

            context.SaveChanges();
        }
    }
}

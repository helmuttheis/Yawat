namespace YawatServer.SeedData
{
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;
    using YawatServer.Models;
    using YawatServer.Models.TodoApi.Models;

    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceScopeFactory scopeFactory;

        public DbInitializer(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public void SeedData()
        {
            using var serviceScope = scopeFactory.CreateScope();
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

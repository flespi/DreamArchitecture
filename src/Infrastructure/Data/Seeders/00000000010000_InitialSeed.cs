using EFSeeder;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Data.Seeders;

[DbContext(typeof(ApplicationDbContext))]
[DataSeeder("00000000010000_InitialSeed")]
public class InitialSeed : IDataSeeder<ApplicationDbContext>
{
    private readonly ILogger<InitialSeed> _logger;

    public InitialSeed(ILogger<InitialSeed> logger)
    {
        _logger = logger;
    }

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        // Default data
        // Seed, if necessary
        context.TodoLists.Add(new TodoList
        {
            Title = "Todo List",
            Items =
            {
                new TodoItem { Title = "Make a todo list 📃" },
                new TodoItem { Title = "Check off the first item ✅" },
                new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
            }
        });

        await context.SaveChangesAsync();
    }
}

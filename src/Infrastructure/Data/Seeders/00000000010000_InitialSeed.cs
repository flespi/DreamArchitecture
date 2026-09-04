using System.Security.Claims;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Common.Identity;
using CleanArchitecture.Domain.Entities;
using EFSeeder;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Data.Seeders;

[DbContext(typeof(ApplicationDbContext))]
[DataSeeder("00000000010000_InitialSeed")]
public class InitialSeed : IDataSeeder<ApplicationDbContext>
{
    private readonly ILogger<InitialSeed> _logger;

    private readonly IIdentityAccessor _identityAccessor;

    public InitialSeed(ILogger<InitialSeed> logger, IIdentityAccessor identityAccessor)
    {
        _logger = logger;
        _identityAccessor = identityAccessor;
    }

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        var identity = CreateIdentity("default", "default");
        var principal = new ClaimsPrincipal(identity);

        using var impersonation = _identityAccessor.Impersonate(principal);

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

    private static ClaimsIdentity CreateIdentity(string subject, string userName)
    {
        var claims = new List<Claim>
        {
            new(DefaultClaimTypes.Subject, subject),
            new(DefaultClaimTypes.Name, userName),
        };

        return new ClaimsIdentity(claims, "Seed", DefaultClaimTypes.Name, DefaultClaimTypes.Role);
    }
}

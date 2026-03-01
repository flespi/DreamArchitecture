using EFSeeder;
using CleanArchitecture.Domain.Constants;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Orca;

namespace CleanArchitecture.Infrastructure.Data.Seeders;

[DbContext(typeof(ApplicationDbContext))]
[DataSeeder("00000000000000_AuthSeed")]
public class AuthSeed : IDataSeeder<ApplicationDbContext>
{
    private readonly ILogger<AuthSeed> _logger;
    private readonly IOrcaStoreAccessor _authContext;

    public AuthSeed(ILogger<AuthSeed> logger, IOrcaStoreAccessor authContext)
    {
        _logger = logger;
        _authContext = authContext;
    }

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        // Default permissions
        var canPurgePermission = new Permission
        {
            Name = Policies.CanPurge
        };

        await _authContext.PermissionStore.CreateAsync(canPurgePermission);

        // Default roles
        var administratorRole = new Role
        {
            Name = Roles.Administrator,
        };

        await _authContext.RoleStore.CreateAsync(administratorRole);
        await _authContext.RoleStore.AddPermissionAsync(administratorRole, canPurgePermission);

        // Default users
        var administrator = new Subject
        {
            Sub = Guid.NewGuid().ToString(),
            Name = "administrator",
            Email = "administrator@host.local"
        };

        await _authContext.SubjectStore.CreateAsync(administrator);
        await _authContext.SubjectStore.AddRoleAsync(administrator, administratorRole);
    }
}

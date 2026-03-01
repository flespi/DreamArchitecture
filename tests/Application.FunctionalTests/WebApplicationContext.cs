using System.Data.Common;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Shared.FunctionalTests;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace CleanArchitecture.Application.FunctionalTests;

public class WebApplicationContext : WebApplicationContext<Program>
{
    public IdentityResolver IdentityResolver { get; } = new();

    protected override IDatabaseContainer CreateDatabaseContainer()
        => new MsSqlBuilder().Build();

    protected override DbConnection CreateDatabaseConnection()
        => new SqlConnection(Container.GetConnectionString());

    protected override WebApplicationFactory<Program> CreateWebApplicationFactory(IDatabaseContainer container)
        => new WebApplicationFactory(container, IdentityResolver);

    public override async Task ResetState()
    {
        await base.ResetState();

        IdentityResolver.Principal = null;
    }

    protected override async Task InitializeData()
    {
        using var scope = CreateServiceScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();
    }
}

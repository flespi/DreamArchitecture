using DotNet.Testcontainers.Containers;
using EFSeeder;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CleanArchitecture.Application.Common.Processing;
using CleanArchitecture.Application.Common.Identity;

namespace CleanArchitecture.Application.FunctionalTests;

public class WebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly IDatabaseContainer _container;

    private readonly IIdentityResolver _identityResolver;

    public WebApplicationFactory(IDatabaseContainer container, IIdentityResolver identityResolver)
    {
        _container = container;
        _identityResolver = identityResolver;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => ConfigureTestServices(services));
    }

    private void ConfigureTestServices(IServiceCollection services)
    {
        services
            .RemoveAll<DbContextOptions<ApplicationDbContext>>()
            .AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var connectionString = _container.GetConnectionString();
                var interceptors = sp.GetServices<ISaveChangesInterceptor>();

                var seeder = sp.GetRequiredService<DbContextSeeder<ApplicationDbContext>>();

                options
                    .UseSqlServer(connectionString)
                    .AddInterceptors(interceptors)
                    .UseAsyncSeeding(seeder);
            });

        services
            .RemoveAll<IIdentityResolver>()
            .AddSingleton(_identityResolver);

        services
            .RemoveAll<IIdempotentRequest>()
            .AddScoped<IIdempotentRequest, IdempotentRequest>();
    }
}

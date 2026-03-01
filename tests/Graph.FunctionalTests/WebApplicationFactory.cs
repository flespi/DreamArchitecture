using DotNet.Testcontainers.Containers;
using EFSeeder;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Graph.FunctionalTests;

public class WebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly IDatabaseContainer _container;

    public WebApplicationFactory(IDatabaseContainer container)
    {
        _container = container;
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
            .RemoveAll<IOptions<JwtBearerOptions>>()
            .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .PostConfigure(options =>
            {
                options.TokenValidationParameters.NameClaimType = "name";
                options.TokenValidationParameters.RoleClaimType = "role";

                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = JwtHelper.SigningKey,

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
            });
    }
}

using CleanArchitecture.Graph.FunctionalTests.Internal.Helpers;
using CleanArchitecture.Infrastructure.Data;
using EFSeeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Graph.FunctionalTests;

public static class DependencyInjection
{
    public static void AddTestServices(this IServiceCollection services, RegistralOptions registralOptions)
    {
        services
            .RemoveDbContext<ApplicationDbContext>()
            .AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var connectionString = registralOptions.Container.GetConnectionString();
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

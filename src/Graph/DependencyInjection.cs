using CleanArchitecture.Application;
using CleanArchitecture.Application.Common.Identity;
using CleanArchitecture.Application.Common.Processing;
using CleanArchitecture.Graph.Services;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddWebServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<IIdentityResolver, IdentityResolver>();
        services.AddScoped<IIdempotentRequest, IdempotentRequest>();

        services.AddHttpContextAccessor();
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddAuthServices();
    }

    private static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .BindConfiguration("Authentication:Schemes:Bearer")
            .PostConfigure(options =>
            {
                options.TokenValidationParameters.NameClaimType = DefaultClaimTypes.Name;
                options.TokenValidationParameters.RoleClaimType = DefaultClaimTypes.Role;
        
                options.MapInboundClaims = false;
            });

        return services;
    }
}

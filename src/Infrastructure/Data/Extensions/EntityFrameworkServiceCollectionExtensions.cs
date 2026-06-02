using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanArchitecture.Infrastructure.Data;

public static class EntityFrameworkServiceCollectionExtensions
{
    public static IServiceCollection RemoveDbContext<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services
            .RemoveAll<TContext>()
            .RemoveAll<DbContextOptions<TContext>>()
            .RemoveAll<IDbContextFactory<TContext>>()
            .RemoveAll<IDbContextOptionsConfiguration<TContext>>();

        return services;
    }
}

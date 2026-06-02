using CleanArchitecture.Graph.Infrastructure.Filters;
using CleanArchitecture.Graph.Infrastructure.Interceptors;
using HotChocolate.Execution.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddGraphServices(this IServiceCollection services, out IRequestExecutorBuilder builder)
    {
        builder = services
            .AddGraphQLServer()
            .AddGraphQLTypes()

            // Authorization
            .AddAuthorization()

            // Schema and types
            .AddGlobalObjectIdentification()

            // Additional features
            .AddQueryContext()
            .AddPagingArguments()
            .AddFiltering()
            .AddSorting()

            // Conventions
            .AddMutationConventions()

            // Infrastructure (interceptors, filters, etc.)
            .AddHttpRequestInterceptor<IdempotentRequestInterceptor>()
            .AddErrorFilter<GraphQLErrorFilter>();
    }

    public static void AddGraphServices(this IServiceCollection services, Action<IRequestExecutorBuilder>? builderAction = null)
    {
        AddGraphServices(services, out var builder);

        if (builderAction is not null)
        {
            builderAction(builder);
        }
    }
}

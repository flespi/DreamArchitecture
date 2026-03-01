using CleanArchitecture.Graph.Infrastructure.Filters;
using CleanArchitecture.Graph.Infrastructure.Interceptors;
using HotChocolate.Execution.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddGraphServices(this IServiceCollection services, Action<IRequestExecutorBuilder>? builderAction = null)
    {
        var builder = services
            .AddGraphQLServer()
            .AddGrapgQLTypes()

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

        if (builderAction is not null)
        {
            builderAction(builder);
        }

        return services;
    }
}

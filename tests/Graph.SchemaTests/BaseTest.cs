namespace CleanArchitecture.Graph.SchemaTests;

// [Collection(AppTestCollection.Name)]
public abstract class BaseTest : IAsyncLifetime, IClassFixture<AppTestContext>
{
    protected AppTestContext Context { get; }

    public BaseTest(AppTestContext context)
    {
        Context = context;
    }

    public async ValueTask InitializeAsync()
    {
        await Context.ResetAsync();
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public async Task<IExecutionResult> ExecuteRequestAsync(
        OperationRequestBuilder requestBuilder,
        CancellationToken cancellationToken = default)
    {
        using var scope = Context.ServiceScopeFactory.CreateScope();

        requestBuilder.SetServices(scope.ServiceProvider);
        var request = requestBuilder.Build();

        var executor = await Context.CreateRequestExecutorAsync();
        var result = await executor.ExecuteAsync(request, cancellationToken);
        result.RegisterForCleanup(scope);

        return result;
    }

    public async Task<IExecutionResult> ExecuteRequestAsync(
        Action<OperationRequestBuilder> requestBuilderOptions,
        CancellationToken cancellationToken = default)
    {
        var requestBuilder = new OperationRequestBuilder();
        requestBuilderOptions(requestBuilder);

        return await ExecuteRequestAsync(requestBuilder, cancellationToken);
    }
}

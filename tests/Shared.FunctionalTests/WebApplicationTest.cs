namespace CleanArchitecture.Shared.FunctionalTests;

public abstract class WebApplicationTest<TEntryPoint> : IAsyncLifetime
    where TEntryPoint : class
{
    protected WebApplicationContext<TEntryPoint> Context { get; }

    public WebApplicationTest(WebApplicationContext<TEntryPoint> context)
    {
        Context = context;
    }

    public virtual async ValueTask InitializeAsync()
        => await Context.ResetState();

    public virtual ValueTask DisposeAsync()
        => ValueTask.CompletedTask;
}

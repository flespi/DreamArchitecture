using CleanArchitecture.Application.Common.Data;

namespace CleanArchitecture.Infrastructure.Data.Implementations;

public class FakeTransaction : Transaction
{
    protected override Task ExecuteCommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    protected override Task ExecuteRollbackAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}

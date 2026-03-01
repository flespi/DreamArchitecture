using CleanArchitecture.Application.Common.Identity;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IIdentityAccessor __identityAccessor;

    public LoggingBehaviour(ILogger<TRequest> logger, IIdentityAccessor _identityAccessor)
    {
        _logger = logger;
        __identityAccessor = _identityAccessor;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = __identityAccessor.Principal?.GetSubject() ?? string.Empty;
        string? userName = string.Empty;

        if (!string.IsNullOrEmpty(userId))
        {
            userName = __identityAccessor.Principal?.GetName();
        }

        _logger.LogInformation("CleanArchitecture Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);

        return Task.CompletedTask;
    }
}

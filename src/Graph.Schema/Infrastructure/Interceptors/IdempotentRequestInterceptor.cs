using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Graph.Infrastructure.Interceptors;

public class IdempotentRequestInterceptor : DefaultHttpRequestInterceptor
{
    public override ValueTask OnCreateAsync(HttpContext context, IRequestExecutor requestExecutor, OperationRequestBuilder requestBuilder, CancellationToken cancellationToken)
    {
        if (context.Request.Headers.TryGetValue("Idempotency-Key", out var value))
        {
            requestBuilder.SetGlobalState("Idempotency-Key", value);
        }

        return base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
    }
}

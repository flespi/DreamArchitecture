namespace CleanArchitecture.Graph.SchemaTests.Internal.Extensions;

public static class RequestContextAccessorExtensions
{
    public static bool TryGetRequestContext(this IRequestContextAccessor requestContextAccessor, out RequestContext? requestContext)
    {
        try
        {
            requestContext = requestContextAccessor.RequestContext;
            return true;
        }
        catch (Exception)
        {
            requestContext = null;
            return false;
        }
    }
}

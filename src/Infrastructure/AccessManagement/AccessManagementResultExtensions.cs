using CleanArchitecture.Application.Common.Models;
using Orca;

namespace CleanArchitecture.Infrastructure.AccessManagement;

public static class AccessManagementResultExtensions
{
    public static Result ToApplicationResult(this AccessManagementResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }
}

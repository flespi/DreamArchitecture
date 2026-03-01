using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.PriorityLevels.Queries.GetPriorityLevels;

[Authorize]
public record GetPriorityLevelsQuery : IRequest<IReadOnlyCollection<LookupDto<int>>>;

public class GetPriorityLevelsQueryHandler : IRequestHandler<GetPriorityLevelsQuery, IReadOnlyCollection<LookupDto<int>>>
{
    public Task<IReadOnlyCollection<LookupDto<int>>> Handle(GetPriorityLevelsQuery request, CancellationToken cancellationToken)
    {
        var values = Enum.GetValues(typeof(PriorityLevel))
                .Cast<PriorityLevel>()
                .Select(p => new LookupDto<int> { Id = (int)p, Title = p.ToString() })
                .ToList();

        return Task.FromResult<IReadOnlyCollection<LookupDto<int>>>(values);
    }
}

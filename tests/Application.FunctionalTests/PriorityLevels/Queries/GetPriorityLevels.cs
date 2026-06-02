using CleanArchitecture.Application.PriorityLevels.Queries.GetPriorityLevels;

namespace CleanArchitecture.Application.FunctionalTests.PriorityLevels.Queries;

public class GetPriorityLevelsTests : BaseTest
{
    public GetPriorityLevelsTests(AppTestContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldReturnPriorityLevels()
    {
        await RunAsDefaultUserAsync();

        var query = new GetPriorityLevelsQuery();

        var result = await SendAsync(query);

        result.ShouldNotBeEmpty();
    }
}

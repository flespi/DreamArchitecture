using CleanArchitecture.Graph.SchemaTests.Internal;
using CleanArchitecture.Graph.SchemaTests.Internal.Helpers;

namespace CleanArchitecture.Graph.SchemaTests.TodoLists.Queries;

using static OperationRequestHelper;

public class TodoListMutationTests : BaseTest
{
    public static ISpecContext SpecContext { get; } = CookieCrumble.Spec.SpecContext.Create();

    public static ISpecTestCollection Specs { get; } = new SpecTestLoader(SpecContext).Load();

    public static TheoryData<SpecTest> TestData { get; } = new SpecTestData(Specs);

    public TodoListMutationTests(AppTestContext context) : base(context)
    {
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public async Task ExecuteTheory(SpecTest spec)
    {
        var user = CreateUser(spec.Identity);

        var requestBuilder = new OperationRequestBuilder()
            .SetDocument(spec.Document)
            .SetOperationName(spec.OperationName)
            .SetVariableValues(spec.Variables)
            .SetUser(user);

        await using var result = await ExecuteRequestAsync(requestBuilder, TestContext.Current.CancellationToken);

        result.MatchSpecSnapshot(spec.Snapshot);
    }
}

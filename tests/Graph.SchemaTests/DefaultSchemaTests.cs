namespace CleanArchitecture.Graph.SchemaTests;

public class DefaultSchemaTests : BaseTest
{
    public DefaultSchemaTests(AppTestContext context) : base(context)
    {
    }

    [Fact]
    public async Task Should_MatchSchema()
    {
        var executor = await Context.CreateRequestExecutorAsync();
        executor.Schema.MatchSnapshot();
    }
}

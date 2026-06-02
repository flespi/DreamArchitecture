namespace CleanArchitecture.Graph.SchemaTests;

[CollectionDefinition(Name)]
public class AppTestCollection : ICollectionFixture<AppTestContext>
{
    public const string Name = nameof(AppTestCollection);
}

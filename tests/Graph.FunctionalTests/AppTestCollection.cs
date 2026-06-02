namespace CleanArchitecture.Graph.FunctionalTests;

[CollectionDefinition(Name)]
public class AppTestCollection : ICollectionFixture<AppTestContext>
{
    public const string Name = nameof(AppTestCollection);
}

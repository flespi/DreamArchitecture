namespace CleanArchitecture.Application.FunctionalTests;

[CollectionDefinition(Name)]
public class WebApplicationTestCollection : ICollectionFixture<WebApplicationContext>
{
    public const string Name = nameof(WebApplicationTestCollection);
}

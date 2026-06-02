namespace CleanArchitecture.Graph.SchemaTests.Internal;

public class SpecTestData : TheoryData<SpecTest>
{
    public SpecTestData(ISpecTestCollection collection)
    {
        foreach (var item in collection)
        {
            if (!item.Ignore)
            {
                Add(item);
            }
        }
    }
}

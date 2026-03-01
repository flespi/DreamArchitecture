using StrawberryShake;

namespace CleanArchitecture.Graph.FunctionalTests.TodoLists.Queries;

public class GetTodosTests : WebApplicationTest
{
    public GetTodosTests(WebApplicationContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldReturnAllListsAndItems()
    {
        await RunAsDefaultUserAsync();

        var createResponse = await Client.CreateTodoList.ExecuteAsync(new()
        {
            Data = new()
            {
                Title = "Shopping",
                Items = [
                    new AddTodoListItemDataInput { Title = "Apples" },
                    new AddTodoListItemDataInput { Title = "Milk" },
                    new AddTodoListItemDataInput { Title = "Bread" },
                    new AddTodoListItemDataInput { Title = "Toilet paper" },
                    new AddTodoListItemDataInput { Title = "Pasta" },
                    new AddTodoListItemDataInput { Title = "Tissues" },
                    new AddTodoListItemDataInput { Title = "Tuna" },
                ]
            }
        }, TestContext.Current.CancellationToken);
        
        createResponse.IsSuccessResult().ShouldBeTrue();

        var response = await Client.GetTodoLists.ExecuteAsync(TestContext.Current.CancellationToken);
        response.IsSuccessResult().ShouldBeTrue();

        var result = response.Data;

        result!.TodoLists!.Nodes.ShouldNotBeNull();
        result!.TodoLists!.Nodes.Count.ShouldBe(2);

        result!.TodoLists!.Nodes!.Skip(1).First().Items.ShouldNotBeNull();
        result!.TodoLists!.Nodes!.Skip(1).First().Items!.Count.ShouldBe(7);
    }

    [Fact]
    public async Task ShouldDenyAnonymousUser()
    {
        var response = await Client.GetTodoLists.ExecuteAsync(TestContext.Current.CancellationToken);

        response.IsSuccessResult().ShouldBeFalse();

        response.Errors.First().Code.ShouldBe("AUTH_NOT_AUTHENTICATED");
    }
}

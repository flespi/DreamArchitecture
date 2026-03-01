using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;
using GreenDonut.Data;

namespace CleanArchitecture.Application.FunctionalTests.TodoLists.Queries;

public class GetTodosTests : WebApplicationTest
{
    public GetTodosTests(WebApplicationContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldReturnAllListsAndItems()
    {
        await RunAsDefaultUserAsync();

        await AddAsync(new TodoList
        {
            Title = "Shopping",
            Colour = Colour.Blue,
            Items =
                    {
                        new TodoItem { Title = "Apples", Done = true },
                        new TodoItem { Title = "Milk", Done = true },
                        new TodoItem { Title = "Bread", Done = true },
                        new TodoItem { Title = "Toilet paper" },
                        new TodoItem { Title = "Pasta" },
                        new TodoItem { Title = "Tissues" },
                        new TodoItem { Title = "Tuna" }
                    }
        });

        var query = new GetTodosQuery
        {
            Specification = new QueryContext<TodoList>()
        };

        var result = await SendAsync(query);

        result.Count().ShouldBe(2);
        result.Skip(1).First().Items.Count.ShouldBe(7);
    }

    [Fact]
    public async Task ShouldDenyAnonymousUser()
    {
        var query = new GetTodosQuery();

        var action = () => SendAsync(query);

        await Should.ThrowAsync<UnauthorizedAccessException>(action);
    }
}

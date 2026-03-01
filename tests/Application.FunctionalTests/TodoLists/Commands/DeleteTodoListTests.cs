using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.DeleteTodoList;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.FunctionalTests.TodoLists.Commands;

public class DeleteTodoListTests : WebApplicationTest
{
    public DeleteTodoListTests(WebApplicationContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new DeleteTodoListCommand { Id = Guid.Empty };
        await Should.ThrowAsync<NotFoundException>(() => SendAsync(command));
    }

    [Fact]
    public async Task ShouldDeleteTodoList()
    {
        var list = await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "New List",
            }
        });

        await SendAsync(new DeleteTodoListCommand { Id = list.Id });

        list = await FindAsync<TodoList>(list.Id);

        list.ShouldBeNull();
    }
}

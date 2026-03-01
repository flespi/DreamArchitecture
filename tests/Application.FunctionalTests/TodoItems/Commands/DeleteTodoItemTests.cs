using CleanArchitecture.Application.TodoLists.Commands.AddTodoListItem;
using CleanArchitecture.Application.TodoLists.Commands.RemoveTodoListItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.FunctionalTests.TodoItems.Commands;

public class DeleteTodoItemTests : WebApplicationTest
{
    public DeleteTodoItemTests(WebApplicationContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new RemoveTodoListItemCommand() { ListId = Guid.Empty, ItemId = Guid.Empty };

        await Should.ThrowAsync<NotFoundException>(() => SendAsync(command));
    }

    [Fact]
    public async Task ShouldDeleteTodoItem()
    {
        var list = await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "New List",
            }
        });

        var item = await SendAsync(new AddTodoListItemCommand
        {
            ListId = list.Id,
            Data = new()
            {
                Title = "New Item",
            }
        });

        await SendAsync(new RemoveTodoListItemCommand() { ListId = list.Id, ItemId = item.Id });

        item = await FindAsync<TodoItem>(item.Id);

        item.ShouldBeNull();
    }
}

using CleanArchitecture.Application.TodoLists.Commands.AddTodoListItem;
using CleanArchitecture.Application.TodoLists.Commands.EditTodoListItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.FunctionalTests.TodoItems.Commands;

public class UpdateTodoItemTests : WebApplicationTest
{
    public UpdateTodoItemTests(WebApplicationContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new EditTodoListItemCommand
        {
            ListId = Guid.Empty,
            ItemId = Guid.Empty,
            Data = new()
            {
                Title = "New Title",
            }
        };

        await Should.ThrowAsync<NotFoundException>(() => SendAsync(command));
    }

    [Fact]
    public async Task ShouldUpdateTodoItem()
    {
        var userId = await RunAsDefaultUserAsync();

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

        var command = new EditTodoListItemCommand
        {
            ListId = list.Id,
            ItemId = item.Id,
            Data = new()
            {
                Title = "Updated Item Title",
            }
        };

        await SendAsync(command);

        item = await FindAsync<TodoItem>(item.Id);

        item.ShouldNotBeNull();
        item!.Title.ShouldBe(command.Data.Title);
    }
}

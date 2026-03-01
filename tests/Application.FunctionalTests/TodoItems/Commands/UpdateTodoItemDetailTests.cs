using CleanArchitecture.Application.TodoLists.Commands.AddTodoListItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.EditTodoListItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using Shouldly;

namespace CleanArchitecture.Application.FunctionalTests.TodoItems.Commands;

public class UpdateTodoItemDetailTests : WebApplicationTest
{
    public UpdateTodoItemDetailTests(WebApplicationContext context) : base(context)
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
            ItemId = item.Id,
            ListId = list.Id,
            Data = new()
            {
                Note = "This is the note.",
                Priority = PriorityLevel.High
            }
        };

        await SendAsync(command);

        item = await FindAsync<TodoItem>(item.Id);

        item.ShouldNotBeNull();
        item!.ListId.ShouldBe(command.ListId);
        item.Note.ShouldBe(command.Data.Note);
        item.Priority.ShouldBe(command.Data.Priority.Value!.Value);
    }
}

using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoLists.Commands.AddTodoListItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.FunctionalTests.TodoItems.Commands;

public class CreateTodoItemTests : WebApplicationTest
{
    public CreateTodoItemTests(WebApplicationContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new AddTodoListItemCommand
        {
            ListId = Guid.NewGuid(),
            Data = new()
            {
                Title = string.Empty
            }
        };

        await Should.ThrowAsync<ValidationException>(() => SendAsync(command));
    }

    [Fact]
    public async Task ShouldCreateTodoItem()
    {
        var userId = await RunAsDefaultUserAsync();

        var list = await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "New List",
            }
        });

        var command = new AddTodoListItemCommand
        {
            ListId = list.Id,
            Data = new()
            {
                Title = "Tasks",
            }
        };

        var item = await SendAsync(command);

        item = await FindAsync<TodoItem>(item.Id);

        item.ShouldNotBeNull();
        item!.ListId.ShouldBe(command.ListId);
        item.Title.ShouldBe(command.Data.Title);
    }
}

using System.Globalization;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.FunctionalTests.TodoLists.Commands;

public class UpdateTodoListTests : WebApplicationTest
{
    public UpdateTodoListTests(WebApplicationContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new UpdateTodoListCommand
        {
            Id = Guid.Empty,
            Data = new()
            {
                Title = "New Title",
            }
        };

        await Should.ThrowAsync<NotFoundException>(() => SendAsync(command));
    }

    [Fact]
    public async Task ShouldRequireUniqueTitle()
    {
        CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

        var list = await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "New List",
            }
        });

        await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "Other List",
            }
        });

        var command = new UpdateTodoListCommand
        {
            Id = list.Id,
            Data = new()
            {
                Title = "Other List",
            }
        };

        var ex = await Should.ThrowAsync<ValidationException>(() => SendAsync(command));

        ex.Errors.ShouldContainKey("Data.Title.Value");
        ex.Errors["Data.Title.Value"].ShouldContain("'Title' must be unique.");
    }

    [Fact]
    public async Task ShouldUpdateTodoList()
    {
        var userId = await RunAsDefaultUserAsync();

        var list = await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "New List",
            }
        });

        var command = new UpdateTodoListCommand
        {
            Id = list.Id,
            Data = new()
            {
                Title = "Updated List Title",
            }
        };

        await SendAsync(command);

        list = await FindAsync<TodoList>(list.Id);

        list.ShouldNotBeNull();
        list!.Title.ShouldBe(command.Data.Title);
        list.Audit.LastModifiedBy.ShouldNotBeNull();
        list.Audit.LastModifiedBy.ShouldBe(userId);
        list.Audit.LastModified.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}

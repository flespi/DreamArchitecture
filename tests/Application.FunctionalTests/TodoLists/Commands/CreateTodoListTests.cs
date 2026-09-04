using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.FunctionalTests.TodoLists.Commands;

public class CreateTodoListTests : BaseTest
{
    public CreateTodoListTests(AppTestContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateTodoListCommand
        {
            Data = new()
            {
                Title = string.Empty
            }
        };

        await Should.ThrowAsync<ValidationException>(() => SendAsync(command));
    }

    [Fact]
    public async Task ShouldRequireUniqueTitle()
    {
        var userId = await RunAsDefaultUserAsync();

        await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "Shopping",
            }
        });

        var command = new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "Shopping",
            }
        };

        await Should.ThrowAsync<ValidationException>(() => SendAsync(command));
    }

    [Fact]
    public async Task ShouldCreateTodoList()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "Tasks",
            }
        };

        var list = await SendAsync(command);

        list = await FindAsync<TodoList>(list.Id);

        list.ShouldNotBeNull();
        list!.Title.ShouldBe(command.Data.Title);
        list.Audit.CreatedBy.ShouldBe(userId);
        list.Audit.Created.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}

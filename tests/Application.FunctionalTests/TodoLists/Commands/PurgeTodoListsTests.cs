using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.PurgeTodoLists;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.FunctionalTests.TodoLists.Commands;

public class PurgeTodoListsTests : WebApplicationTest
{
    public PurgeTodoListsTests(WebApplicationContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new PurgeTodoListsCommand();

        command.GetType().ShouldBeDecoratedWith<AuthorizeAttribute>();

        var action = () => SendAsync(command);

        await Should.ThrowAsync<UnauthorizedAccessException>(action);
    }

    [Fact]
    public async Task ShouldDenyNonAdministrator()
    {
        await RunAsDefaultUserAsync();

        var command = new PurgeTodoListsCommand();

        var action = () => SendAsync(command);

        await Should.ThrowAsync<ForbiddenAccessException>(action);
    }

    [Fact]
    public async Task ShouldAllowAdministrator()
    {
        await RunAsAdministratorAsync();

        var command = new PurgeTodoListsCommand();

        Func<Task> asyncAction = async () => await SendAsync(command);
        await asyncAction.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task ShouldDeleteAllLists()
    {
        await RunAsAdministratorAsync();

        await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "New List #1",
            }
        });

        await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "New List #2",
            }
        });

        await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "New List #3",
            }
        });

        await SendAsync(new PurgeTodoListsCommand());

        var count = await CountAsync<TodoList>();

        count.ShouldBe(0);
    }
}

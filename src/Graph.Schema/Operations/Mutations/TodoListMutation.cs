using CleanArchitecture.Application.Common.Processing;
using CleanArchitecture.Application.TodoLists.Commands.AddTodoListItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.DeleteTodoList;
using CleanArchitecture.Application.TodoLists.Commands.EditTodoListItem;
using CleanArchitecture.Application.TodoLists.Commands.RemoveTodoListItem;
using CleanArchitecture.Application.TodoLists.Commands.Shared;
using CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;
using CleanArchitecture.Domain.Entities;
using HotChocolate.Authorization;

namespace CleanArchitecture.Graph.Operations.Mutations;

[Authorize]
[MutationType]
public static class TodoListMutation
{
    public static async Task<TodoList> CreateTodoList(CreateTodoListData data, [Service] ISender sender)
    {
        return await sender.Send(new CreateTodoListCommand
        {
            Data = data
        });
    }

    public static async Task<TodoList> UpdateTodoList([ID] Guid id, UpdateTodoListData data, Condition? condition, [Service] ISender sender)
    {
        return await sender.Send(new UpdateTodoListCommand
        {
            Id = id,
            Data = data,
            Condition = condition
        });
    }

    public static async Task<TodoList> DeleteTodoList([ID] Guid id, [Service] ISender sender)
    {
        return await sender.Send(new DeleteTodoListCommand
        {
            Id = id
        });
    }

    public static async Task<TodoItem> AddTodoListItem([ID] Guid listId, AddTodoListItemData data, [Service] ISender sender)
    {
        return await sender.Send(new AddTodoListItemCommand
        {
            ListId = listId,
            Data = data
        });
    }

    public static async Task<TodoItem> EditTodoListItem([ID] Guid listId, [ID] Guid itemId, EditTodoListItemData data, Condition? condition, [Service] ISender sender)
    {
        return await sender.Send(new EditTodoListItemCommand
        {
            ListId = listId,
            ItemId = itemId,
            Data = data,
            Condition = condition
        });
    }

    public static async Task<TodoItem> RemoveTodoListItem([ID] Guid listId, [ID] Guid itemId, [Service] ISender sender)
    {
        return await sender.Send(new RemoveTodoListItemCommand
        {
            ListId = listId,
            ItemId = itemId
        });
    }
}

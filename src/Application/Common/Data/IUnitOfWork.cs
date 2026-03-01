using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Data;

public interface IUnitOfWork : IUnitOfWorkBase
{
    IRepository<TodoList> TodoList => Repository<TodoList>();
}

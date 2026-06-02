using System.Data.Common;
using CleanArchitecture.Infrastructure.Data.Abstractions;

namespace CleanArchitecture.Infrastructure.Data.Implementations;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly Func<DbConnection> _factory;

    public DbConnectionFactory(Func<DbConnection> factory)
    {
        _factory = factory;
    }

    public DbConnection CreateConnection() => _factory();
}
